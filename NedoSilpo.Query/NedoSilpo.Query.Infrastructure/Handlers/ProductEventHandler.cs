using NedoSilpo.Common.Events;
using NedoSilpo.Common.Handlers;
using NedoSilpo.Query.Domain.Repositories;
// ReSharper disable once ConvertToPrimaryConstructor

namespace NedoSilpo.Query.Infrastructure.Handlers;

public class ProductEventHandler :
    IEventHandler<ProductCreated>,
    IEventHandler<ProductUpdated>,
    IEventHandler<ProductSold>,
    IEventHandler<ProductRemoved>
{
    private readonly IProductRepository _productRepository;

    public ProductEventHandler(IProductRepository productRepository) => _productRepository = productRepository;

    public async Task On(ProductCreated @event)
    {
        var product = new Domain.Entities.Product
        {
            Id = @event.Id,
            Name = @event.Name,
            Description = @event.Description,
            Price = @event.Price,
            QuantityAvailable = @event.QuantityAvailable
        };

        await _productRepository.CreateAsync(product);
    }

    public async Task On(ProductUpdated @event)
    {
        var product = await _productRepository.GetByIdAsync(@event.Id);

        product.Name = @event.Name;
        product.Description = @event.Description;
        product.Price = @event.Price;
        product.QuantityAvailable = @event.QuantityAvailable;

        await _productRepository.UpdateAsync(product);
    }

    public async Task On(ProductSold @event)
    {
        var product = await _productRepository.GetByIdAsync(@event.Id);
        product.QuantityAvailable -= @event.Quantity;
        await _productRepository.UpdateAsync(product);
    }

    public async Task On(ProductRemoved @event)
    {
        var product = await _productRepository.GetByIdAsync(@event.Id);
        await _productRepository.RemoveAsync(product.Id);
    }
}
