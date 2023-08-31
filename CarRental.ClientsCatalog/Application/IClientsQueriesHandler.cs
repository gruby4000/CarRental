using CarRental.ClientsCatalog.Application.Queries;
using CarRental.ClientsCatalog.Model;

namespace CarRental.ClientsCatalog.Application;

public interface IClientsQueriesHandler
{
    Task<(bool Exist, Client Client)> GetIfCompanyClientExistAsync(string companyTaxId);
    Task<(bool Exist, Client Client)> GetIfClientExistAsync(string idNumber);
    
    Task<List<Client>> GetClients(ClientsQuery clientsQuery);
}