namespace CarRental.BuildingBlocks.CRUD;

public interface ISoftDeleteEntityService<ISoftDeletable>
{
    public void SoftDelete(ISoftDeletable item);
}