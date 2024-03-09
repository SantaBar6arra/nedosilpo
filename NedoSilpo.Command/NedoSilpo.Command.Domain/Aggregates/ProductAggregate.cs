using Cqrs.Core.Domain;
using NedoSilpo.Command.Domain.Events;

namespace NedoSilpo.Command.Domain.Aggregates;

public class ProductAggregate : AggregateRoot
{
    private bool _isDeleted;
    private string _name;
    private string _description;
    private decimal _price;
    private int _quantityAvailable;

    #region Methods

    public ProductAggregate()
    {

    }

    public ProductAggregate(Guid id, string name, string description, decimal price, int quantityAvailable) // todo consider removing id
    {
        RaiseEvent(new ProductCreated(name, description, price, quantityAvailable) { Id = id }); // but use Guid.NewGuid() instead
    }

    public void Update(string name, string description, decimal price, int quantityAvailable)
    {
        if (_isDeleted)
            throw new InvalidOperationException("Product is deleted!");

        RaiseEvent(new ProductUpdated(name, description, price, quantityAvailable)); // todo consider passing id (why?)
    }

    public void Sell(int quantity)
    {
        if (_isDeleted)
            throw new InvalidOperationException("Product is deleted!");

        if (_quantityAvailable < quantity)
            throw new InvalidOperationException("Not enough quantity available!");

        RaiseEvent(new ProductSold(quantity));
    }

    public void Remove()
    {
        if (_isDeleted)
            throw new InvalidOperationException("Product is deleted!");

        RaiseEvent(new ProductRemoved(_id));
    }

    #endregion

    #region Apply

    private void Apply(ProductCreated @event)
    {
        _id = @event.Id;
        _isDeleted = false;
        _name = @event.Name;
        _description = @event.Description;
        _price = @event.Price;
        _quantityAvailable = @event.QuantityAvailable;
    }

    private void Apply(ProductUpdated @event)
    {
        _name = @event.Name;
        _description = @event.Description;
        _price = @event.Price;
        _quantityAvailable = @event.QuantityAvailable;
    }

    private void Apply(ProductSold @event)
    {
        _quantityAvailable -= @event.Quantity;
    }

    private void Apply(ProductRemoved _)
    {
        _isDeleted = true;
    }

    #endregion
}
