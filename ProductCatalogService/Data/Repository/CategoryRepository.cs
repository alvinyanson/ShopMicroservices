using ProductCatalogService.Data.Repository.Contracts;
using ProductCatalogService.Models;

namespace ProductCatalogService.Data.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private AppDbContext _db;

        public CategoryRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
