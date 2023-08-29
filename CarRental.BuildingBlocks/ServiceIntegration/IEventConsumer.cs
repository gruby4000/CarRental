namespace CarRental.BuildingBlocks.ServiceIntegration;

public interface IEventConsumer<T> where T: IEvent
{
    void StartConsuming();
    void StopConsuming();
}