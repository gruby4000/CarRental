using CarRental.BuildingBlocks.DDD;
using MediatR;

namespace CarRental.HR;

public record VacationRequestEventsBase: DomainEvent, INotification
{
    public required string FirstName {get; init;}
    public required string LastName {get; init;}
    public required DateOnly Start {get; init;}
    public required DateOnly End {get; init;}
}