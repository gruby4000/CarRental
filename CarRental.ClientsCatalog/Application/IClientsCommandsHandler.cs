using CarRental.ClientsCatalog.Application.Commands;
using CarRental.ClientsCatalog.Model;
using MassTransit.Monitoring.Performance;

namespace CarRental.ClientsCatalog.Application;

public interface IClientsCommandsHandler
{
    Task<Client> CreateClientAsync(CreateClient command);
}