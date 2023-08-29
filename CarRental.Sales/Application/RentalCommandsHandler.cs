using CarRental.BuildingBlocks.DDD;
using CarRental.BuildingBlocks.ServiceIntegration;
using CarRental.Sales.Application.Commands;
using CarRental.Sales.Application.IntegrationEvents;
using CarRental.Sales.Model;

namespace CarRental.Sales.Application;

public class RentalCommandsHandler: IRentalCommandHandler
{
    private readonly IRentalRepository _rentalRepository;
    private readonly IEventBus _eventBus;

    public RentalCommandsHandler(IRentalRepository rentalRepository, IEventBus eventBus)
    {
        _rentalRepository = rentalRepository;
        _eventBus = eventBus;
    }

    public async Task RentACar(RentACar command)
    {
        if (command.CarNumber is 0)
            throw new ArgumentException("Car number is empty");
        
        var rental = await _rentalRepository.GetRental(command.RentalIdentifier);
        
        var rentalEmployee = rental.RentalEmployees.SingleOrDefault(x =>
            x.FirstName.Equals(command.RentalEmployeeFirstName) && x.LastName.Equals(command.RentalEmployeeLastName));
        
        if (rentalEmployee is null)
            throw new ArgumentException("Employee was not found");

        var client = new Client() //change it to getting data from ClientsService
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            CompanyName = command.CompanyName,
            CompanyTaxId = command.CompanyTaxId,
            CorrespondencyAddress = command.CorrespondencyAddress,
            CompanyAddress = command.CompanyAddress,
            IdNumber = command.ClientIdentifier
        };
        
        rental.RentingACar(
            rentalEmployee,
            command.CarNumber,
            client,
            command.Start,
            command.End
            );

        if (rental.Notification.HasErrors)
            throw new DomainException(rental.Notification);
        
        await _rentalRepository.UnitOfWork.SaveChangesAsync();
        
        _eventBus.Publish(new CarRentOnSideProcessStarted
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            CorrespondencyAddress = command.CorrespondencyAddress,
            IdNumber = command.IdNumber,
            CompanyName = command.CompanyName ,
            CompanyAddress = command.CompanyAddress,
            CompanyTaxId = command.CompanyTaxId,
            IsCompany = !string.IsNullOrEmpty(command.CompanyTaxId),
            RentalIdentifier = command.RentalIdentifier,
            Start = command.Start,
            End = command.End,
            CarNumber = command.CarNumber,
            ClientAgreements = null! // toDo
        });
    }
}