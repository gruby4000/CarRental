using CarRental.CarCatalog.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRental.CarCatalog.Infrastructure;

public sealed class EFBrandEntityTypeConfiguration: IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasAlternateKey(x => x.FrontId);
        builder.HasMany(x => x.Cars).WithOne(x => x.Brand).HasForeignKey(x => x.Id);
        builder.Property(x => x.Producer).IsRequired();
    }
}