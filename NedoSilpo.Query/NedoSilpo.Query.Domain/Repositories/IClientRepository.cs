using NedoSilpo.Query.Domain.Entities;

namespace NedoSilpo.Query.Domain.Repositories;

public interface IClientRepository
{
    Task CreateAsync(Client client);
    Task UpdateAsync(Client client);
    Task RemoveAsync(Guid id);

    Task<Client> GetByIdAsync(Guid id);
    Task<IEnumerable<Client>> GetAllAsync();
}
