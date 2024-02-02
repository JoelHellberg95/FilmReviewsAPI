using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmRecensioner.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public string ReviewText { get; set; }
        [Range(1, 5, ErrorMessage ="Rating needs to be between 1-5")]
        public int Rating { get; set; }
       
        
        [ForeignKey("VideoId")]
        public int VideoId { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }
    }
}
