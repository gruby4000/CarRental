using System.Collections.Immutable;
using CarRental.BuildingBlocks.CRUD;
using CarRental.CarCatalog.Infrastructure;
using CarRental.CarCatalog.Model;
using Microsoft.EntityFrameworkCore;

namespace CarRental.CarCatalog.Application;

public sealed class BrandService: DefaultCrudService<Brand>, ISoftDeleteEntityService<Brand>
{
    private CarCatalogContext _carCatalogContext => _ctx as CarCatalogContext;
    public BrandService(CarCatalogContext ctx) : base(ctx)
    {
    }

    public override async Task<Brand> Get(int id)
    {
        return await _carCatalogContext.Brands
            .AsTracking()
            .Include(x => x.Cars)
            .SingleAsync(x => x.Id.Equals(id));
    }

    public async Task<IReadOnlyCollection<Brand>> GetAll(int skip, int take, bool desc = true)
    {
        var query = _carCatalogContext.Brands.AsNoTracking().Skip(skip).Take(take).AsQueryable();

        if (desc)
            query = query.OrderByDescending(x => x.Id);
        else 
            query = query.OrderBy(x => x.Id);

        return (await query.Include(x => x.Cars).ToListAsync()).AsReadOnly();
    }
    
    public void SoftDelete(Brand item)
    {
        _ctx.Attach(item);
        item.IsDeleted = true;
    }
}