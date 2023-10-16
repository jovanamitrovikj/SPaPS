using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPaPS.Models.AccountModels
{
    public class ResetPasswordModel
    {
        public string Email { get; set; } =  string.Empty;
        public string Token { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter confirm password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Confirm password doesn't match, try again !")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
