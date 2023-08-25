using CarRental.HR.Model.Employee;

namespace CarRental.HR.Application.Queries;

public sealed record VacationRequests(DateOnly Start, DateOnly? End, VacationRequestStatus Status, string? FirstName, string? LastName);