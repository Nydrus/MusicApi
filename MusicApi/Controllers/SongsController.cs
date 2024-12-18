using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicApi.Data;
using MusicApi.Helpers;
using MusicApi.Models;
using System.IO;
using System.Threading.Tasks;

namespace MusicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private ApiDbContext _dbContext;
        public SongsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Song song)
        {
            var imageUrl = await FileHelper.UploadImage(song.Image);
            song.ImageUrl = imageUrl;
            await _dbContext.Songs.AddAsync(song);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }
        public static async Task<string> UploadImage(IFormFile file)
        {
            string connectionString = @"DefaultEndpointsProtocol=https;AccountName=musicblobstorage;AccountKey=U/YdF93ZRP+sZITZmAdpLqAy1Ek1zYg8cfySWLE72cnXyrZsHk7P4Trq45n3JzMm6wp/GUdx23Vw+AStvPYMVA==;EndpointSuffix=core.windows.net";
            string containerName = "audiofiles";

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
