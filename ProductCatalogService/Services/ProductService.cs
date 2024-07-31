using ProductCatalogService.Data.Repository.Contracts;
using ProductCatalogService.Models;
using ProductCatalogService.Services.Contracts;

namespace ProductCatalogService.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddProduct(Product product)
        {
            _unitOfWork.Product.Add(product);
            SaveChanges();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _unitOfWork.Product.GetAll();
        }

        public Product GetProductById(int id)
        {
            return _unitOfWork.Product.Get(u => u.Id == id);
        }

        public void RemoveProduct(int id)
        {
            _unitOfWork.Product.Remove(id);
            SaveChanges();
        }

        public void SaveChanges()
        {
            _unitOfWork.Save();
        }

        public void UpdateProduct(Product product)
        {
            _unitOfWork.Product.Update(product);
            SaveChanges();
        }
    }
}
