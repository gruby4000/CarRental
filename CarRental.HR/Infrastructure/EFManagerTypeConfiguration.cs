using CarRental.HR.Model.Manager;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRental.HR.Infrastructure;

public class EFManagerTypeConfiguration: IEntityTypeConfiguration<Manager>
{
    public void Configure(EntityTypeBuilder<Manager> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.HasAlternateKey(x => x.FrontId);
        builder.HasKey(x => new {x.FirstName, x.LastName });
        builder.Property(x => x.FirstName).IsRequired();
        builder.Property(x => x.LastName).IsRequired();
        builder.Ignore(x => x.Employees);
        builder.Ignore(x => x.Events);
        builder.Ignore(x => x.Notification);
    }
}