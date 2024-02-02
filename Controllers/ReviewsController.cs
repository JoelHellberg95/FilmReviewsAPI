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
using Microsoft.AspNetCore.Identity;
using FilmRecensioner.DTOs;
using System.Security.Claims;

namespace FilmRecensioner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<CustomUser> _userManager;

        public ReviewsController(ApplicationDbContext context, UserManager<CustomUser>userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: api/Reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            var reviews = await _context.Reviews.ToListAsync();
            if (!reviews.Any())
            {
                return NotFound("No reviews was found, try adding one.");
            }
            var videos = await _context.Videos.ToListAsync();
            var reviewsWithVideos = reviews.Select(r =>
                               {
                                var video = videos.FirstOrDefault(v => v.Id == r.VideoId);
                                   return new
                                   {
                                    r.Id,
                                    r.Rating,
                                    r.ReviewText,
                                    VideoTitle = video?.Title,
                                    r.VideoId,
                                    r.UserId
                                    
                                };
                               });

            return Ok(reviewsWithVideos);

        }

        // GET: api/Reviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id must be higher than 0");
            }

            var review = await _context.Reviews.FirstOrDefaultAsync(r=>r.Id==id);

            if (review == null)
            {
                return NotFound("There is no reviews with this ID");
            }

            
            var Video = await _context.Videos.FindAsync(review.VideoId);
            return Ok(new
            {
                review.Id,
                review.Rating,
                review.ReviewText,
                Videoname = Video?.Title,
                review.VideoId,
                review.UserId
            });
        }
       
        // PUT: api/Reviews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}") , Authorize] 
        public async Task<IActionResult> PutReview(int id,[FromBody] ReviewDTO review)
        {
            if (id <= 0)
            {
                return BadRequest("Id must be higher than 0");
            }
            var reviewToUpdate = await _context.Reviews.FindAsync(id);

            var userId = User.FindFirstValue (ClaimTypes.NameIdentifier);

            if (reviewToUpdate == null)
            {
                return NotFound("No review was found with that id");
            }
            
            if (reviewToUpdate.UserId != userId)
            {
                return Unauthorized("You are not allowed to change another users review"); 
            }

            var video = await _context.Videos.FindAsync(review.VideoId);
            if (video == null)
            {
                return NotFound("No video was found with that id");
            }

            reviewToUpdate.Rating = review.Rating;
            reviewToUpdate.ReviewText = review.ReviewText;
            reviewToUpdate.VideoId = review.VideoId;
            reviewToUpdate.UserId = userId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
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

        // POST: api/Reviews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize]
        public async Task<ActionResult<Review>> PostReview(ReviewDTO review)
        {
            var video = await _context.Videos.FindAsync(review.VideoId);
            
            if (review.VideoId <= 0)
            {
                return BadRequest("VideoId must be higher than 0");
            }
            
            if (video == null)
            {
                return NotFound("No video was found with that id");
            }

            var reviewToAdd = new Review
            {
                Rating = review.Rating,
                ReviewText = review.ReviewText,
                VideoId = review.VideoId,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            _context.Reviews.Add(reviewToAdd);

            await _context.SaveChangesAsync();
            var response = new
            {
                reviewToAdd.Id,
                reviewToAdd.Rating,
                reviewToAdd.ReviewText,
                Videoname = video.Title,
                reviewToAdd.VideoId,
                reviewToAdd.UserId
            };

            return CreatedAtAction("GetReview", new { id = reviewToAdd.Id }, response);
        }

        // DELETE: api/Reviews/5
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteReview(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id must be higher than 0");
            }
            var ReviewToDelete = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound("Review not found. Try searching for another one.");
            }
            if (review.UserId != ReviewToDelete)
            {
                return Unauthorized("You are not allowed to delete another users review");
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("GetReviewsForVideo/{videoId}")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsForVideo(int videoId)
        {
            if (videoId <= 0)
            {
                return BadRequest("Id must be higher than 0");
            }
            var reviews = await _context.Reviews.Where(r => r.VideoId == videoId).ToListAsync();
            if (!reviews.Any())
            {
                return NotFound("No reviews was found for that video");
            }
            var videos = await _context.Videos.FindAsync(videoId);
            if (videos == null)
            {
                return NotFound("No video was found with that id");
            }
                        
            var reviewsWithVideos = reviews.Select(r => new
            {
             r.Id,
             r.Rating,
             r.ReviewText,
             Videoname = videos.Title,
             r.VideoId,
             r.UserId
            });

            return Ok(reviewsWithVideos);
        }

        [HttpGet("GetReviewsForUser"),Authorize]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsForUser()
        {
            var userId = _userManager.GetUserId(User);
            var reviews = await _context.Reviews.Where(r => r.UserId == userId).ToListAsync();
            
            if (!reviews.Any())
            {
                return NotFound("No reviews was found for that user");
            }

            var videos = await _context.Videos.ToListAsync();
            var reviewsWithVideos = reviews.Select(r =>
            {
                var video = videos.FirstOrDefault(v => v.Id == r.VideoId);
                return new
                {
                    r.Id,
                    r.Rating,
                    r.ReviewText,
                    Videoname = video?.Title,
                    r.VideoId,
                    r.UserId
                };
            }).ToList();

            return Ok(reviewsWithVideos);
        }




        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}
