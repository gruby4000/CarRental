using CarRental.HR.Application.Models;
using CarRental.HR.Application.Queries;
using CarRental.HR.Infrastructure;
using CarRental.HR.Model.Employee;
using Microsoft.EntityFrameworkCore;

namespace CarRental.HR.Application;

public sealed class EmployeesManagementQueriesHandler
{
    private readonly HRContext _ctx;

    public EmployeesManagementQueriesHandler(HRContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<ICollection<EmployeeDto>> AvailableEmployees(AvailableEmployees query)
    {
        var res = await _ctx.Employees.Where(x => !x.VacationRequests.Any(y => y.Start >= query.Start && y.End <= query.End && y.Status == VacationRequestStatus.Accepted)).ToListAsync();

        return res.Select(x => (EmployeeDto)x).ToList();
    }

    public async Task<ICollection<VacationRequest>> VacationRequests(VacationRequests query)
    {
        var res = new List<VacationRequest>();
        
        if ((query.FirstName is not null && query.LastName is null) ||
            (query.LastName is not null && query.FirstName is null))
        {
            throw new ArgumentException("Provide both FirstName and LastName");
        }

        if (!string.IsNullOrEmpty(query.FirstName) && !string.IsNullOrEmpty(query.LastName))
        {
            var employee = await _ctx.Employees.Include(x => x.VacationRequests).SingleOrDefaultAsync(x =>
                x.FirstName.Equals(query.FirstName) && x.LastName.Equals(query.LastName));

            if (employee is null)
                throw new ArgumentException("Employee not found");

            res  = employee.VacationRequests.Where(x => x.Start >= query.Start && x.Status == query.Status).ToList();
            if (query.End.HasValue) res = res.Where(x => x.End <= query.End).ToList();
        }
        else
        {
            var requestsQuery = _ctx.VacationRequests.AsQueryable();
        
            requestsQuery = requestsQuery.Where(x => x.Start >= query.Start && x.Status == query.Status);

            if (query.End.HasValue) requestsQuery = requestsQuery.Where(x => x.End <= query.End);

            res = await requestsQuery.ToListAsync();
        }

        return res;
    }
}