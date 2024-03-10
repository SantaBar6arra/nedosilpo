using Cqrs.Core.Domain;
using NedoSilpo.Common.Events;

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

    public ProductAggregate(string name, string description, decimal price, int quantityAvailable)
    {
        RaiseEvent(new ProductCreated(Id, name, description, price, quantityAvailable));
    }

    public void Update(string name, string description, decimal price, int quantityAvailable)
    {
        if (_isDeleted)
            throw new InvalidOperationException("Product is deleted!");

        RaiseEvent(new ProductUpdated(Id, name, description, price, quantityAvailable));
    }

    public void Sell(int quantity)
    {
        if (_isDeleted)
            throw new InvalidOperationException("Product is deleted!");

        if (_quantityAvailable < quantity)
            throw new InvalidOperationException("Not enough quantity available!");

        RaiseEvent(new ProductSold(Id, quantity));
    }

    public void Remove()
    {
        if (_isDeleted)
            throw new InvalidOperationException("Product is deleted!");

        RaiseEvent(new ProductRemoved(Id));
    }

    #endregion

    #region Apply

    private void Apply(ProductCreated @event)
    {
        Id = @event.Id;
        _isDeleted = false;
        _name = @event.Name;
        _description = @event.Description;
        _price = @event.Price;
        _quantityAvailable = @event.QuantityAvailable;
    }

    private void Apply(ProductUpdated @event)
    {
        Id = @event.Id; // Not necessary, but for the sake of consistency
        _name = @event.Name;
        _description = @event.Description;
        _price = @event.Price;
        _quantityAvailable = @event.QuantityAvailable;
    }

    private void Apply(ProductSold @event)
    {
        Id = @event.Id;
        _quantityAvailable -= @event.Quantity;
    }

    private void Apply(ProductRemoved @event)
    {
        Id = @event.Id;
        _isDeleted = true;
    }

    #endregion
}
