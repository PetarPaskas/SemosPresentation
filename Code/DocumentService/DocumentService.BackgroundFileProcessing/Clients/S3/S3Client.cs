using Amazon.S3;
using Amazon.S3.Model;
using DocumentService.Domain.Clients.S3;

namespace DocumentService.BackgroundFileProcessing.Clients.S3
{
    public class S3Client : IS3Client
    {
        private IAmazonS3 _s3Client;
        public S3Client(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }
        public async Task<S3DeleteObjectDto> DeleteAsync(string objectPath)
        {
            try
            {
                DeleteObjectRequest request = new() { Key = objectPath };

                var response = await _s3Client.DeleteObjectAsync(request);

                return new S3DeleteObjectDto() { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new S3DeleteObjectDto() { IsSuccess = false };
            }


        }

        public async Task<S3GetObjectDto> GetAsync(string objectPath)
        {
            try
            {
                GetObjectRequest request = new() { Key = objectPath };

                var response = await _s3Client.GetObjectAsync(request);

                return new S3GetObjectDto() { Content = null, IsSuccess = true };

            }
            catch (Exception ex)
            {
                return new S3GetObjectDto() { Content = null, IsSuccess = false };
            }


        }

        public async Task<S3PutObjectDto> PutAsync(string objectPath, byte[] objectData, string contentType)
        {
            try
            {
                using Stream objectStream = new MemoryStream(objectData);
                PutObjectRequest request = new() { Key = objectPath, InputStream = objectStream, ContentType = contentType };

                var response = await _s3Client.PutObjectAsync(request);

                return new S3PutObjectDto { IsSuccess = true };

            }
            catch (Exception ex)
            {
                return new S3PutObjectDto { IsSuccess = false };
            }



        }
    }
}
