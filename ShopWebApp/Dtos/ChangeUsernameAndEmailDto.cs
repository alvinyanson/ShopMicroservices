using System.ComponentModel.DataAnnotations;

namespace ShopWebApp.Dtos
{
    public class ChangeUsernameAndEmailDto
    {
        [Required]
        public string OldEmail { get; set; }

        [Required]
        public string NewEmail { get; set; }
    }
}
