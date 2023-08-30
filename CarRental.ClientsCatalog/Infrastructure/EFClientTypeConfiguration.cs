using CarRental.ClientsCatalog.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRental.ClientsCatalog.Infrastructure;

public class EFClientTypeConfiguration: IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.FirstName).IsRequired();
        builder.Property(x => x.LastName).IsRequired();
        builder.HasAlternateKey(x => x.FrontId);
        builder.HasAlternateKey(x => x.IdNumber);
        builder.HasOne(a => a.CorrespondencyAddress);
        builder.Property(x => x.CorrespondencyAddress).IsRequired();
        builder.Property(x => x.FrontId).IsRequired();
        builder.Ignore(x => x.Notification);
    }
}