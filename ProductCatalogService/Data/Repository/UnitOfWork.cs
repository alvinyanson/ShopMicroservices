using ProductCatalogService.Data.Repository.Contracts;

namespace ProductCatalogService.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _db;

        public IProductRepository Product { get; private set; }

        public ICategoryRepository Category { get; private set; }

        public ICartRepository Cart { get; private set; }

        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            Product = new ProductRepository(_db);
            Category = new CategoryRepository(_db);
            Cart = new CartRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
