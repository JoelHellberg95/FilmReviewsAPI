using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FilmRecensioner.Models
{
    public class Video
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime Date { get; set; }

        [ValidateNever]
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
