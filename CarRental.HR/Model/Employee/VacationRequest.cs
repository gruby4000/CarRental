using CarRental.BuildingBlocks.DDD;

namespace CarRental.HR.Model.Employee;

public sealed class VacationRequest: Entity
{
    public required DateOnly Start { get; init; }
    public required DateOnly End { get; init; }
    public required string Description { get; init; }
    public VacationRequestStatus Status { get; private set; } = VacationRequestStatus.Pending;
    private Employee Employee { get; }
    
    public override bool Validate()
    {
        if (string.IsNullOrEmpty(Description))
            Notification.AddError(new() { Message = "Vacation request must have description", Source = nameof(VacationRequest)});
        if(End < Start)
            Notification.AddError(new() {Message = "End date could not be earlier than start date", Source = nameof(VacationRequest)});

        return !Notification.HasErrors;
    }

    public void Accept() => Status = VacationRequestStatus.Accepted;
    public void Decline() => Status = VacationRequestStatus.Declined;
    
    private bool Equals(VacationRequest other)
    {
        return Start.Equals(other.Start) && End.Equals(other.End) && string.Equals(Description, other.Description, StringComparison.CurrentCultureIgnoreCase) && Status == other.Status;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is VacationRequest other && Equals(other);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(Start);
        hashCode.Add(End);
        hashCode.Add(Description, StringComparer.CurrentCultureIgnoreCase);
        hashCode.Add((int)Status);
        return hashCode.ToHashCode();
    }

    public static bool operator ==(VacationRequest? left, VacationRequest? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(VacationRequest? left, VacationRequest? right)
    {
        return !Equals(left, right);
    }
}

public enum VacationRequestStatus {
    Pending = 0,
    Accepted,
    Declined
}