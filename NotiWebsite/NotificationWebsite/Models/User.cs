
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json.Serialization;

namespace NotificationWebsite.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [MaxLength(length: 30, ErrorMessage = "Your username has more than 30 characters")]
        public string Username { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [MaxLength(length: 80, ErrorMessage = "Your mail has more than 80 characters")]
        public string Mail { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string Password { get; set; }
    }
}
