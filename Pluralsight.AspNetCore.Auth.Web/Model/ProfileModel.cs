using System.ComponentModel.DataAnnotations;

namespace Pluralsight.AspNetCore.Auth.Web.Model
{
    public class ProfileModel
    {
        [Required(ErrorMessage = "Display name is required")]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string Email { get; set; }
    }
}
