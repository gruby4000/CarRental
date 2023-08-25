namespace CarRental.BuildingBlocks;

public interface IIdentifable
{
     int Id {get; init;}
     Guid FrontId {get; init;}
}
