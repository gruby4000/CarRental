namespace CarRental.BuildingBlocks.CRUD;

public interface ISoftDeletable
{
    public bool IsDeleted { get; set; }
}