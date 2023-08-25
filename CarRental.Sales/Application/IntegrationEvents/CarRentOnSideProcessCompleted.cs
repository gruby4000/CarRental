using CarRental.HR.Model;

namespace CarRental.Sales.Application.IntegrationEvents;

public record CarRentOnSideProcessCompleted()
{
    public required string CarNumber { get; init; }
    public required Address NewCarAddress { get; init; }
    public required string RentNumber { get; init; }
}