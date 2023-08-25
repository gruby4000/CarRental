using CarRental.BuildingBlocks.DDD;
using CarRental.HR.Model;
using MediatR;

namespace CarRental.HR.Events.AgreementCatalog;

public record AgreementSigned: DomainEvent
{
    public required string FirstName {get; init;}
    public required string LastName { get; init;}
    public required decimal Salary {get; init;}
    public required Address Address {get; init;}
    public required DateOnly SignDate {get; init;}

}
