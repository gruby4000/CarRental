using CarRental.HR.Model.AgreementCatalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRental.HR.Infrastructure;

public class EFAgreementCatalogTypeConfiguration: IEntityTypeConfiguration<AgreementCatalog>
{
    public void Configure(EntityTypeBuilder<AgreementCatalog> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.HasAlternateKey(x => x.FrontId);
        builder.HasMany(x => x.Agreements);
        builder.Ignore(x => x.Events);
        builder.Ignore(x => x.Notification);
    }
}