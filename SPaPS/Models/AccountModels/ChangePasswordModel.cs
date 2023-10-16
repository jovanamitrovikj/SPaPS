using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPaPS.Models.AccountModels
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Please enter old password")]
        public string OldPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter new password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        
        [Required(ErrorMessage = "Please enter confirm password")]
        [DataType(DataType.Password)]  
        [Compare("NewPassword", ErrorMessage = "Confirm password doesn't match, try again !")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
