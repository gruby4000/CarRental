using CarRental.BuildingBlocks.DDD;
using CarRental.BuildingBlocks.ServiceIntegration;
using CarRental.ClientsCatalog.Application.Commands;

namespace CarRental.ClientsCatalog.Application.IntegrationEvents.Handlers;

public class CarRentOnSideProcessStartedHandler
{
    private readonly IClientsQueriesHandler _queriesHandler;
    private readonly IClientsCommandsHandler _commandsHandler;
    private readonly IEventBus _eventBus;

    public CarRentOnSideProcessStartedHandler(IClientsQueriesHandler queriesHandler,
        IClientsCommandsHandler commandsHandler, IEventBus eventBus)
    {
        _queriesHandler = queriesHandler;
        _commandsHandler = commandsHandler;
        _eventBus = eventBus;
    }

    public async Task Handle(CarRentOnSideProcessStarted @event)
    {
        try
        {
            if (@event.IsCompany)
                await HandleCompanyClient(@event);
            await HandleClient(@event);
        }
        catch (Exception ex)
        {
            _eventBus.Publish(new ClientCheckingProcessFailed() { IdNumber = @event.IdNumber, CompanyTaxId = @event.CompanyTaxId});
        }
    }

    private async Task HandleClient(CarRentOnSideProcessStarted @event)
    {
        var client = await _queriesHandler.GetIfClientExistAsync(@event.IdNumber);
        var correlationId = @event.CorrelationId;

        if (client.Exist)
        {
            _eventBus.Publish(new ClientChecked() { ClientId = client.Client.Id, RentNumber = @event.RentNumber, CorrelationId = @correlationId });
        }
        else
        {
            var createClientCommand = new CreateClient()
            {
                FirstName = @event.FirstName,
                LastName = @event.LastName,
                CompanyName = @event.CompanyName,
                CompanyTaxId = @event.CompanyTaxId,
                CorrespondencyAddress = @event.CorrespondencyAddress,
                CompanyAddress = @event.CompanyAddress,
                IdNumber = @event.IdNumber
            };

            await _commandsHandler.CreateClientAsync(createClientCommand);
            var createdClient = await _queriesHandler.GetIfCompanyClientExistAsync(@event.IdNumber);
            _eventBus.Publish(new ClientChecked() { ClientId = createdClient.Client.Id, RentNumber = @event.RentNumber, CorrelationId = correlationId });
        }
    }
    private async Task HandleCompanyClient(CarRentOnSideProcessStarted @event)
    {
        var correlationId = @event.CorrelationId;
        var companyClient = await _queriesHandler.GetIfCompanyClientExistAsync(@event.CompanyTaxId);

        if (companyClient.Exist)
        {
            _eventBus.Publish(new ClientChecked() { ClientId = companyClient.Client.Id, RentNumber = @event.RentNumber, CorrelationId = correlationId});
        }
        else
        {
            var createClientCommand = new CreateClient
            {
                FirstName = @event.FirstName,
                LastName = @event.LastName,
                CorrespondencyAddress = @event.CorrespondencyAddress,
                IdNumber = @event.IdNumber,
                CompanyTaxId = @event.CompanyTaxId,
                CompanyAddress = @event.CompanyAddress,
                CompanyName = @event.CompanyName
            };

            await _commandsHandler.CreateClientAsync(createClientCommand);
            var createdCompanyClient = await _queriesHandler.GetIfCompanyClientExistAsync(@event.CompanyTaxId);
                
            _eventBus.Publish(new ClientChecked() { ClientId = createdCompanyClient.Client.Id, RentNumber = @event.RentNumber, CorrelationId = correlationId });
        }
    }
}