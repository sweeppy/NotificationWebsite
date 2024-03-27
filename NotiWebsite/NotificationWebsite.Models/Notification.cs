using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotificationWebsite.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "This field is required"),
         MaxLength(80, ErrorMessage = "Your message should not exceed 80 characters")]
        public string? Message { get; set; }
        [MaxLength(30)]
        public string? SocialNetwork { get; set; }
        [MaxLength(30)]
        public string? Status { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }

    }
}