using CarRental.BuildingBlocks.DDD;
using CarRental.BuildingBlocks.ServiceIntegration;

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
            if (@event.CompanyTaxId is not null)
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


        if (client.Exist)
        {
            _eventBus.Publish(new ClientChecked() { ClientId = client.Client.Id });
        }
        else
        {
            var createClientCommand = new CreateClient()
            {

            };

            await _commandsHandler.CreateClient(createClientCommand);
            var createdClient = await _queriesHandler.GetIfCompanyClientExistAsync(@event.IdNumber);
            _eventBus.Publish(new ClientChecked() { ClientId = createdCompanyClient.Client.Id });
        }
    }
    private async Task HandleCompanyClient(CarRentOnSideProcessStarted @event)
    {
        var companyClient = await _queriesHandler.GetIfCompanyClientExistAsync(@event.CompanyTaxId);

        if (companyClient.Exist)
        {
            _eventBus.Publish(new ClientChecked() { ClientId = companyClient.Client.Id });
        }
        else
        {
            var createClientCommand = new CreateClient()
            {

            };

            await _commandsHandler.CreateClient(createClientCommand);
            var createdCompanyClient = await _queriesHandler.GetIfCompanyClientExistAsync(@event.CompanyTaxId);
                
            _eventBus.Publish(new ClientChecked() { ClientId = createdCompanyClient.Client.Id });
        }
    }
}