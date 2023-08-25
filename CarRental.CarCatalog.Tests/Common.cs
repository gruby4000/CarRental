using CarRental.CarCatalog.Application;
using CarRental.CarCatalog.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace CarRental.CarCatalog.Tests;

public class Common
{
    protected CarCatalogContext _ctx;
    protected IServiceScopeFactory _serviceScopeFactoryMock;
    
    protected void ServiceProviderConfig()
    {
        var serviceProvider = new ServiceCollection()
            .AddMediatR(config => config.RegisterServicesFromAssembly(typeof(CarCatalogContext).Assembly))
            .AddTransient<BrandService>()
            .AddTransient<CarService>()
            .AddDbContext<CarCatalogContext>()
            .BuildServiceProvider();
        
        var serviceScopeMock = Substitute.For<IServiceScope>();
        serviceScopeMock.ServiceProvider.Returns(serviceProvider);
        
        _serviceScopeFactoryMock = Substitute.For<IServiceScopeFactory>();
        _serviceScopeFactoryMock.CreateScope().Returns(serviceScopeMock);
        
        _serviceScopeFactoryMock.CreateScope();
        
        _ctx = serviceProvider.GetService(typeof(CarCatalogContext)) as CarCatalogContext;
        
        _ctx.CarDetails.RemoveRange(_ctx.CarDetails);
        _ctx.Cars.RemoveRange(_ctx.Cars);
        _ctx.Brands.RemoveRange(_ctx.Brands);
        _ctx.SaveChanges();
    }
}