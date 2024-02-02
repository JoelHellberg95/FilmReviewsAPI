using Microsoft.AspNetCore.Identity;

namespace FilmRecensioner.Models
{
    public class CustomUser : IdentityUser
    {
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
