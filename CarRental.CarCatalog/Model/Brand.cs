using CarRental.BuildingBlocks.CRUD;
using CarRental.BuildingBlocks.DDD;

namespace CarRental.CarCatalog.Model;

public class Brand: Entity, ISoftDeletable
{
    public required string Producer { get; init; }
    public virtual List<Car> Cars { get; init; } = new();
    
    public bool IsDeleted { get; set; }
    
    public override bool Validate()
    {
        if (string.IsNullOrEmpty(Producer))
        {
            Notification.AddError(new DomainError() { Message = "Producer could not be empty", Source = nameof(Brand)});
        }

        return !Notification.HasErrors;
    }

    protected bool Equals(Brand other)
    {
        return Producer == other.Producer && IsDeleted == other.IsDeleted;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Brand)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Producer, IsDeleted);
    }

    public static bool operator ==(Brand? left, Brand? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Brand? left, Brand? right)
    {
        return !Equals(left, right);
    }
}