using CarRental.BuildingBlocks;
using CarRental.CarCatalog.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarRental.CarCatalog.Infrastructure;

public sealed class CarCatalogContext: BaseDbContext
{
    public CarCatalogContext(DbContextOptions options, IMediator mediator) : base(options, mediator)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("CarCatalog");
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CarCatalogContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Brand> Brands { get; private set; }
    public DbSet<Car> Cars { get; private set; }
    public DbSet<CarDetails> CarDetails { get; private set; }
}