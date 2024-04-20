using Cqrs.Core.Domain;
using NedoSilpo.Common.Events;

namespace NedoSilpo.Command.Domain.Aggregates;

public class ClientAggregate : AggregateRoot
{
    private bool _isInactive;
    private string _fullName;
    private string _phoneNumber;
    private string _email;
    private string _address;

    public ClientAggregate()
    {

    }

    public ClientAggregate(string fullName, string email, string address, string phoneNumber)
    {
        RaiseEvent(new ClientRegistered(Guid.NewGuid(), fullName, email, address, phoneNumber));
    }

    public void Update(string fullName, string email, string address, string phoneNumber)
    {
        if (_isInactive)
            throw new InvalidOperationException("Client is inactive!");

        RaiseEvent(new ClientUpdated(Id, fullName, email, address, phoneNumber));
    }

    public void Deactivate()
    {
        if (_isInactive)
            throw new InvalidOperationException("Client is already inactive!");

        RaiseEvent(new ClientDeactivated(Id));
    }

    #region Apply

    private void Apply(ClientRegistered @event)
    {
        _id = @event.Id;
        _fullName = @event.FullName;
        _email = @event.Email;
        _address = @event.Address;
        _phoneNumber = @event.PhoneNumber;
    }

    private void Apply(ClientUpdated @event)
    {
        _id = @event.Id;
        _fullName = @event.FullName;
        _email = @event.Email;
        _address = @event.Address;
        _phoneNumber = @event.PhoneNumber;
    }

    private void Apply(ClientDeactivated @event)
    {
        _id = @event.Id;
        _isInactive = true;
    }

    #endregion
}
