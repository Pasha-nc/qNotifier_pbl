using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace qNotifier.ViewModels
{
    public class CreateUserViewModel
    {
        [Required]
        [Display(Name = "Email:")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Field {0} should have at least {2} and less then {1} symbols.", MinimumLength = 5)]
        [Display(Name = "Password:")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password:")]
        public string PasswordConfirm { get; set; }
    }
}
