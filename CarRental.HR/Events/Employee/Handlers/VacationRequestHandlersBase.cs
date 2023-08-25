using CarRental.HR.Model.Employee;
using MediatR;

namespace CarRental.HR.Events.Employee.Handlers;

public abstract class VacationRequestHandlersBase
{
    private readonly IEmployeeRepository _employeeRepository;

    public VacationRequestHandlersBase(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    
    protected async Task<VacationRequest> GetVacationRequest(VacationRequestEventsBase notification, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetAsync(notification.FirstName, notification.LastName);

        var vacationRequest = employee.VacationRequests.FirstOrDefault(x =>
            x.Start.Equals(notification.Start) && x.End.Equals(notification.End) && x.Status ==
            VacationRequestStatus.Pending);

        if (vacationRequest is null)
            throw new ArgumentException($@"Vacation Request could not be found for the employee {notification.FirstName} {notification.LastName}");

        return vacationRequest;
    }
}