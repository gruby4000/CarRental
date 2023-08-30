using CarRental.ClientsCatalog.Application.Queries;
using CarRental.ClientsCatalog.Model;

namespace CarRental.ClientsCatalog.Application;

public interface IClientsQueriesHandler
{
    Task<List<Client>> GetClients(ClientsQuery clientsQuery);
}