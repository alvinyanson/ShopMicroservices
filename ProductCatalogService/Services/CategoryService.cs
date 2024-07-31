using ProductCatalogService.Data.Repository.Contracts;
using ProductCatalogService.Models;
using ProductCatalogService.Services.Contracts;

namespace ProductCatalogService.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddCategory(Category category)
        {
            _unitOfWork.Category.Add(category);
            SaveChanges();
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _unitOfWork.Category.GetAll();
        }

        public Category GetCategoryById(int id)
        {
            return _unitOfWork.Category.Get(u => u.Id == id);
        }

        public void RemoveCategory(int id)
        {
            _unitOfWork.Category.Remove(id);
            SaveChanges();
        }

        public void SaveChanges()
        {
            _unitOfWork.Save();
        }

        public void UpdateCategory(Category category)
        {
            _unitOfWork.Category.Update(category);
            SaveChanges();
        }
    }
}
