using CarRental.CarCatalog.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRental.CarCatalog.Infrastructure;

public sealed class EFCarEntityTypeConfiguration: IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasAlternateKey(x => x.FrontId);
        builder.HasOne<Brand>(x => x.Brand).WithMany(x => x.Cars);
        builder.HasOne<CarDetails>(x => x.Details);
        builder.Property(x => x.CarNumber).IsRequired();
        builder.Property(x => x.Model).IsRequired();

    }
}