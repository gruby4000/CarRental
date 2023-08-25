namespace CarRental.HR.Application.Commands;

public sealed record DeclineVacationRequest(string FirstName, string LastName, DateOnly Start, DateOnly End);