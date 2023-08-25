using CarRental.BuildingBlocks.CRUD;
using CarRental.CarCatalog.Infrastructure;
using CarRental.CarCatalog.Model;
using Microsoft.EntityFrameworkCore;

namespace CarRental.CarCatalog.Application;

public sealed class CarService: DefaultCrudService<Car>, ISoftDeleteEntityService<Car>
{
    private CarCatalogContext _carCatalogContext => _ctx as CarCatalogContext;
    
    public CarService(CarCatalogContext ctx) : base(ctx)
    {
    }

    public override async Task<Car> Get(int id)
    {
        return await _carCatalogContext.Cars
            .Include(x => x.Brand)
            .Include(x => x.Details)
            .SingleAsync(x => x.Id.Equals(id));
    }

    public async Task<IReadOnlyCollection<Car>> GetAll(int skip, int take, bool desc = true)
    {
        var query = _carCatalogContext.Cars.AsNoTracking().Skip(skip).Take(take).AsQueryable();

        if (desc)
            query = query.OrderByDescending(x => x.Id);
        else 
            query = query.OrderBy(x => x.Id);

        return (await query
            .Include(x => x.Brand)
            .Include(x => x.Details)
            .ToListAsync()).AsReadOnly();
    }
    
    public void SoftDelete(Car item)
    {
        _ctx.Attach(item);
        item.Details.IsDeleted = true;
        item.IsDeleted = true;
    }
}