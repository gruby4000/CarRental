using CarRental.HR.Model.Employee;
using MediatR;

namespace CarRental.HR.Events.Employee.Handlers;

public sealed class VacationRequestDeclinedDomainEventHandler: VacationRequestHandlersBase, INotificationHandler<VacationRequestDeclined>
{
    private readonly IEmployeeRepository _employeeRepository;

    public VacationRequestDeclinedDomainEventHandler(IEmployeeRepository employeeRepository): base(employeeRepository)
    {
    }
    
    public async Task Handle(VacationRequestDeclined notification, CancellationToken cancellationToken)
    {
        var vacationRequest = await GetVacationRequest(notification, cancellationToken);

        vacationRequest.Decline();

        await _employeeRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}