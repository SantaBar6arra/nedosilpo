using NedoSilpo.Query.Domain.Entities;

namespace NedoSilpo.Query.Domain.Repositories;

public interface IProductRepository
{
    Task CreateAsync(Product product);
    Task UpdateAsync(Product product);
    Task RemoveAsync(Guid id);

    Task<Product> GetByIdAsync(Guid id);
    Task<IEnumerable<Product>> GetAllAsync(string name, string description, decimal priceMin, decimal priceMax);
    Task<IEnumerable<Product>> GetDeletedAsync();
}
