using CarRental.BuildingBlocks.CRUD;
using CarRental.BuildingBlocks.DDD;

namespace CarRental.CarCatalog.Model;

public class CarDetails: Entity, ISoftDeletable
{
    public required EngineType EngineType { get; init; }
    public required float EngineCapacity { get; init; }
    public required DriveType DriveType { get; init; }
    public required string Color { get; init; }
    public bool IsDeleted { get; set; }
    public override bool Validate()
    {
        if (EngineCapacity < 0.1f &&
            EngineType != EngineType.Electric &&
            EngineType != EngineType.ElectricTwinMotor &&
            EngineType != EngineType.ElectricTripleMotor &&
            EngineType != EngineType.ElectricAllWheelMotor)
            Notification.AddError(new() { Message = "Capacity of engine is too small", Source = nameof(CarDetails)});

        if(string.IsNullOrEmpty(Color)) 
            Notification.AddError(new() {Message = "Car must have color provided", Source = nameof(CarDetails)});
        
        return !Notification.HasErrors;
    }

    protected bool Equals(CarDetails other)
    {
        return EngineType == other.EngineType &&
               EngineCapacity.Equals(other.EngineCapacity) &&
               DriveType == other.DriveType &&
               Color == other.Color &&
               IsDeleted == other.IsDeleted;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((CarDetails)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)EngineType, EngineCapacity, (int)DriveType, Color);
    }

    public static bool operator ==(CarDetails? left, CarDetails? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(CarDetails? left, CarDetails? right)
    {
        return !Equals(left, right);
    }
}

public enum DriveType
{
    FWD,
    RWD,
    AWD,
    D4x4
}

public enum EngineType
{
    R4,
    R2,
    R3,
    V4,
    Boxer4,
    Boxer2,
    Boxer6,
    Boxer5,
    R5,
    V6,
    V8,
    V10,
    V12,
    V16,
    W8,
    W16,
    Electric,
    ElectricTwinMotor,
    ElectricTripleMotor,
    ElectricAllWheelMotor
        
}