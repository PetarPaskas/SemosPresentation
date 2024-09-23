using DocumentService.BackgroundFileProcessing.Clients.S3;
using DocumentService.BackgroundFileProcessing.Clients.SQS;
using DocumentService.BackgroundFileProcessing.Domain;
using DocumentService.BackgroundFileProcessing.Helpers.XlsFileGeneration;
using DocumentService.BackgroundFileProcessing.Mappers;
using DocumentService.BackgroundFileProcessing.Processes;
using DocumentService.BackgroundFileProcessing.Validators;
using DocumentService.Domain.Clients.S3;
using DocumentService.Domain.Clients.SQS;
using DocumentService.Domain.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Amazon.S3;
using Amazon.SQS;
using Amazon;
using Amazon.Runtime;

namespace DocumentService.BackgroundFileProcessing;

public static class BackgroundFileProcessingServices
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, 
        BasicAWSCredentials credentials,
        RegionEndpoint region)
    {
        services.RegisterAwsServices(credentials, region);

        services.AddTransient<IXlsFileInputDeliveryContentV1Validator, XlsFileInputDeliveryContentV1Validator>();
        services.AddTransient<IMapper<XlsFileInputDeliveryContentV1, XlsFileMetadata>, XlsFileMetadataMapper>();
        services.AddTransient<IFileGenerator<XlsFileMetadata>, XlsFileGenerator>();

        services.AddScoped<IS3Client, DeliveriesS3Client>();
        services.AddScoped<ISqsClient,InputDeliveryQueue>();

        services.AddScoped<IFileGenerationProcess, FileGenerationProcess>();

        return services;
    }

    private static IServiceCollection RegisterAwsServices(this IServiceCollection services, 
        BasicAWSCredentials credentials,
        RegionEndpoint region)
    {
        services.AddScoped<IAmazonS3>(sp => new AmazonS3Client(credentials,region));
        services.AddScoped<IAmazonSQS>(sp =>new AmazonSQSClient(credentials, region));
        
        return services;
    }
}
