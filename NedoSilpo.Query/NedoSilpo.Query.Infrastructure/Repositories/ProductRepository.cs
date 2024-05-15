using Microsoft.EntityFrameworkCore;
using NedoSilpo.Query.Domain.Entities;
using NedoSilpo.Query.Domain.Repositories;
using NedoSilpo.Query.Infrastructure.Context;
// ReSharper disable once ConvertToPrimaryConstructor

namespace NedoSilpo.Query.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly DataContext _dataContext;

    public ProductRepository(DataContext dataContext) => _dataContext = dataContext;

    public async Task CreateAsync(Product product)
    {
        _dataContext.Products.Add(product);
        await _dataContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _dataContext.Products.Update(product);
        await _dataContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(Guid id)
    {
        var product = await _dataContext.Products.FindAsync(id);
        if (product is null)
            return;

        _dataContext.Remove(product);
        await _dataContext.SaveChangesAsync();
    }

    public async Task<Product> GetByIdAsync(Guid id)
    {
        return await _dataContext.Products.FindAsync(id) ?? throw new Exception("entity not found");
    }

    public async Task<IEnumerable<Product>> GetAllAsync(string name, string description, decimal priceMin, decimal priceMax)
    {
        return await _dataContext.Products
            .AsNoTracking()
            .Where(product => product.Name.Contains(name)
                              && product.Description.Contains(description)
                              && product.Price >= priceMin
                              && product.Price <= priceMax)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetDeletedAsync()
    {
        return await _dataContext.Products
            .AsNoTracking()
            .Where(product => product.IsDeleted)
            .ToListAsync();
    }
}
