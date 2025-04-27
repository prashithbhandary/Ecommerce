using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using clothing_store.application.interfaces;
using clothing_store.domain.models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clothing_store.application.services
{
    public class BlobService : IBlobService
    {
        private readonly AzureBlobSettings _settings;

        public BlobService(IOptions<AzureBlobSettings> options)
        {
            _settings = options.Value;
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var blobServiceClient = new BlobServiceClient(_settings.ConnectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(_settings.ContainerName);

            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blobClient = containerClient.GetBlobClient(Guid.NewGuid() + Path.GetExtension(file.FileName));

            await using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);

            return blobClient.Uri.ToString(); // Return the public URL
        }

        public async Task<bool> DeleteFileAsync(string fileUrl)
        {
            try
            {
                var blobServiceClient = new BlobServiceClient(_settings.ConnectionString);
                var containerClient = blobServiceClient.GetBlobContainerClient(_settings.ContainerName);

                // Extract blob name from URL
                var blobName = Path.GetFileName(new Uri(fileUrl).LocalPath);
                var blobClient = containerClient.GetBlobClient(blobName);

                return await blobClient.DeleteIfExistsAsync();
            }
            catch (Exception ex)
            {
                // Optionally log the exception
                return false;
            }
        }
    }
}
