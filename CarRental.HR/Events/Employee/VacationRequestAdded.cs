using CarRental.BuildingBlocks.DDD;
using MediatR;

namespace CarRental.HR.Events.Employee;

public record VacationRequestAdded: DomainEvent, INotification
{
    public required string FirstName {get; init;}
    public required string LastName {get; init;}
    public required DateOnly Start {get; init;}
    public required DateOnly End {get; init;}
}
