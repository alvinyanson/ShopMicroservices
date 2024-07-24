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
    public class CategoriesController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoriesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        // GET: api/<CategoriesController>
        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetCategories()
        {
            var results = _unitOfWork.Category.GetAll();
            return Ok(new { success = true, message = "Categories retrieved!", result = results });
        }

        // GET api/<CategoriesController>/5
        [HttpGet("{id}")]
        public ActionResult<IEnumerable<Product>> GetCategory(int id)
        {
            var result = _unitOfWork.Category.Get(u => u.Id == id);
            return Ok(new { success = true, message = "Category retrieved!", result = result });
        }

        // POST api/<CategoriesController>
        [HttpPost]
        public ActionResult<Category> AddCategory(CategoryCreateDto categoryCreateDto)
        {
            var category = _mapper.Map<Category>(categoryCreateDto);

            _unitOfWork.Category.Add(category);
            _unitOfWork.Save();

            return Ok(new { success = true, message = "Category created!" });
        }

        // PUT api/<CategoriesController>/5
        [HttpPut]
        public ActionResult<Category> UpdateCategory(CategoryUpdateDto categoryUpdateDto)
        {
            var category = _mapper.Map<Category>(categoryUpdateDto);

            _unitOfWork.Category.Update(category);
            _unitOfWork.Save();

            return Ok(new { success = true, message = "Category updated!" });
        }

        // DELETE api/<CategoriesController>/5
        [HttpDelete("{id}")]
        public ActionResult<Category> Delete(int id)
        {
            _unitOfWork.Category.Remove(id);
            _unitOfWork.Save();

            return Ok(new { success = true, message = "Category deleted!" });
        }
    }
}
