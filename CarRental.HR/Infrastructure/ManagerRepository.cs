using CarRental.BuildingBlocks.DDD;
using CarRental.HR.Model.Manager;
using Microsoft.EntityFrameworkCore;

namespace CarRental.HR.Infrastructure;

public class ManagerRepository: IManagerRepository
{
    private readonly HRContext _ctx;
    public IUnitOfWork UnitOfWork => _ctx;

    public ManagerRepository(HRContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<Manager> GetAsync(string firstName, string lastName)
    {
        var manager =  await _ctx.Managers.FirstAsync(x => x.FirstName.Equals(firstName) && x.LastName.Equals(lastName));
        var employees = await _ctx.Employees
            .Where(x =>
            x.ManagerFirstName.Equals(firstName) && x.ManagerLastName.Equals(lastName))
            .Select(x => new Employee() {FirstName = x.FirstName, LastName = x.LastName })
            .ToListAsync();

        foreach (Employee employee in employees)
            manager.Employees.Add(employee);

        return manager;
    }
}