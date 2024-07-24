using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProductCatalogService.Dtos
{
    public class ProductUpdateDto : ProductCreateDto
    {
        public int Id { get; set; }
    }
}
