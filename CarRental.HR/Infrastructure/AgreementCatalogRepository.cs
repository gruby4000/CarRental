using CarRental.BuildingBlocks.DDD;
using CarRental.HR.Model.AgreementCatalog;
using Microsoft.EntityFrameworkCore;

namespace CarRental.HR.Infrastructure;

public class AgreementCatalogRepository: IAgreementCatalogRepository
{
    private readonly HRContext _ctx;
    public IUnitOfWork UnitOfWork => _ctx;

    public AgreementCatalogRepository(HRContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<AgreementCatalog> GetAgreementCatalogAsync()
    {
        return await _ctx.AgreementCatalogs.FirstAsync(x => x.Year.Equals(DateTime.Now.Year));
        
    }
}