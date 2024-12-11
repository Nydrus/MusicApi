using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApi.Data;
using MusicApi.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        // GET: api/<SongsController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            
             return Ok(await _dbContext.Songs.ToListAsync());
           
        }

        // GET api/<SongsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            
           var song = await _dbContext.Songs.FindAsync(id);
            if (song == null)
            {
                return NotFound("No Record found against this Id");
            }
            else
            {
                return Ok(song);
            }
        }

        //// POST api/<SongsController>
        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] Song song)
        //{
        //    await _dbContext.AddAsync(song);
        //   await _dbContext.SaveChangesAsync();
        //    return StatusCode(StatusCodes.Status201Created);
        //}
        // POST api/<SongsController>
        [HttpPost]
        public async Task Post([FromForm] Song song)
        {
            string connectionString = @"DefaultEndpointsProtocol=https;AccountName=musicblobstorage;AccountKey=U/YdF93ZRP+sZITZmAdpLqAy1Ek1zYg8cfySWLE72cnXyrZsHk7P4Trq45n3JzMm6wp/GUdx23Vw+AStvPYMVA==;EndpointSuffix=core.windows.net";
            string containerName = "songscover";
            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(song.Image.FileName);
            var memoryStream = new MemoryStream();
           await song.Image.CopyToAsync(memoryStream);
            memoryStream.Position=0;
            await blobClient.UploadAsync(memoryStream);
        }

        // PUT api/<SongsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Song songObj)
        {
           var song = await _dbContext.Songs.FindAsync(id);
            if (song == null)
            {
                return NotFound("No Record found against this Id");
            }
            else
            {
                song.Title = songObj.Title;
                song.Language = songObj.Language;
                song.Duration= songObj.Duration;
                await _dbContext.SaveChangesAsync();
                return Ok("Record updated sucessfully");
            }
        }

        // DELETE api/<SongsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var song = await _dbContext.Songs.FindAsync(id);
            if (song == null)
            {
                return NotFound("No Record found against this Id");
            }
            else
            {               
                _dbContext.Songs.Remove(song);
                await _dbContext.SaveChangesAsync();
                return Ok("Record Deleted");
            }
        }
    }
}
