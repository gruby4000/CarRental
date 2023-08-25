namespace CarRental.BuildingBlocks.DDD;

public interface IRepository<T> where T: AggregateRoot {
    IUnitOfWork UnitOfWork {get;}
}