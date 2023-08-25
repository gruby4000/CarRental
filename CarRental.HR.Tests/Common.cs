using CarRental.HR.Application;
using CarRental.HR.Infrastructure;
using CarRental.HR.Model.AgreementCatalog;
using CarRental.HR.Model.Employee;
using CarRental.HR.Model.Manager;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.Extensions;

namespace CarRental.HR.Tests;

public abstract class Common
{
    protected HRContext _ctx;
    protected IServiceScopeFactory _serviceScopeFactoryMock;
    
    protected void ServiceProviderConfig()
    {
        var serviceProvider = new ServiceCollection()
            .AddMediatR(config => config.RegisterServicesFromAssembly(typeof(HRContext).Assembly))
            .AddTransient<IEmployeeRepository, EmployeeRepository>()
            .AddTransient<IManagerRepository, ManagerRepository>()
            .AddTransient<IAgreementCatalogRepository, AgreementCatalogRepository>()
            .AddTransient<EmployeesManagementCommandsHandler>()
            .AddTransient<EmployeesManagementQueriesHandler>()
            .AddDbContext<HRContext>()
            .BuildServiceProvider();
        
        var serviceScopeMock = Substitute.For<IServiceScope>();
        serviceScopeMock.ServiceProvider.Returns(serviceProvider);
        
        _serviceScopeFactoryMock = Substitute.For<IServiceScopeFactory>();
        _serviceScopeFactoryMock.CreateScope().Returns(serviceScopeMock);
        
        _ctx = serviceProvider.GetService(typeof(HRContext)) as HRContext;
        
        _ctx.AgreementCatalogs.Add(new AgreementCatalog(new List<Agreement>())
            { Agreements = new(), Year = 2023 });
        _ctx.RemoveRange(_ctx.Managers);
        _ctx.RemoveRange(_ctx.Employees);
        _ctx.RemoveRange(_ctx.Agreements);
        _ctx.RemoveRange(_ctx.AgreementCatalogs);
        _ctx.RemoveRange(_ctx.VacationRequests);
        _ctx.RemoveRange(_ctx.Addresses);

        _ctx.SaveChanges();
    }
}