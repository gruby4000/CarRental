namespace CarRental.HR.Application.Commands;

public sealed record AssignManagerToEmployee(string EmployeeFirstName, string EmployeeLastName, string ManagerFirstName, string ManagerLastName);