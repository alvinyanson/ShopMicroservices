using CatalogService.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogService.Dtos;
using ProductCatalogService.Models;
using ProductCatalogService.Services.Contracts;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductCatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public ActionResult<string> GetProducts()
        {
            var result = _productService.GetAllProducts();

            return Ok(new { success = true, message = "Products retrieved!", result });
        }

        [HttpGet("{id}")]
        public ActionResult<string> GetProduct(int id)
        {
            var result = _productService.GetProductById(id);

            if (result == null)
            {
                return NotFound(new { success = false, message = $"Product with id {id} does not exist!" });
            }

            return Ok(new { success = true, message = "Product retrieved!", result });
        }

        [AuthHeaderFilter]
        [HttpPost]
        public ActionResult<string> CreateOrUpdateProduct(UpdateProductDto request)
        {
            var product = request.Adapt<Product>();

            if (product.Id == 0)
            {
                _productService.AddProduct(product);
            }
            else
            {
                _productService.UpdateProduct(product);
            }

            return Ok(new { success = true, message = "Product saved!" });
        }

        [AuthHeaderFilter]
        [HttpDelete("{id}")]
        public ActionResult<string> Delete(int id)
        {

            var result = _productService.GetProductById(id);

            if (result == null)
            {
                return NotFound(new { success = false, message = $"Product with id {id} does not exist!" });
            }

            _productService.RemoveProduct(id);

            return Ok(new { success = true, message = "Product deleted!" });
        }
    }
}
