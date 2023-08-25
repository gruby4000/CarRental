namespace CarRental.HR.Application.Commands;

public record RegisterVacationRequest(string FirstName, string LastName, DateOnly Start, DateOnly End, string Description);