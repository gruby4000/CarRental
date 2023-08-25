using CarRental.HR.Application;
using CarRental.HR.Application.Commands;
using CarRental.HR.Application.Models;
using CarRental.HR.Application.Queries;
using CarRental.HR.Model.AgreementCatalog;
using CarRental.HR.Model.Employee;
using CarRental.HR.Model.Manager;
using Microsoft.EntityFrameworkCore;
using Employee = CarRental.HR.Model.Manager.Employee;
using EmployeeAggregate = CarRental.HR.Model.Employee;

namespace CarRental.HR.Tests;

public class EmployeesManagementServiceIntegrationTests: Common
{
    [SetUp]
    public void Setup()
    {
        ServiceProviderConfig();
    }
    
    [Test]
    public async Task HireNewEmployee()
    {
        using var scope = _serviceScopeFactoryMock.CreateScope();

        var employeesManagementService = scope.ServiceProvider.GetService(typeof(EmployeesManagementCommandsHandler)) as EmployeesManagementCommandsHandler;
         
        
        await employeesManagementService.HireNewEmployeeAsync(new HireNewEmployee()
        {
            FirstName = "Pawel2",
            LastName = "Pawlowski",
            Salary = 3800.20m,
            SignDate = DateOnly.FromDateTime(new DateTime(2023, 10, 10)),
            Address = new()
            {
                Street = "street", City = "warsaw", HouseNumber = "number",
                ZipCode = "02-495"
            },
            EndDate = DateOnly.FromDateTime(new DateTime(2023, 10, 10).AddDays(360))
        });
        
        
        var employee = _ctx.Employees.FirstOrDefault(x => x.FirstName.Equals("Pawel2") && x.LastName.Equals("Pawlowski"));
        var catalog = _ctx.AgreementCatalogs.Include(x => x.Agreements).FirstOrDefault();
        
        Assert.IsNotNull(employee);
        Assert.IsNotNull(catalog);
        
        Assert.IsNotEmpty(catalog.Agreements);
        
        Assert.IsTrue(catalog.Agreements.First().Equals(new Agreement()
        {
            AgreementNumber = "AG/2023/000001",
            FirstName = "Pawel2",
            LastName = "Pawlowski",
            Salary = 3800.20m,
            SignDate = DateOnly.FromDateTime(new DateTime(2023, 10, 10)),
            Address = new()
            {
                Street = "street", City = "warsaw", HouseNumber = "number",
                ZipCode = "02-495"
            },
            EndDate = DateOnly.FromDateTime(new DateTime(2023, 10, 10).AddDays(360))
        }));
        
        Assert.IsTrue(employee.Equals(new EmployeeAggregate.Employee()
        {
            FirstName = "Pawel2",
            LastName = "Pawlowski",
            Salary = 3800.20m,
            Address = new()
            {
                Street = "street", City = "warsaw", HouseNumber = "number",
                ZipCode = "02-495"
            }
        }));
        
        scope.Dispose();
    }

    [Test]
    public async Task AssignManagerToEmployee()
    {
        using var scope = _serviceScopeFactoryMock.CreateScope();
        
        var newEmployee = new EmployeeAggregate.Employee()
        {
            FirstName = "Pawel3",
            LastName = "Pawlowski",
            Salary = 3800.20m,
            Address = new()
            {
                Street = "street", City = "warsaw", HouseNumber = "number",
                ZipCode = "02-495"
            }
        };

        var newManager = new Manager()
        {
            FirstName = "manager1",
            LastName = "manager1",
            Employees = new HashSet<Employee>()
        };
        
        _ctx.Employees.Add(newEmployee);
        _ctx.Managers.Add(newManager);

        await _ctx.SaveChangesAsync();
        
        var employeesManagementService = scope.ServiceProvider.GetService(typeof(EmployeesManagementCommandsHandler)) as EmployeesManagementCommandsHandler;
        var managerRepository = scope.ServiceProvider.GetService(typeof(IManagerRepository)) as IManagerRepository;
        
        await employeesManagementService.AssignManagerToEmployeeAsync(new AssignManagerToEmployee("Pawel3", "Pawlowski",
            "manager1", "manager1"));

        var manager = await managerRepository.GetAsync("manager1", "manager1");
        
        Assert.That(manager, Is.EqualTo(newManager));
        Assert.That(manager.Employees.Count, Is.EqualTo(1));

        var managerEmployee = manager.Employees.First();
        
        Assert.That(managerEmployee.FirstName, Is.EqualTo(newEmployee.FirstName));
        Assert.That(managerEmployee.LastName, Is.EqualTo(newEmployee.LastName));
        
        scope.Dispose();
    }

    [Test]
    public async Task RegisterVacationRequest()
    {
        using var scope = _serviceScopeFactoryMock.CreateScope();
        
        var employeesManagementService = scope.ServiceProvider.GetService(typeof(EmployeesManagementCommandsHandler)) as EmployeesManagementCommandsHandler;
        
        var newEmployee = new EmployeeAggregate.Employee()
        {
            FirstName = "Pawel4",
            LastName = "Pawlowski",
            Salary = 3800.20m,
            Address = new()
            {
                Street = "street", City = "warsaw", HouseNumber = "number",
                ZipCode = "02-495"
            }
        };

        _ctx.Add(newEmployee);
        await _ctx.SaveChangesAsync();

        await employeesManagementService.RegisterVacationRequestAsync(new RegisterVacationRequest("Pawel4",
            "Pawlowski",
            DateOnly.FromDateTime(new DateTime(2023,
                10,
                10)),
            DateOnly.FromDateTime(new DateTime(2023,
                10,
                20)), "Description"));

        var employeeRepository = scope.ServiceProvider.GetService(typeof(IEmployeeRepository)) as IEmployeeRepository;

        var fromDb = await employeeRepository.GetAsync("Pawel4", "Pawlowski");
        
        Assert.That(fromDb.VacationRequests.Count, Is.EqualTo(1));
        Assert.IsTrue(fromDb.VacationRequests.First()
            .Equals(new VacationRequest()
            {
                Start = DateOnly.FromDateTime(new DateTime(2023,10,10)),
                End = DateOnly.FromDateTime(new DateTime(2023,10,20)),
                Description = "Description"
            }));
        
        scope.Dispose();
    }

    [Test]
    public async Task AcceptVacationRequest()
    {
        using var scope = _serviceScopeFactoryMock.CreateScope();

        var (newEmployee, vacationRequest) = await VacationRequestTestBase("employee1", "manager3");
        
        var employeesManagementService = scope.ServiceProvider.GetService(typeof(EmployeesManagementCommandsHandler)) as EmployeesManagementCommandsHandler;
        var employeeRepository = scope.ServiceProvider.GetService(typeof(IEmployeeRepository)) as IEmployeeRepository;

        await employeesManagementService.AcceptVacationRequestAsync(new AcceptVacationRequest(newEmployee.FirstName,
            newEmployee.LastName, vacationRequest.Start, vacationRequest.End));

        var employee = await employeeRepository.GetAsync(newEmployee.FirstName, newEmployee.LastName);
        
        Assert.IsTrue(employee.VacationRequests.First().Status == VacationRequestStatus.Accepted);
        
        scope.Dispose();
    }

    [Test]
    public async Task DeclineVacationRequest()
    {
        using var scope = _serviceScopeFactoryMock.CreateScope();

        var (newEmployee, vacationRequest) = await VacationRequestTestBase("employee2", "manager4");
        
        var employeesManagementService = scope.ServiceProvider.GetService(typeof(EmployeesManagementCommandsHandler)) as EmployeesManagementCommandsHandler;
        var employeeRepository = scope.ServiceProvider.GetService(typeof(IEmployeeRepository)) as IEmployeeRepository;

        await employeesManagementService.DeclineVacationRequestAsync(new DeclineVacationRequest(newEmployee.FirstName,
            newEmployee.LastName, vacationRequest.Start, vacationRequest.End));

        var employee = await employeeRepository.GetAsync(newEmployee.FirstName, newEmployee.LastName);
        
        Assert.IsTrue(employee.VacationRequests.First().Status == VacationRequestStatus.Declined);
        
        scope.Dispose();
    }
    
    private async Task<(EmployeeAggregate.Employee, VacationRequest)> VacationRequestTestBase(string employeeName, string managerName)
    {
        var newEmployee = new EmployeeAggregate.Employee()
        {
            FirstName = employeeName,
            LastName = employeeName,
            Salary = 3800.20m,
            Address = new()
            {
                Street = "street", City = "warsaw", HouseNumber = "number",
                ZipCode = "02-495"
            },
            ManagerFirstName = managerName,
            ManagerLastName = managerName,
        };

        var newManager = new Manager()
        {
            FirstName = managerName,
            LastName = managerName,
            Employees = new HashSet<Employee>()
        };

        var vacationRequest = new VacationRequest()
        {
            Start = DateOnly.FromDateTime(new DateTime(2023,
                10,
                10)),
            End = DateOnly.FromDateTime(new DateTime(2023,
                10,
                20)),
            Description = "Description"
        };

        newEmployee.VacationRequests.Add(vacationRequest);
        
        _ctx.Employees.Add(newEmployee);
        _ctx.Managers.Add(newManager);

        await _ctx.SaveChangesAsync();

        return (newEmployee, vacationRequest);
    }
    
}