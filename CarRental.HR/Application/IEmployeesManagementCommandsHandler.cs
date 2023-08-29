using CarRental.HR.Application.Commands;

namespace CarRental.HR.Application;

public interface IEmployeesManagementCommandsHandler
{
    Task HireNewEmployeeAsync(HireNewEmployee command);
    Task AssignManagerToEmployeeAsync(AssignManagerToEmployee command);
    Task RegisterVacationRequestAsync(RegisterVacationRequest command);
    Task AcceptVacationRequestAsync(AcceptVacationRequest command);
    Task DeclineVacationRequestAsync(DeclineVacationRequest command);
}