using CarRental.BuildingBlocks.DDD;
using CarRental.HR.Events.Employee;
using CarRental.HR.Model;

namespace CarRental.HR.Model.Employee;
 
public sealed class Employee: AggregateRoot
{
    public HashSet<VacationRequest> VacationRequests {get; private set;}
    public required string FirstName { get; init;}
    public required string LastName { get; init;}
    public required Address Address{ get; init;}
    public required decimal Salary {get; init;}
    public string? ManagerFirstName { get; set; }
    public string? ManagerLastName { get; set; }

    public Employee(): base()
    {
        VacationRequests = new();
    }

    public Employee(HashSet<VacationRequest> vacationRequests): base()
    {
        VacationRequests = vacationRequests;
    }

    public void MakeVacationRequest(DateOnly start, DateOnly end, string description) 
    {
        if(end < start)
            Notification.AddError(new() {Message = "End date cannot be less than start date", Source = nameof(Employee)});

        if(VacationRequests.Any(x => x.Start.Equals(start))) {
            Notification.AddError(new() { Message = "There is another vacation request for this date", Source = nameof(Employee)});
        } else VacationRequests.Add(new VacationRequest() { Start = start, End = end, Description = description});

        if (Notification.HasErrors) throw new DomainException(Notification);
        
        Events.Add(new VacationRequestAdded() { FirstName = FirstName, LastName = LastName, Start = start, End = end});
    }

    public override bool Validate()
    {
         if(
            string.IsNullOrEmpty(FirstName)
         && string.IsNullOrEmpty(LastName))
         Notification.AddError(new() {Message = "Lack of FirstName or LastName", Source = nameof(Employee)});      

         if(!Address.Validate())
            Notification.AddError(new() { Message = "Incorrect employee address", Source = nameof(Employee)});
        
         if(Salary < 0) 
            Notification.AddError(new() { Message = "Salary cannot be set below zero", Source = nameof(Employee)});

         return !Notification.HasErrors;
    }

    private bool Equals(Employee other)
    {
        return string.Equals(FirstName,
                   other.FirstName,
                   StringComparison.CurrentCultureIgnoreCase) &&
               string.Equals(LastName,
                   other.LastName,
                   StringComparison.CurrentCultureIgnoreCase) &&
               Address.Equals(other.Address) &&
               Salary == other.Salary &&
               string.Equals(ManagerFirstName,
                   other.ManagerFirstName,
                   StringComparison.CurrentCultureIgnoreCase) &&
               string.Equals(ManagerLastName,
                   other.ManagerLastName,
                   StringComparison.CurrentCultureIgnoreCase);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is Employee other && Equals(other);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(VacationRequests);
        hashCode.Add(FirstName, StringComparer.CurrentCultureIgnoreCase);
        hashCode.Add(LastName, StringComparer.CurrentCultureIgnoreCase);
        hashCode.Add(Address);
        hashCode.Add(Salary);
        hashCode.Add(ManagerFirstName, StringComparer.CurrentCultureIgnoreCase);
        hashCode.Add(ManagerLastName, StringComparer.CurrentCultureIgnoreCase);
        return hashCode.ToHashCode();
    }

    public static bool operator ==(Employee? left, Employee? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Employee? left, Employee? right)
    {
        return !Equals(left, right);
    }
}
