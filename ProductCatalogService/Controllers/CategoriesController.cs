using AutoMapper;
using CatalogService.Services;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogService.Dtos;
using ProductCatalogService.Models;
using ProductCatalogService.Services.Contracts;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductCatalogService.Controllers
{
    [AuthHeaderFilter]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }


        [HttpGet]
        public ActionResult<string> GetCategories()
        {
            var result = _categoryService.GetAllCategories();

            return Ok(new { success = true, message = "Categories retrieved!", result });
        }

        [HttpGet("{id}")]
        public ActionResult<string> GetCategory(int id)
        {
            var result = _categoryService.GetCategoryById(id);

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
                _categoryService.AddCategory(category);
            }
            else
            {
                _categoryService.UpdateCategory(category);
            }

            return Ok(new { success = true, message = "Category saved!" });
        }

        [HttpDelete("{id}")]
        public ActionResult<string> Delete(int id)
        {

            var result = _categoryService.GetCategoryById(id);

            if (result == null)
            {
                return NotFound(new { success = false, message = $"Category with id {id} does not exist!" });
            }

            _categoryService.RemoveCategory(id);

            return Ok(new { success = true, message = "Category deleted!" });
        }
    }
}
