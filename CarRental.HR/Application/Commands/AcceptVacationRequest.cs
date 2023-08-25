namespace CarRental.HR.Application.Commands;

public record AcceptVacationRequest(string FirstName, string LastName, DateOnly Start, DateOnly End)
{
    
}