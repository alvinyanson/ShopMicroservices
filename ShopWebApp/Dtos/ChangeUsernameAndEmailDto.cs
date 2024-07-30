using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShopWebApp.Dtos
{
    public class ChangeUsernameAndEmailDto
    {
        [Required]
        [DisplayName(displayName: "Old Email")]
        public string OldEmail { get; set; } = string.Empty;

        [Required]
        [DisplayName(displayName: "New Email")]
        public string NewEmail { get; set; } = string.Empty;
    }
}
