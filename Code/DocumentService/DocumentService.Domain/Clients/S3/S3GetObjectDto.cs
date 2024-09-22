using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentService.Domain.Clients.S3
{
    public class S3GetObjectDto
    {
        public bool IsSuccess { get; set; }
        public byte[]? Content { get; set; }
    }
}
