using CarRental.BuildingBlocks.DDD;
using CarRental.HR.Model;

namespace CarRental.Sales.Events;

public record CarRentOnSide: DomainEvent
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string RentNumber { get; init; }
    public required Address CorrespondencyAddress { get; init; }
    public required string IdNumber { get; init; }
    public string? CompanyName { get; init; }
    public Address? CompanyAddress { get; init; }
    public string? CompanyTaxId { get; init; }
    public required bool IsCompany { get; init; }
    public required string RentalIdentifier { get; init;}
    public required DateTime Start { get; init; }
    public required DateTime End { get; init; }
    public int CarNumber { get; init; }
}