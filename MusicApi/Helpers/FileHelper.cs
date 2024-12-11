using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MusicApi.Models;
using System.IO;
using System.Threading.Tasks;

namespace MusicApi.Helpers
{
    public static class FileHelper
    {
        public static async Task<string> UploadImage(IFormFile file)
        {
            string connectionString = @"DefaultEndpointsProtocol=https;AccountName=musicblobstorage;AccountKey=U/YdF93ZRP+sZITZmAdpLqAy1Ek1zYg8cfySWLE72cnXyrZsHk7P4Trq45n3JzMm6wp/GUdx23Vw+AStvPYMVA==;EndpointSuffix=core.windows.net";
            string containerName = "songscover";

            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(file.FileName);
            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            await blobClient.UploadAsync(memoryStream);                
            return blobClient.Uri.AbsoluteUri;
        }
    }
}
