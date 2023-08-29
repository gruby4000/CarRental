using CarRental.Sales.Application.Commands;

namespace CarRental.Sales.Application;

public interface IRentalCommandHandler
{
    Task RentACar(RentACar command);
}