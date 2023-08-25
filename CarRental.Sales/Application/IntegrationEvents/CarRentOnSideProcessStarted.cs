using CarRental.BuildingBlocks.ServiceIntegration;
using CarRental.HR.Model;
using CarRental.Sales.Model;

namespace CarRental.Sales.Application.IntegrationEvents;

public record CarRentOnSideProcessStarted: IEvent
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
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
    public required ClientAgreements ClientAgreements { get; init; }
    public Guid CorrelationId { get; } = new Guid();
    public DateTime Timestamp { get; } = DateTime.Now;
    public string Type { get; } = nameof(CarRentOnSideProcessStarted);
}