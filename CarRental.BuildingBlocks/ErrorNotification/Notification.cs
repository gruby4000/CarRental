namespace CarRental.BuildingBlocks.ErrorNotification;

public sealed class Notification<T> where T: class 
{
    public HashSet<T> Errors { get; private set; }
    public bool HasErrors => Errors.Any();
    public Notification()
    {
        Errors = new();
    }
    public void AddError(T error) => Errors.Add(error);

    public void AddErrors(ICollection<T> errors)
    {
        foreach (var error in errors)
            Errors.Add(error);
    }
}