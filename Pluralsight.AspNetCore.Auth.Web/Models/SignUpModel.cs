using System.ComponentModel.DataAnnotations;

namespace Pluralsight.AspNetCore.Auth.Web.Models
{
    public class SignUpModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Password Must be same")]
        public string ConfirmPassword { get; set; }

        public string returnUrl { get; set; }

    }
}
