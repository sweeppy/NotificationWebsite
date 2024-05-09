using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NotificationWebsite.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [MaxLength(length: 30, ErrorMessage = "Your username has more than 30 characters")]
        public string Username { get; set; } = string.Empty;
        [Required(ErrorMessage = "This field is required")]
        [MaxLength(length: 80, ErrorMessage = "Your mail has more than 80 characters")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "This field is required")]
        [JsonIgnore]//!to stop return password in JSON responses
        [RegularExpression(@"^(?=.*\p{Lu})(?=.*\p{Nd}).{6,}$", ErrorMessage = "The password must contain capital letters and numbers")]
        public string Password { get; set; } = string.Empty;

        public List<Notification>? Notifications { get; set; }

    }
}
