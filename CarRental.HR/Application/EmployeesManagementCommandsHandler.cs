using CarRental.HR.Application.Commands;
using System;
using System.Runtime.InteropServices.JavaScript;
using CarRental.BuildingBlocks.DDD;
using CarRental.HR.Model.AgreementCatalog;
using EmployeeAggregate = CarRental.HR.Model.Employee;
using CarRental.HR.Model.Manager;
using ManagerAggregate = CarRental.HR.Model.Manager;

namespace CarRental.HR.Application;

public sealed class EmployeesManagementCommandsHandler: IEmployeesManagementCommandsHandler
{
    private readonly IAgreementCatalogRepository _agreementCatalogRepository;
    private readonly IManagerRepository _managerRepository;
    private readonly EmployeeAggregate.IEmployeeRepository _employeeRepository;

    public EmployeesManagementCommandsHandler(IAgreementCatalogRepository agreementCatalogRepository, IManagerRepository managerRepository, EmployeeAggregate.IEmployeeRepository employeeRepository)
    {
        _agreementCatalogRepository = agreementCatalogRepository;
        _managerRepository = managerRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task HireNewEmployeeAsync(HireNewEmployee command) 
    {
        var catalog = await _agreementCatalogRepository.GetAgreementCatalogAsync();
        
        catalog.SignNewAgreementWithEmployee(new Agreement()
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Salary = command.Salary,
            SignDate = command.SignDate,
            EndDate = command.EndDate,
            Address = command.Address
        });
        
        if (catalog.Notification.HasErrors)
            throw new DomainException(catalog.Notification);
        
        await _agreementCatalogRepository.UnitOfWork.SaveChangesAsync();   
    }

    public async Task AssignManagerToEmployeeAsync(AssignManagerToEmployee command)
    {
        var manager = await _managerRepository.GetAsync(command.ManagerFirstName, command.ManagerLastName);
        
        manager.AssignNewEmployee(command.EmployeeFirstName, command.EmployeeLastName);

        if(manager.Notification.HasErrors)
            throw new DomainException(manager.Notification);

        await _managerRepository.UnitOfWork.SaveChangesAsync();
    }

    public async Task RegisterVacationRequestAsync(RegisterVacationRequest command)
    {
        var employee = await _employeeRepository.GetAsync(command.FirstName, command.LastName);
        
        employee.MakeVacationRequest(command.Start, command.End, command.Description);

        if (employee.Notification.HasErrors)
            throw new DomainException(employee.Notification);

        await _employeeRepository.UnitOfWork.SaveChangesAsync();
    }

    public async Task AcceptVacationRequestAsync(AcceptVacationRequest command)
    {
        var (manager, employee, vacationRequest) = await VacationRequestProcessBase(command.FirstName, command.LastName, command.Start, command.End);
        
        manager.AcceptVacationRequest(new ManagerAggregate.Employee() { FirstName = employee.FirstName, LastName = employee.LastName }, vacationRequest.Start, vacationRequest.End);

        if(manager.Notification.HasErrors)
            throw new DomainException(manager.Notification);
        
        await _managerRepository.UnitOfWork.SaveChangesAsync();
    }
    
    public async Task DeclineVacationRequestAsync(DeclineVacationRequest command)
    {
        var (manager, employee, vacationRequest) = await VacationRequestProcessBase(command.FirstName, command.LastName, command.Start, command.End);
        
        manager.DeclineVacationRequest(new ManagerAggregate.Employee() { FirstName = employee.FirstName, LastName = employee.LastName }, vacationRequest.Start, vacationRequest.End);

        if(manager.Notification.HasErrors)
            throw new DomainException(manager.Notification);
        
        await _managerRepository.UnitOfWork.SaveChangesAsync();
    }
    
    private async Task<(Manager manager, EmployeeAggregate.Employee employee, EmployeeAggregate.VacationRequest vacationRequest)>
        VacationRequestProcessBase(string firstName, string lastName, DateOnly start, DateOnly end)
    {
        var employee = await _employeeRepository.GetAsync(firstName, lastName);

        var vacationRequest =
            employee.VacationRequests.FirstOrDefault(x => x.Start.Equals(start) && x.End.Equals(end));
        
        if (vacationRequest is null)
            throw new ArgumentException("No vacation request with this date range");

        if (string.IsNullOrEmpty(employee.ManagerFirstName) || string.IsNullOrEmpty(employee.ManagerLastName))
            throw new DomainException("Assign manager first");

        var manager = await _managerRepository.GetAsync(employee.ManagerFirstName, employee.ManagerLastName);
        
        return (manager, employee, vacationRequest);
    }
}
