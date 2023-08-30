using System.Text.Json;
using CarRental.ClientsCatalog.Application.Queries;
using CarRental.ClientsCatalog.Infrastructure;
using CarRental.ClientsCatalog.Model;

namespace CarRental.ClientsCatalog.Application;

public sealed class ClientsCommandsHandler
{
    private readonly ClientsContext _ctx;

    public ClientsCommandsHandler(ClientsContext ctx)
    {
        _ctx = ctx;
    }
    
    public Task<(bool Exist, Client Value)> CheckIfClientExistAsync(string idNumber)
    {
        throw new NotImplementedException();
    }
    
    public Task<(bool Exist, Client Value)> CheckIfCompanyClientExistAsync(string companyTaxId)
    {
        throw new NotImplementedException();
    }
}