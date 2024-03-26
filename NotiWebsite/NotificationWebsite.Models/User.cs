using System.ComponentModel.DataAnnotations;

namespace NotificationWebsite.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [MaxLength(length: 30, ErrorMessage = "Your username has more than 30 characters")]
        public required string Username { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [MaxLength(length: 80, ErrorMessage = "Your mail has more than 80 characters")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public required string Password { get; set; }
    }
}
