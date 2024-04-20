
using System.ComponentModel.DataAnnotations;
using NotificationWebsite.Models;

namespace NotificationWebsite.Utility.Helpers.Validation
{
    public class CheckValidation : ILoginValidation
    {
        public bool IsLoginValid(User user)
        {
            var emailContext = new ValidationContext(user) { MemberName = "Email" };
            var emailResults = new List<ValidationResult>();
            bool isValidEmail = Validator.TryValidateProperty(user.Email, emailContext, emailResults);

            var passwordContext = new ValidationContext(user) { MemberName = "Password" };
            var passwordResults = new List<ValidationResult>();
            bool isValidPassword = Validator.TryValidateProperty(user.Password, passwordContext, passwordResults);

            if (!isValidEmail || !isValidPassword) return false;
            return true;
        }
    }
}