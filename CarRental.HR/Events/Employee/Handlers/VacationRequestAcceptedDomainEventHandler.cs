using CarRental.HR.Model.Employee;
using MediatR;

namespace CarRental.HR.Events.Employee.Handlers;

public sealed class VacationRequestAcceptedDomainEventHandler: VacationRequestHandlersBase, INotificationHandler<VacationRequestAccepted>
{
    private readonly IEmployeeRepository _employeeRepository;

    public VacationRequestAcceptedDomainEventHandler(IEmployeeRepository employeeRepository): base(employeeRepository)
    {
    }
    
    public async Task Handle(VacationRequestAccepted notification, CancellationToken cancellationToken)
    {
        var vacationRequest = await GetVacationRequest(notification, cancellationToken);

        vacationRequest.Accept();

        await _employeeRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}