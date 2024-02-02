using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FilmRecensioner.Data;
using FilmRecensioner.Models;
using Microsoft.AspNetCore.Authorization;
using FilmRecensioner.DTOs;

namespace FilmRecensioner.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VideosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Videos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Video>>> GetVideos()
        {
            var videos = await _context.Videos.Include(r => r.Reviews).ToListAsync();

            if (!videos.Any())
            {
                return NotFound("No videos was found, try adding one.");
            }

            var videosWithReviews = videos.Select(
                v => new
                {
                    Id = v.Id,
                    Title = v.Title,
                    Date = v.Date,
                    Reviews = v.Reviews.Select(r => new
                    {
                        r.Id,
                        r.Rating,
                        r.ReviewText,
                        r.UserId
                    }
                    ).ToList()
                });
            return Ok(videosWithReviews);
        }

        // GET: api/Videos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Video>> GetVideo(int id)
        {

            if (id <= 0)
            {
                return BadRequest("Id must be higher than 0");
            }

            if (!VideoExists(id))
            {
                return NotFound("No video was found with that id");
            }

            var video = await _context.Videos.Include(v => v.Reviews).FirstAsync(v => v.Id == id);

            if (video == null)
            {
                return NotFound();
            }

            var videosWithReviews = new

            {
                video.Id,
                video.Title,
                video.Date,
                Reviews = video.Reviews.Select(r => new
                {
                    r.Id,
                    r.Rating,
                    r.ReviewText,
                    r.UserId
                }

                    ).ToList()
            }
                ;
            return Ok(videosWithReviews);
        }

        // PUT: api/Videos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> PutVideo(int id, [FromBody] VideoDTO videoUpdated)
        {
            if (id <= 0)
            {
                return BadRequest("Id must be higher than 0");
            }

            var videoToUpdate = await _context.Videos.FindAsync(id);
           
            if (videoToUpdate == null) 
            {
                return NotFound("No video was found with that id");
            }

            videoToUpdate.Title = videoUpdated.Title;
            videoToUpdate.Date = videoUpdated.Date;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VideoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Videos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize]
        public async Task<ActionResult<Video>> PostVideo(VideoDTO video)
        {

            if (video.Date == default(DateTime))
            {
                return BadRequest("Date is required and can't be the default value.");
            }

            var videoCreated = new Video
            {
                Title = video.Title,
                Date = video.Date
            };

            _context.Videos.Add(videoCreated);
            await _context.SaveChangesAsync();

            var response = new
            {
                videoCreated.Id,
                videoCreated.Title,
                videoCreated.Date
            };

            return CreatedAtAction("GetVideo", new { id = videoCreated.Id }, response);
        }

        // DELETE: api/Videos/5
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteVideo(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id must be higher than 0");
            }

            var video = await _context.Videos.FindAsync(id);
            
            if (video == null)
            {
                return NotFound();
            }

            _context.Videos.Remove(video);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VideoExists(int id)
        {
            return _context.Videos.Any(e => e.Id == id);
        }
    }
}
