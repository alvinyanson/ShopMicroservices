using ProductCatalogService.Models;

namespace ProductCatalogService.Services.Contracts
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAllCategories();
        Category GetCategoryById(int id);
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void RemoveCategory(int id);
        void SaveChanges();
    }
}
