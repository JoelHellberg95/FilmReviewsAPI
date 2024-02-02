using System.ComponentModel.DataAnnotations;

namespace FilmRecensioner.DTOs
{
    public class VideoDTO
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
