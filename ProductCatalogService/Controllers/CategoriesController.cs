using AutoMapper;
using CatalogService.Services;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogService.Data.Repository.Contracts;
using ProductCatalogService.Dtos;
using ProductCatalogService.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductCatalogService.Controllers
{
    [AuthHeaderFilter]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoriesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        [HttpGet]
        public ActionResult<string> GetCategories()
        {
            var result = _unitOfWork.Category.GetAll();

            return Ok(new { success = true, message = "Categories retrieved!", result });
        }

        [HttpGet("{id}")]
        public ActionResult<string> GetCategory(int id)
        {
            var result = _unitOfWork.Category.Get(u => u.Id == id);

            if (result == null)
            {
                return NotFound(new { success = false, message = $"Category with id {id} does not exist!" });
            }

            return Ok(new { success = true, message = "Category retrieved!", result });
        }

        [HttpPost]
        public ActionResult<string> CreateOrUpdateCategory(CreateCategoryDto request)
        {
            var category = _mapper.Map<Category>(request);

            if (category.Id == 0)
            {
                _unitOfWork.Category.Add(category);
            }
            else
            {
                _unitOfWork.Category.Update(category);
            }

            _unitOfWork.Save();

            return Ok(new { success = true, message = "Category saved!" });
        }

        [HttpDelete("{id}")]
        public ActionResult<string> Delete(int id)
        {

            var result = _unitOfWork.Category.Get(u => u.Id == id);

            if (result == null)
            {
                return NotFound(new { success = false, message = $"Category with id {id} does not exist!" });
            }

            _unitOfWork.Category.Remove(id);

            _unitOfWork.Save();

            return Ok(new { success = true, message = "Category deleted!" });
        }
    }
}
