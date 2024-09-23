using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentService.Domain.Clients.S3;

public interface IS3Client
{
    Task<S3DeleteObjectDto> DeleteAsync(string objectPath);
    Task<S3GetObjectDto> GetAsync(string objectPath);
    Task<S3PutObjectDto> PutAsync(string objectPath, byte[] objectData, string contentType);
}
