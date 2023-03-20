using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace qNotifier.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "Email:")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Field {0} should have at least {2} and less then {1} symbols.", MinimumLength = 5)]
        [Display(Name = "New password:")]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password:")]
        public string PasswordConfirm { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Field {0} should have at least {2} and less then {1} symbols.", MinimumLength = 5)]
        [Display(Name = "Old password:")]
        public string OldPassword { get; set; }
    }
}
