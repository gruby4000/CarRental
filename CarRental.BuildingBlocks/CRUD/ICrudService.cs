namespace CarRental.BuildingBlocks.CRUD;

public interface ICrudService<T> where T: class
{
    T Add(T item);
    T Update(T item);
    void Delete(T item);
    Task<T> Get(int id);
    Task SaveChangesAsync();
    void SaveChanges();
}