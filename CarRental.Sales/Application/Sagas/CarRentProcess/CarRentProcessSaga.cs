using CarRental.Sales.Infrastructure;

namespace CarRental.Sales.Application.Sagas.CarRentProcess;

public sealed class CarRentProcessSaga
{
    private readonly SalesContext _ctx;

    public CarRentProcessSaga(SalesContext ctx)
    {
        _ctx = ctx;
    }

    public void Start(string rentNumber)
    {
        _ctx.CarRentProcessStates.Add(new CarRentProcessState()
        {

        });
    }
}