using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogService.Data.Repository.Contracts;
using ProductCatalogService.Dtos;
using ProductCatalogService.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductCatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<string> GetProducts()
        {
            var result = _unitOfWork.Product.GetAll();

            return Ok(new { success = true, message = "Products retrieved!", result });
        }

        [HttpGet("{id}")]
        public ActionResult<string> GetProduct(int id)
        {
            var result = _unitOfWork.Product.Get(u => u.Id == id);

            return Ok(new { success = true, message = "Product retrieved!", result });
        }

        [HttpPost]
        public ActionResult<string> UpdateProduct(ProductUpdateDto productUpdateDto)
        {
            var product = _mapper.Map<Product>(productUpdateDto);

            if(product.Id == 0)
            {
                _unitOfWork.Product.Add(product);
            }
            else
            {
                _unitOfWork.Product.Update(product);
            }

            _unitOfWork.Save();

            return Ok(new { success = true, message = "Product saved!" });
        }

        [HttpDelete("{id}")]
        public ActionResult<string> Delete(int id)
        {
            _unitOfWork.Product.Remove(id);

            _unitOfWork.Save();

            return Ok(new { success = true, message = "Product deleted!" });
        }
    }
}
