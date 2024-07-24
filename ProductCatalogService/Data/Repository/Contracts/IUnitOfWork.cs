namespace ProductCatalogService.Data.Repository.Contracts
{
    public interface IUnitOfWork
    {
        IProductRepository Product { get; }

        ICategoryRepository Category { get; }

        ICartRepository Cart { get; }

        void Save();
    }
}
