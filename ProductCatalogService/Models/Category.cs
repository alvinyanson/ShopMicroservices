using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProductCatalogService.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Category Name")]
        public string Name { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}
