using ProductCatalogService.Models;

namespace ProductCatalogService.Services.Contracts
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();
        Product GetProductById(int id);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void RemoveProduct(int id);
        void SaveChanges();
    }
}
