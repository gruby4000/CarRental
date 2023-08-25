using CarRental.BuildingBlocks.Validation;
using CarRental.HR.Events.AgreementCatalog;
using CarRental.HR.Infrastructure;
using CarRental.HR.Model.AgreementCatalog;
using MediatR;
using Microsoft.EntityFrameworkCore;
using EmployeeAggregate = CarRental.HR.Model.Employee;

namespace CarRental.HR.Events.Employee.Handlers;

public sealed class AgreementSignedDomainEventHandler: INotificationHandler<AgreementSigned>
{
    private readonly EmployeeAggregate.IEmployeeRepository _employeeRepository;

    public AgreementSignedDomainEventHandler(EmployeeAggregate.IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    
    public async Task Handle(AgreementSigned notification, CancellationToken cancellationToken)
    {
        EmployeeAggregate.Employee newEmployee = new()
        {
            FirstName = notification.FirstName,
            LastName = notification.LastName,
            Address = notification.Address,
            Salary = notification.Salary
        };
        
        _employeeRepository.Add(newEmployee);
        await _employeeRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}