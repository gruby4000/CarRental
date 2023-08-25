using CarRental.HR.Model.Employee;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRental.HR.Infrastructure;

public class EFVacationRequestTypeConfiguration: IEntityTypeConfiguration<VacationRequest>
{
    public void Configure(EntityTypeBuilder<VacationRequest> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.HasAlternateKey(x => x.FrontId);
        builder.Property(x => x.Start).IsRequired();
        builder.Property(x => x.End).IsRequired();
        builder.Property(x => x.Description).IsRequired();
        builder.Ignore(x => x.Notification);
    }
}