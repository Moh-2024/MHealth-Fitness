using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MHealth_FitnessAPI
{
    public class BlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName = "images"; // Container name in Azure

        public BlobService(IConfiguration configuration)
        {
            string? connectionString = configuration["AzureBlobStorage:ConnectionString"];
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "Azure Blob Storage connection string is not configured.");
            }
            _blobServiceClient = new BlobServiceClient(connectionString);
        }
        public string GetBlobSasUrl(string fileName)
        {
            string storageAccountName = "mhealthfitness";
            string containerName = "images";
            string sasToken = "sp=racwd&st=2025-03-14T01:38:15Z&se=2028-03-14T09:38:15Z&spr=https&sv=2022-11-02&sr=c&sig=jsgIcvvCO1NDOnwNm0k6GwceytT%2F3yltkgB3SNxlTcI%3D";

            return $"https://{storageAccountName}.blob.core.windows.net/{containerName}/{fileName}?{sasToken}";
        }


        public async Task<string> UploadImageAsync(Stream imageStream, string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(imageStream, true);

            return blobClient.Uri.ToString(); // Return the image URL
        }
    }
}
