using System.Linq.Expressions;

namespace ProductCatalogService.Data.Repository.Contracts
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(string? includedProperties = null);

        T Get(Expression<Func<T, bool>> filter, string? includedProperties = null);

        void Add(T entity);

        void Update(T entity);

        void Remove(T entity);

        void Remove(int id);

        void RemoveRange(IEnumerable<T> entities);
    }
}
