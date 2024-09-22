using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentService.Domain.Clients.SQS
{
    public class SQSReceiveMessageResponse
    {
        public int ReceivedMessages { get; set; }
        public string MessageContent { get; set; }
        public string MessageId { get; set; }
        public string Error { get; set; }
        public string ReceiptHandle { get; set; }
    }
}
