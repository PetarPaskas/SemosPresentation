using Amazon.S3.Util;
using DocumentService.BackgroundFileProcessing.Domain;
using DocumentService.BackgroundFileProcessing.Mappers;
using DocumentService.BackgroundFileProcessing.Validators;
using DocumentService.Domain.Clients.S3;
using DocumentService.Domain.Clients.SQS;
using DocumentService.Domain.Contracts;
using DocumentService.Domain.Persistence;
using System.Text.Json;


namespace DocumentService.BackgroundFileProcessing.Processes;

public class FileGenerationProcess : IFileGenerationProcess
{
    private IFileGenerator<XlsFileMetadata> _fileGenerator;
    private IXlsFileInputDeliveryContentV1Validator _xlsFileInputDeliveryContentValidator;
    private IS3Client _s3Client;
    private ISqsClient _sqsClient;
    private IMapper<XlsFileInputDeliveryContentV1, XlsFileMetadata> _mapper;
    private IMetadataRepository _metadataRepository;

    public FileGenerationProcess(
        IFileGenerator<XlsFileMetadata> fileGenerator,
        IXlsFileInputDeliveryContentV1Validator xlsFileInputDeliveryContentValidator,
        IMapper<XlsFileInputDeliveryContentV1, XlsFileMetadata> mapper,
        IS3Client s3Client, 
        ISqsClient sqsClient,
        IMetadataRepository metadataRepository)
    {
        _fileGenerator = fileGenerator;
        _xlsFileInputDeliveryContentValidator = xlsFileInputDeliveryContentValidator;
        _s3Client = s3Client;
        _sqsClient = sqsClient;
        _mapper = mapper;
        _metadataRepository = metadataRepository;
    }

    public async Task Process()
    {
        SQSReceiveMessageResponse message = null;
        try
        {
            message = await TryReceiveMessageOrEmpty();

            if (message == null)
                return;

            var (contract, objectPath) = await TryReceiveContractOrEmpty(message);

            var validationResult = _xlsFileInputDeliveryContentValidator.Validate(contract);
            if (!validationResult.IsValid)
                throw new Exception(String.Join(' ', validationResult.Errors));

            var publishSuccess = await GenerateAndPublish(contract);
            if (!publishSuccess)
                throw new Exception(String.Join(' ', validationResult.Errors));

            _metadataRepository.UpdateFilesGenerated();
            await CleanFromQueueAndDeleteRequestFromS3(message, objectPath);

        }catch(Exception ex)
        {
            //logger.LogError(ex.Message);
            await SendToDeadletterAndCleanFromQueue(message);
        }
    }

    private async Task<SQSReceiveMessageResponse> TryReceiveMessageOrEmpty()
    {
        try
        {
            var receiveResult = await _sqsClient.ReceiveMessage();

            if (receiveResult == null || receiveResult.ReceivedMessages == 0)
                return null;

            return receiveResult;

        }catch(Exception ex)
        {
            return null;
        }


    }

    private async Task<(XlsFileInputDeliveryContentV1, string)> TryReceiveContractOrEmpty(SQSReceiveMessageResponse messageResponse)
    {
        //Shouold look something like this
        var response = S3EventNotification.ParseJson(messageResponse.MessageContent);
        string objectPath = response.Records.First().S3.Object.Key;

        var s3GetObjectResult = await _s3Client.GetAsync(objectPath);

        if (!s3GetObjectResult.IsSuccess)
            return (null, null);

        return (JsonSerializer
            .Deserialize<XlsFileInputDeliveryContentV1>
            (s3GetObjectResult.Content, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }), objectPath);
    }

    private async Task<bool> GenerateAndPublish(XlsFileInputDeliveryContentV1 contract)
    {
        var xlsFileMetadata = _mapper.Map(contract);

        var file = _fileGenerator.Process(xlsFileMetadata);

        string destinationPath = $"output/{Guid.NewGuid()}.xlsx";
        string contentType = "application/vnd.ms-excel";
        var putResult = await _s3Client.PutAsync(destinationPath, file, contentType);

        return putResult.IsSuccess;
    }

    private async Task CleanFromQueueAndDeleteRequestFromS3(SQSReceiveMessageResponse message, string objectPath)
    {
        await CleanFromQueue(message);

        await _s3Client.DeleteAsync(objectPath);
    }

    private async Task SendToDeadletterAndCleanFromQueue(SQSReceiveMessageResponse message)
    {
        await _sqsClient.SendToDeadletter(message.MessageContent, message.MessageId);
        await CleanFromQueue(message);
    }

    private async Task CleanFromQueue(SQSReceiveMessageResponse message)
    {
        await _sqsClient.DeleteMessage(message.ReceiptHandle);
    }
}
