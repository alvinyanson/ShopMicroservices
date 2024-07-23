using System.ComponentModel.DataAnnotations;

namespace AuthService.Models
{
    public class ChangePasswordAccount
    {
        [Required]
        public string Password { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; }
    }
}
