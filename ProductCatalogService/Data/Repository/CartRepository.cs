using ProductCatalogService.Data.Repository.Contracts;
using ProductCatalogService.Models;

namespace ProductCatalogService.Data.Repository
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private AppDbContext _db;

        public CartRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
