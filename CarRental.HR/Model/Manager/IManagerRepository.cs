using CarRental.BuildingBlocks.DDD;

namespace CarRental.HR.Model.Manager;

public interface IManagerRepository: IRepository<Manager> {
    Task<Manager> GetAsync(string firstName, string lastName);
}