using CarRental.BuildingBlocks.DDD;

namespace CarRental.HR.Model.Employee;

public interface IEmployeeRepository: IRepository<Employee> {
    void Add(Employee newEmployee);
    Task<Employee> GetAsync(string firstName, string lastName);
}