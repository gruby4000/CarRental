using CarRental.ClientsCatalog.Application.Queries;
using CarRental.ClientsCatalog.Infrastructure;
using CarRental.ClientsCatalog.Model;
using Microsoft.EntityFrameworkCore;

namespace CarRental.ClientsCatalog.Application;

public class ClientsQueriesHandler : IClientsQueriesHandler
{
    private readonly ClientsContext _ctx;

    public ClientsQueriesHandler(ClientsContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<(bool Exist, Client Client)> GetIfClientExistAsync(string idNumber)
    {
        var client = await _ctx.Clients.AsNoTracking().SingleOrDefaultAsync(x => x.IdNumber.Equals(idNumber));

        return (client is not null, client!);
    }
    
    public async Task<(bool Exist, Client Client)> GetIfCompanyClientExistAsync(string companyTaxId)
    {
        var client = await _ctx.Clients.AsNoTracking().SingleOrDefaultAsync(x => x.CompanyTaxId.Equals(companyTaxId));

        return (client is not null, client!);
    }
    
    public Task<List<Client>> GetClients(ClientsQuery clientsQuery)
    {
        throw new NotImplementedException();
    }
}