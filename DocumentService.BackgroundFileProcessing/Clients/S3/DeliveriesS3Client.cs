﻿using Amazon.S3;
using Amazon.S3.Model;
using DocumentService.Domain.Clients.S3;

namespace DocumentService.BackgroundFileProcessing.Clients.S3
{
    public class DeliveriesS3Client : IS3Client
    {
        private IAmazonS3 _s3Client;
        private string _bucketName = "x1234-deliveries";
        public DeliveriesS3Client(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }
        public async Task<S3DeleteObjectDto> DeleteAsync(string objectPath)
        {
            try
            {
                DeleteObjectRequest request = new() { Key = objectPath, BucketName = _bucketName };

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
                GetObjectRequest request = new() { 
                    BucketName = _bucketName,
                    Key = objectPath 
                };

                var response = await _s3Client.GetObjectAsync(request);

                byte[] responseBytes = ReadResponse(response);

                return new S3GetObjectDto() { Content = responseBytes, IsSuccess = true };

            }
            catch (Exception ex)
            {
                return new S3GetObjectDto() { Content = null, IsSuccess = false };
            }


        }

        private byte[] ReadResponse(GetObjectResponse response)
        {
            using var memoryStream = new MemoryStream();
            response.ResponseStream.CopyTo(memoryStream);

            return memoryStream.ToArray();
        }

        public async Task<S3PutObjectDto> PutAsync(string objectPath, byte[] objectData, string contentType)
        {
            try
            {
                using Stream objectStream = new MemoryStream(objectData);
                PutObjectRequest request = new() { 
                    Key = objectPath, 
                    BucketName = _bucketName,
                    InputStream = objectStream, 
                    ContentType = contentType };

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
