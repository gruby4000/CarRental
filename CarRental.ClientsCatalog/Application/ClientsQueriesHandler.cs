using CarRental.ClientsCatalog.Application.Queries;
using CarRental.ClientsCatalog.Infrastructure;
using CarRental.ClientsCatalog.Model;

namespace CarRental.ClientsCatalog.Application;

public class ClientsQueriesHandler : IClientsQueriesHandler
{
    private readonly ClientsContext _ctx;

    public ClientsQueriesHandler(ClientsContext ctx)
    {
        _ctx = ctx;
    }
    
    public Task<List<Client>> GetClients(ClientsQuery clientsQuery)
    {
        throw new NotImplementedException();
    }
}