using Amazon.SQS;
using Amazon.SQS.Model;
using DocumentService.Domain.Clients.SQS;


namespace DocumentService.BackgroundFileProcessing.Clients.SQS
{
    public class InputDeliveryQueue : ISqsClient
    {
        private IAmazonSQS _sqsClient;
        private string _queueUrl = "";
        private string _deadletterQueueUrl = "";
        public InputDeliveryQueue(IAmazonSQS sqsClient)
        {
            _sqsClient = sqsClient;
        }

        public async Task<SQSDeleteMessageResponse> DeleteMessage(string receiptHandle)
        {
            try
            {
                var deleteMessageRequest = new DeleteMessageRequest() { QueueUrl = _queueUrl, ReceiptHandle = receiptHandle };
                var deleteResponse = await _sqsClient.DeleteMessageAsync(deleteMessageRequest);

                bool isSuccess = (int)deleteResponse.HttpStatusCode < 300 && (int)deleteResponse.HttpStatusCode >= 200;
                return new SQSDeleteMessageResponse() { IsSuccess = isSuccess };
                
            }catch (Exception ex)
            {
                return new SQSDeleteMessageResponse() { IsSuccess = false, Error = ex.Message };

            }
        }

        public async Task<SQSReceiveMessageResponse> ReceiveMessage()
        {
            try
            {
                var receiveRequest = new ReceiveMessageRequest
                {
                    QueueUrl = _queueUrl,
                    MaxNumberOfMessages = 1,
                    WaitTimeSeconds = 1
                };

                var receiveResponse = await _sqsClient.ReceiveMessageAsync(receiveRequest);

                if (receiveResponse.Messages.Count > 0)
                    return new SQSReceiveMessageResponse() { 
                        ReceivedMessages = 1, 
                        MessageId = receiveResponse.Messages[0].MessageId, 
                        MessageContent = receiveResponse.Messages[0].Body,
                        ReceiptHandle = receiveResponse.Messages[0].ReceiptHandle
                    };

                return new SQSReceiveMessageResponse()
                {
                    ReceivedMessages = 0
                };
            }
            catch(Exception ex)
            {
                return new SQSReceiveMessageResponse()
                {
                    ReceivedMessages = 0,
                    Error = ex.Message
                };
            }
            
        }

        public async Task<SQSSendToDeadletterResponse> SendToDeadletter(string messageBody, string messageId = "")
        {
            try
            {
                var sendMessageRequest = new SendMessageRequest()
                {
                    MessageBody = messageBody,
                    QueueUrl = _deadletterQueueUrl,
                    MessageAttributes = messageId == "" ? null : new Dictionary<string, MessageAttributeValue>()
                {
                    {"CorrelationId", new MessageAttributeValue(){ StringValue = messageId } }
                }
                };

                var sendMessageResponse = await _sqsClient.SendMessageAsync(sendMessageRequest);

                return new SQSSendToDeadletterResponse() { IsSuccess = true };

            }
            catch (Exception ex)
            {
                return new SQSSendToDeadletterResponse() { IsSuccess = false, Message = ex.Message };


            }
        }
    }
}
