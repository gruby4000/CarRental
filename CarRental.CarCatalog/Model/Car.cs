using CarRental.BuildingBlocks.CRUD;
using CarRental.BuildingBlocks.DDD;

namespace CarRental.CarCatalog.Model;

public class Car: Entity, ISoftDeletable
{
    public virtual required Brand Brand { get; init; }
    public required string Model { get; init; }
    public virtual required CarDetails Details { get; init; }
    public required int CarNumber { get; init; }
    public required bool IsAvailable { get; set; }
    public bool IsDeleted { get; set; }
    
    public override bool Validate()
    {
        
        return false;
    }
    
    private bool Equals(Car other)
    {
        return Brand.Equals(other.Brand) &&
               Model == other.Model &&
               Details.Equals(other.Details) &&
               CarNumber == other.CarNumber &&
               IsAvailable == other.IsAvailable;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is Car other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Brand, Model, Details, CarNumber);
    }

    public static bool operator ==(Car? left, Car? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Car? left, Car? right)
    {
        return !Equals(left, right);
    }

    
}