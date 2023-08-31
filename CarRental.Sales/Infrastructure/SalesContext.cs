using CarRental.BuildingBlocks;
using CarRental.Sales.Application.Sagas.CarRentProcess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Sales.Infrastructure;

public class SalesContext: BaseDbContext
{
    public SalesContext(DbContextOptions options, IMediator mediator) : base(options, mediator)
    {
    }
    
    
    public DbSet<CarRentProcessState> CarRentProcessStates { get; set; }
}