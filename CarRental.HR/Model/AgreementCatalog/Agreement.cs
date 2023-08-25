using CarRental.BuildingBlocks.DDD;
using CarRental.BuildingBlocks.ErrorNotification;
using CarRental.BuildingBlocks.Validation;

namespace CarRental.HR.Model.AgreementCatalog;

public sealed class Agreement: Entity, IValidatable
{
    public string AgreementNumber { get; set; } = null!;
    public required DateOnly SignDate {get; init;}
    public DateOnly? EndDate {get ;init;}
    public string? FirstName {get; init; }
    public string? LastName {get; init;}
    public string? CompanyName {get; init;}
    public string? TaxId {get; init;}
    public required Address Address {get; init;}
    public required decimal Salary {get; init;}
 
    public override bool Validate()
    {
        if(!AgreementNumber.StartsWith("AG/"))
            Notification.AddError(new DomainError() {Message = "Agreement number must starts from AG/", Source = nameof(Agreement)});
        
        if(
            string.IsNullOrEmpty(FirstName)
         && string.IsNullOrEmpty(LastName)
         && string.IsNullOrEmpty(CompanyName))
         Notification.AddError(new DomainError() {Message = "Agreement does not have 2 parties", Source = nameof(Agreement)});

        if(string.IsNullOrEmpty(TaxId) && !string.IsNullOrEmpty(CompanyName))
            Notification.AddError(new() { Message = "No TaxID provided for the company", Source = nameof(Agreement)});

        if(SignDate < DateOnly.MinValue ||  (EndDate.HasValue && SignDate > EndDate) || DateTime.Now.DayOfYear-SignDate.DayOfYear > 30)
            Notification.AddError(new() { Message = "Sign date is incorrect", Source = nameof(Agreement) });

        if(!Address.Validate())
            Notification.AddError(new() { Message = "Incorrect address", Source = nameof(Agreement)});

        return !Notification.HasErrors;
    }
    private bool Equals(Agreement other)
    {
        return string.Equals(AgreementNumber,
                   other.AgreementNumber,
                   StringComparison.CurrentCultureIgnoreCase) &&
               SignDate.Equals(other.SignDate) &&
               Nullable.Equals(EndDate,
                   other.EndDate) &&
               string.Equals(FirstName,
                   other.FirstName,
                   StringComparison.CurrentCultureIgnoreCase) &&
               string.Equals(LastName,
                   other.LastName,
                   StringComparison.CurrentCultureIgnoreCase) &&
               string.Equals(CompanyName,
                   other.CompanyName,
                   StringComparison.CurrentCultureIgnoreCase) &&
               string.Equals(TaxId,
                   other.TaxId,
                   StringComparison.CurrentCultureIgnoreCase) &&
               Address.Equals(other.Address) &&
               Salary == other.Salary;
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is Agreement other && Equals(other);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(AgreementNumber, StringComparer.CurrentCultureIgnoreCase);
        hashCode.Add(SignDate);
        hashCode.Add(EndDate);
        hashCode.Add(FirstName, StringComparer.CurrentCultureIgnoreCase);
        hashCode.Add(LastName, StringComparer.CurrentCultureIgnoreCase);
        hashCode.Add(CompanyName, StringComparer.CurrentCultureIgnoreCase);
        hashCode.Add(TaxId, StringComparer.CurrentCultureIgnoreCase);
        hashCode.Add(Address);
        hashCode.Add(Salary);
        return hashCode.ToHashCode();
    }

    public static bool operator ==(Agreement? left, Agreement? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Agreement? left, Agreement? right)
    {
        return !Equals(left, right);
    }
}