using System.Security.Cryptography.X509Certificates;
using CarRental.HR.Model.AgreementCatalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRental.HR.Infrastructure;

public sealed class EFAgreementTypeConfiguration: IEntityTypeConfiguration<Agreement>
{
    public void Configure(EntityTypeBuilder<Agreement> builder)
    {
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.HasAlternateKey(x => x.FrontId);
        builder.HasOne(a => a.Address);
        builder.Property(x => x.AgreementNumber).IsRequired();
        builder.Property(x => x.Salary).IsRequired().HasPrecision(2);
        
        builder.Property(x => x.SignDate).IsRequired();
        builder.Property(x => x.FrontId).IsRequired();
        builder.Ignore(x => x.Notification);
    }
}
