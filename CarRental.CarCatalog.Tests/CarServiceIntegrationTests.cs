using CarRental.CarCatalog.Application;
using CarRental.CarCatalog.Model;
using NSubstitute;
using DriveType = CarRental.CarCatalog.Model.DriveType;

namespace CarRental.CarCatalog.Tests;

public class CarServiceIntegrationTests: Common
{
    [SetUp]
    public void Setup()
    {
        ServiceProviderConfig();
        _ctx.Brands.Add(new Brand() { Producer = "Volkswagen" });
        _ctx.Brands.Add(new Brand() { Producer = "Mazda" });
        _ctx.Brands.Add(new Brand() { Producer = "Ferrari" });
        _ctx.SaveChanges();
    }

    [Test]
    public void Add()
    {
        using var scope = _serviceScopeFactoryMock.CreateScope();

        var carService = scope.ServiceProvider.GetService(typeof(CarService)) as CarService;

        Car newCar = new()
        {
            Brand = _ctx.Brands.First(),
            Details = new CarDetails()
            {
                Color = "Red",
                DriveType = DriveType.AWD,
                EngineType = EngineType.ElectricTwinMotor,
                EngineCapacity = 0.00f
            },
            IsAvailable = true,
            Model = "ID3",
            CarNumber = 123456
        };

        carService.Add(newCar);
        carService.SaveChanges();
        Assert.IsTrue(_ctx.Cars.FirstOrDefault() is not null);
    }
    
    [Test]
    public void Update()
    {
        using var scope = _serviceScopeFactoryMock.CreateScope();

        var carService = scope.ServiceProvider.GetService(typeof(CarService)) as CarService;

        Car newCar = new()
        {
            Brand = _ctx.Brands.First(),
            Details = new CarDetails()
            {
                Color = "Red",
                DriveType = DriveType.AWD,
                EngineType = EngineType.ElectricTwinMotor,
                EngineCapacity = 0.00f
            },
            IsAvailable = true,
            Model = "ID3",
            CarNumber = 123456
        };

        carService.Add(newCar);
        carService.SaveChanges();
        var car = _ctx.Cars.First();
        car.IsAvailable = false;
        carService.Update(car);
        _ctx.SaveChanges();
        Assert.IsFalse(_ctx.Cars.First().IsAvailable);
    }
    
    [Test]
    public void Delete()
    {
        using var scope = _serviceScopeFactoryMock.CreateScope();

        var carService = scope.ServiceProvider.GetService(typeof(CarService)) as CarService;

        Car newCar = new()
        {
            Brand = _ctx.Brands.First(),
            Details = new CarDetails()
            {
                Color = "Red",
                DriveType = DriveType.AWD,
                EngineType = EngineType.ElectricTwinMotor,
                EngineCapacity = 0.00f
            },
            IsAvailable = true,
            Model = "ID3",
            CarNumber = 123456
        };

        carService.Add(newCar);
        carService.SaveChanges();
        var car = _ctx.Cars.First();
        car.IsAvailable = false;
        carService.Delete(car);
        _ctx.SaveChanges();
        Assert.IsEmpty(_ctx.Cars.ToList());
    }
    
    [Test]
    public void SoftDelete()
    {
        using var scope = _serviceScopeFactoryMock.CreateScope();

        var carService = scope.ServiceProvider.GetService(typeof(CarService)) as CarService;

        Car newCar = new()
        {
            Brand = _ctx.Brands.First(),
            Details = new CarDetails()
            {
                Color = "Red",
                DriveType = DriveType.AWD,
                EngineType = EngineType.ElectricTwinMotor,
                EngineCapacity = 0.00f
            },
            IsAvailable = true,
            Model = "ID3",
            CarNumber = 123456
        };

        carService.Add(newCar);
        carService.SaveChanges();
        var car = _ctx.Cars.First();
        car.IsAvailable = false;
        carService.SoftDelete(car);
        _ctx.SaveChanges();
        Assert.IsNotEmpty(_ctx.Cars.ToList());
        Assert.IsFalse(_ctx.Cars.First().IsAvailable);
    }
    
    [Test]
    public async Task Get()
    {
        using var scope = _serviceScopeFactoryMock.CreateScope();

        var carService = scope.ServiceProvider.GetService(typeof(CarService)) as CarService;

        Car newCar = new()
        {
            Brand = _ctx.Brands.First(),
            Details = new CarDetails()
            {
                Color = "Red",
                DriveType = DriveType.AWD,
                EngineType = EngineType.ElectricTwinMotor,
                EngineCapacity = 0.00f
            },
            IsAvailable = true,
            Model = "ID3",
            CarNumber = 123456
        };

        carService.Add(newCar);
        carService.SaveChanges();
        var carFromDb = await carService.Get(newCar.Id);
        Assert.IsNotNull(carFromDb);
        Assert.IsTrue(carFromDb.Equals(newCar));
    }
}