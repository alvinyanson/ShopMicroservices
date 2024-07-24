using ProductCatalogService.Data.Repository.Contracts;
using ProductCatalogService.Models;

namespace ProductCatalogService.Data.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private AppDbContext _db;

        public ProductRepository(AppDbContext db) : base (db)
        {
            _db = db;
        }

    }
}
