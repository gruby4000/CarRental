using CarRental.HR.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRental.HR.Infrastructure;

public class EFAddressTypeConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.HasAlternateKey(x => x.FrontId);
        builder.Property(x => x.Street).IsRequired();
        builder.Property(x => x.City).IsRequired();
        builder.Property(x => x.ZipCode).IsRequired();
        builder.Property(x => x.HouseNumber).IsRequired();
        
    }
}
