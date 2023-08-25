using CarRental.HR.Model;
using CarRental.HR.Model.Manager;
using Employee = CarRental.HR.Model.Employee.Employee;

namespace CarRental.HR.Application.Models;

public sealed record EmployeeDto()
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required AddressDto Address { get; init; }
    public required decimal Salary { get; init; }
    public ManagerDto? Manager { get; init; }
    
    public static  explicit operator EmployeeDto(Employee e)
    {
        return new EmployeeDto()
        {
            FirstName = e.FirstName,
            LastName = e.LastName,
            Address = (AddressDto)e.Address,
            Manager = new ManagerDto() { FirstName = e.ManagerFirstName, LastName = e.ManagerLastName },
            Salary = e.Salary
        };
    }
}