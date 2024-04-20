using NedoSilpo.Common.Events;
using NedoSilpo.Common.Handlers;
using NedoSilpo.Query.Domain.Entities;
using NedoSilpo.Query.Domain.Repositories;

namespace NedoSilpo.Query.Infrastructure.Handlers;

public class ClientEventHandler : 
    IEventHandler<ClientRegistered>,
    IEventHandler<ClientUpdated>,
    IEventHandler<ClientDeactivated>
{
    private readonly IClientRepository _clientRepository;

    public ClientEventHandler(IClientRepository clientRepository) => _clientRepository = clientRepository;

    public async Task On(ClientRegistered @event)
    {
        var client = new Client
        {
            Id = @event.Id,
            FullName = @event.FullName,
            Email = @event.Email,
            PhoneNumber = @event.PhoneNumber,
            Address = @event.Address,
            IsInactive = false
        };

        await _clientRepository.CreateAsync(client);
    }

    public async Task On(ClientUpdated @event)
    {
        var client = await _clientRepository.GetByIdAsync(@event.Id);

        client.FullName = @event.FullName;
        client.Email = @event.Email;
        client.PhoneNumber = @event.PhoneNumber;
        client.Address = @event.Address;

        await _clientRepository.UpdateAsync(client);
    }

    public async Task On(ClientDeactivated @event)
    {
        var client = await _clientRepository.GetByIdAsync(@event.Id);
        client.IsInactive = true;
        await _clientRepository.UpdateAsync(client);
    }
}
