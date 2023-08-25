
using System.Reflection;
using CarRental.BuildingBlocks;
using CarRental.BuildingBlocks.DDD;
using CarRental.HR.Model;
using CarRental.HR.Model.AgreementCatalog;
using CarRental.HR.Model.Employee;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Manager = CarRental.HR.Model.Manager;

namespace CarRental.HR.Infrastructure;

public class HRContext: BaseDbContext
{
    public HRContext(DbContextOptions options, IMediator mediator) : base(options, mediator) 
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("HR");
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    
    public DbSet<AgreementCatalog> AgreementCatalogs { get; private set; }
    public DbSet<Agreement> Agreements { get; private set; }
    public DbSet<Address> Addresses { get; private set; }
    public DbSet<Employee> Employees {get; private set;}
    public DbSet<VacationRequest> VacationRequests { get; private set; }
    public DbSet<Manager.Manager> Managers {get; private set;}
}
