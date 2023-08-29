using CarRental.HR.Application.Models;
using CarRental.HR.Application.Queries;
using CarRental.HR.Model.Employee;

namespace CarRental.HR.Application;

public interface IEmployeesManagementQueriesHandler
{
    Task<ICollection<EmployeeDto>> AvailableEmployees(AvailableEmployees query);
    Task<ICollection<VacationRequest>> VacationRequests(VacationRequests query);
}