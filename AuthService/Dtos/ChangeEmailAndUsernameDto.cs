using System.ComponentModel.DataAnnotations;

namespace AuthService.Dtos
{
    public class ChangeEmailAndUsernameDto
    {
        [Required]
        public string OldEmail { get; set; }

        [Required]
        public string NewEmail { get; set; }
    }
}
