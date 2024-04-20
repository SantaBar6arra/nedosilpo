using Microsoft.EntityFrameworkCore;
using NedoSilpo.Query.Domain.Entities;
using NedoSilpo.Query.Domain.Repositories;
using NedoSilpo.Query.Infrastructure.Context;

namespace NedoSilpo.Query.Infrastructure.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly DataContext _dataContext;

    public ClientRepository(DataContext dataContext) => _dataContext = dataContext;

    public async Task CreateAsync(Client client)
    {
        _dataContext.Clients.Add(client);
        await _dataContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Client client)
    {
        _dataContext.Clients.Update(client);
        await _dataContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(Guid id)
    {
        var client = await _dataContext.Clients.FindAsync(id);
        if (client is null)
            return;

        _dataContext.Remove(client);
        await _dataContext.SaveChangesAsync();
    }

    public async Task<Client> GetByIdAsync(Guid id)
    {
        return await _dataContext.Clients.FindAsync(id) ?? throw new Exception("client not found");
    }

    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        return await _dataContext.Clients
            .Where(client => !client.IsInactive)
            .AsNoTracking()
            .ToListAsync();
    }
}
