using CarRental.BuildingBlocks.DDD;
using CarRental.HR.Model.Employee;
using Microsoft.EntityFrameworkCore;

namespace CarRental.HR.Infrastructure;

public class EmployeeRepository: IEmployeeRepository
{
    private readonly HRContext _ctx;
    public IUnitOfWork UnitOfWork => _ctx;

    public EmployeeRepository(HRContext ctx)
    {
        _ctx = ctx;
    }
    
    public void Add(Employee newEmployee)
    {
        _ctx.Employees.Add(newEmployee);
    }

    public async Task<Employee> GetAsync(string firstName, string lastName)
    {
        var employee = await _ctx.Employees.SingleOrDefaultAsync(x =>
            x.FirstName.Equals(firstName) && x.LastName.Equals(lastName));
        
        if (employee is null) throw new ArgumentException($@"No employee with name: {firstName} {lastName}");

        return employee;
    }
}