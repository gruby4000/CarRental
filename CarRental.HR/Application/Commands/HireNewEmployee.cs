using CarRental.HR.Model;

namespace CarRental.HR.Application.Commands;

public sealed record HireNewEmployee {
    public required DateOnly SignDate {get; init;}
    public DateOnly? EndDate {get ;init;}
    public required string FirstName {get; init; }
    public required string LastName {get; init;}
    public required Address Address {get; init;}
    public required decimal Salary {get; init;}
}