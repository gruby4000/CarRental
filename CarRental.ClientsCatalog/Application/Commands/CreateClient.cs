using CarRental.HR.Model;

namespace CarRental.ClientsCatalog.Application.Commands;

public record CreateClient()
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required Address CorrespondencyAddress { get; init; }
    public required string IdNumber { get; init; }
    public string? CompanyName { get; init; }
    public Address? CompanyAddress { get; init; }
    public string? CompanyTaxId { get; init; }
}