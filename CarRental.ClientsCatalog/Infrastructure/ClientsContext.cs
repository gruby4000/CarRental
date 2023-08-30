using CarRental.BuildingBlocks;
using CarRental.ClientsCatalog.Model;
using CarRental.HR.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarRental.ClientsCatalog.Infrastructure;

public sealed class ClientsContext: BaseDbContext
{
    public ClientsContext(DbContextOptions options, IMediator mediator) : base(options, mediator)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("ClientsCatalog");
        base.OnConfiguring(optionsBuilder);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClientsContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Client> Clients { get; private set; }
    public DbSet<Address> Addresses { get; private set; }
    public DbSet<ClientAgreements> ClientAgreements { get; private set; }
}