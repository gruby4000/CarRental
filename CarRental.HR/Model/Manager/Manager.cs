using CarRental.BuildingBlocks.DDD;
using CarRental.BuildingBlocks.ErrorNotification;
using CarRental.BuildingBlocks.Validation;

namespace CarRental.HR.Model.Manager;

public sealed class Manager: AggregateRoot
{
    public required string FirstName  {get; init;}
    public required string LastName {get; init;}
    public HashSet<Employee> Employees {get; init;}

    public Manager(HashSet<Employee> employees): base()
    {
        Employees = employees;        
    }

    public Manager(): base()
    {
        Employees = new();
    }
    
    public override bool Validate()
    {
         if(
            string.IsNullOrEmpty(FirstName)
         && string.IsNullOrEmpty(LastName))
         Notification.AddError(new DomainError() {Message = "Lack of FirstName or LastName", Source = nameof(Manager)});     

        if(Notification.HasErrors)
            return false;
        
        return true;
    }

    public void AssignNewEmployee(string firstName, string lastName) {
        Employee newEmployee = new () {FirstName = firstName, LastName = lastName };
        if(!newEmployee.Validate())
            throw new DomainException(newEmployee.Notification);
        Employees.Add(newEmployee);
    }

    public void AcceptVacationRequest(Employee employee, DateOnly start, DateOnly end)
    {
        if (!employee.Validate())
            throw new DomainException(employee.Notification);
        
        Events.Add(new VacationRequestAccepted() { FirstName = employee.FirstName, LastName = employee.LastName, Start = start, End = end });
    }

    public void DeclineVacationRequest(Employee employee, DateOnly start, DateOnly end) 
    { 
        if (!employee.Validate())
            throw new DomainException(employee.Notification);
        
        Events.Add(new VacationRequestDeclined() { FirstName = employee.FirstName, LastName = employee.LastName, Start = start, End = end });
    }
    
    private bool Equals(Manager other)
    {
        return string.Equals(FirstName,
                   other.FirstName,
                   StringComparison.CurrentCultureIgnoreCase) &&
               string.Equals(LastName,
                   other.LastName,
                   StringComparison.CurrentCultureIgnoreCase);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is Manager other && Equals(other);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(FirstName, StringComparer.CurrentCultureIgnoreCase);
        hashCode.Add(LastName, StringComparer.CurrentCultureIgnoreCase);
        return hashCode.ToHashCode();
    }

    public static bool operator ==(Manager? left, Manager? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Manager? left, Manager? right)
    {
        return !Equals(left, right);
    }
}    
