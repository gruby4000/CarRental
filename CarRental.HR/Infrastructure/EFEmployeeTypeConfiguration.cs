using CarRental.HR.Model.Employee;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRental.HR.Infrastructure;

public class EFEmployeeTypeConfiguration: IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.HasAlternateKey(x => x.FrontId);
        builder.HasKey(x => new {x.FirstName, x.LastName });
        builder.HasOne(x => x.Address);
        builder.Property(x => x.FirstName).IsRequired();
        builder.Property(x => x.LastName).IsRequired();
        builder.Property(x => x.Salary).IsRequired();
        builder.Ignore(x => x.Events);
        builder.Ignore(x => x.Notification);
        builder.HasMany(x => x.VacationRequests);
    }
}