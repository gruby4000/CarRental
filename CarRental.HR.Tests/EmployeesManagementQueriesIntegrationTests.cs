using CarRental.HR.Application;
using CarRental.HR.Application.Queries;
using CarRental.HR.Model.Employee;
using CarRental.HR.Model.Manager;
using Employee = CarRental.HR.Model.Manager.Employee;
using EmployeeAggregate = CarRental.HR.Model.Employee;

namespace CarRental.HR.Tests;

public class EmployeesManagementQueriesIntegrationTests: Common
{
    [SetUp]
    public void Setup()
    {
        
        ServiceProviderConfig();
    }
    [Test]
    public async Task GetAvailableEmployees_StatusChangeOnly()
    {
        using var scope = _serviceScopeFactoryMock.CreateScope();
        
        var newManager = new Manager()
        {
            FirstName = "manager5",
            LastName = "manager5",
            Employees = new HashSet<Employee>()
        };
        
        var newEmployee1 = new EmployeeAggregate.Employee()
        {
            FirstName = "employee5",
            LastName = "employee5",
            Salary = 3800.20m,
            Address = new()
            {
                Street = "street", City = "warsaw", HouseNumber = "number",
                ZipCode = "02-495"
            },
            ManagerFirstName = "manager5",
            ManagerLastName = "manager5",
            VacationRequests =
            {
                new VacationRequest()
                {
                    Start = DateOnly.FromDateTime(new DateTime(2023,
                        5,
                        5)),
                    End = DateOnly.FromDateTime(new DateTime(2023,
                        5,
                        10)),
                    Description = "description1"
                }
            }
        };

        var newEmployee2 = new EmployeeAggregate.Employee()
        {
            FirstName = "employee6",
            LastName = "employee6",
            Salary = 3800.20m,
            Address = new()
            {
                Street = "street", City = "warsaw", HouseNumber = "number",
                ZipCode = "02-495"
            },
            ManagerFirstName = "manager5",
            ManagerLastName = "manager5",
            VacationRequests =
            {
                new VacationRequest()
                {
                    Start = DateOnly.FromDateTime(new DateTime(2023,
                        6,
                        5)),
                    End = DateOnly.FromDateTime(new DateTime(2023,
                        6,
                        10)),
                    Description = "description2"
                }
            }
        };
        var newEmployee3 = new EmployeeAggregate.Employee()
        {
            FirstName = "employee7",
            LastName = "employee7",
            Salary = 3800.20m,
            Address = new()
            {
                Street = "street", City = "warsaw", HouseNumber = "number",
                ZipCode = "02-495"
            },
            ManagerFirstName = "manager5",
            ManagerLastName = "manager5",
            VacationRequests =
            {
                new VacationRequest()
                {
                    Start = DateOnly.FromDateTime(new DateTime(2023,
                        7,
                        5)),
                    End = DateOnly.FromDateTime(new DateTime(2023,
                        7,
                        10)),
                    Description = "description3"
                }
            }
        };
        _ctx.Managers.Add(newManager);
        _ctx.Employees.Add(newEmployee1);
        _ctx.Employees.Add(newEmployee2);
        _ctx.Employees.Add(newEmployee3);
        _ctx.SaveChanges();
        
        var employeesManagementQueriesHandler = scope.ServiceProvider.GetService(typeof(EmployeesManagementQueriesHandler)) as EmployeesManagementQueriesHandler;

        var res = await employeesManagementQueriesHandler.AvailableEmployees(
            new AvailableEmployees(DateOnly.FromDateTime(new DateTime(2023, 5, 5)), DateOnly.FromDateTime(new DateTime(2023, 8, 10)) ));
        
        Assert.IsNotEmpty(res);
        Assert.IsTrue(res.Count == 3);
        
        
        newEmployee1.VacationRequests.ElementAt(0).Accept();
        _ctx.SaveChanges();
        
        var res2 = await employeesManagementQueriesHandler.AvailableEmployees(
            new AvailableEmployees(DateOnly.FromDateTime(new DateTime(2023, 5, 5)), DateOnly.FromDateTime(new DateTime(2023, 8, 10)) ));
        
        Assert.IsNotEmpty(res2);
        Assert.IsTrue(res2.Count == 2);
        
        newEmployee2.VacationRequests.ElementAt(0).Accept();
        _ctx.SaveChanges();
        
        var res3 = await employeesManagementQueriesHandler.AvailableEmployees(
            new AvailableEmployees(DateOnly.FromDateTime(new DateTime(2023, 5, 5)), DateOnly.FromDateTime(new DateTime(2023, 8, 10)) ));
        
        Assert.IsNotEmpty(res3);
        Assert.IsTrue(res3.Count == 1);
        
        newEmployee3.VacationRequests.ElementAt(0).Accept();
        _ctx.SaveChanges();
        
        var res4 = await employeesManagementQueriesHandler.AvailableEmployees(
            new AvailableEmployees(DateOnly.FromDateTime(new DateTime(2023, 5, 5)), DateOnly.FromDateTime(new DateTime(2023, 8, 10)) ));
        
        Assert.IsEmpty(res4);

    }
}