using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentService.Domain.Clients.SQS
{
    public interface ISqsClient
    {
        Task<SQSDeleteMessageResponse> DeleteMessage(string receiptHandle);
        Task<SQSReceiveMessageResponse> ReceiveMessage();
        Task<SQSSendToDeadletterResponse> SendToDeadletter(string messageBody, string messageId = "");
    }
}
