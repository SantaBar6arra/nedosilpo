using Microsoft.EntityFrameworkCore;
using NedoSilpo.Query.Domain.Entities;

namespace NedoSilpo.Query.Infrastructure.Context;

public class DataContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Client> Clients { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }
}
