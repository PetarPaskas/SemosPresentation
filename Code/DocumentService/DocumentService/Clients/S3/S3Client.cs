using DocumentService.Domain.Clients.S3;

namespace DocumentService.Clients.S3
{
    public class S3Client : IS3Client
    {
        public Task<S3DeleteObjectDto> DeleteAsync(string objectPath)
        {
            throw new NotImplementedException();
        }

        public Task<S3GetObjectDto> GetAsync(string objectPath)
        {
            throw new NotImplementedException();
        }

        public Task<S3PutObjectDto> PutAsync(string objectPath, byte[] objectDat
            .
            throw new NotImplementedException();
        }
    }
}
