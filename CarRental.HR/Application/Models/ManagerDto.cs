namespace CarRental.HR.Application.Models;

public record ManagerDto()
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}; 