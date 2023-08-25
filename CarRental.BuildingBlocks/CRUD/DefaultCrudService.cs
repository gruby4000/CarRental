using Microsoft.EntityFrameworkCore;

namespace CarRental.BuildingBlocks.CRUD;

public abstract class DefaultCrudService<T>: ICrudService<T> where T: class
{
    protected readonly DbContext _ctx;

    public DefaultCrudService(DbContext ctx)
    {
        _ctx = ctx;
    }

    public virtual T Add(T item)
    {
        _ctx.Add(item);

        return item;
    }

    public virtual T Update(T item)
    {
        _ctx.Attach(item);
        var entry =_ctx.Entry(item);
        entry.State = EntityState.Modified;

        return item;
    }

    public virtual void Delete(T item)
    {
        _ctx.Remove(item);
    }
    

    public virtual async Task<T> Get(int id)
    {
        return await _ctx.Set<T>().AsNoTracking().SingleAsync(x => (x as IIdentifable).Id.Equals(id));
    }

    public virtual async Task SaveChangesAsync()
    {
        await _ctx.SaveChangesAsync();
    }

    public virtual void SaveChanges()
    {
        _ctx.SaveChanges();
    }
}