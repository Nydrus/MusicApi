﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicApi.Data;
using MusicApi.Models;
using System.Collections.Generic;

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
        public IActionResult Get()
        {
            
             return Ok(_dbContext.Songs);
           
        }

        // GET api/<SongsController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            
           var song = _dbContext.Songs.Find(id);
            if (song == null)
            {
                return NotFound("No Record found against this Id");
            }
            else
            {
                return Ok(song);
            }
        }

        // POST api/<SongsController>
        [HttpPost]
        public IActionResult Post([FromBody] Song song)
        {
            _dbContext.Add(song);
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        // PUT api/<SongsController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Song songObj)
        {
           var song = _dbContext.Songs.Find(id);
            if (song == null)
            {
                return NotFound("No Record found against this Id");
            }
            else
            {
                song.Title = songObj.Title;
                song.Language = songObj.Language;
                _dbContext.SaveChanges();
                return Ok("Record updated sucessfully");
            }
        }

        // DELETE api/<SongsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var song = _dbContext.Songs.Find(id);
            if (song == null)
            {
                return NotFound("No Record found against this Id");
            }
            else
            {               
                _dbContext.Songs.Remove(song);
                _dbContext.SaveChanges();
                return Ok("Record Deleted");
            }
        }
    }
}
