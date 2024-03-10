using Microsoft.EntityFrameworkCore;
using NedoSilpo.Query.Domain.Entities;

namespace NedoSilpo.Query.Infrastructure.Context;

public class DataContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }
}
