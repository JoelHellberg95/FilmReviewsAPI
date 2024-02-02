using System.ComponentModel.DataAnnotations;

namespace FilmRecensioner.DTOs
{
    public class ReviewDTO
    {
        [Range(1, 5, ErrorMessage = "Rating must be between 1-5")]
        public int Rating { get; set; }

        [Required]
        public string ReviewText { get; set; }

        [Required]
        public int VideoId { get; set; }
    }
}
