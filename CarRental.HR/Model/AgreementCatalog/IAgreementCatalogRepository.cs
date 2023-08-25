using CarRental.BuildingBlocks.DDD;

namespace CarRental.HR.Model.AgreementCatalog;

public interface IAgreementCatalogRepository: IRepository<AgreementCatalog>
{
    Task<AgreementCatalog> GetAgreementCatalogAsync();
}
