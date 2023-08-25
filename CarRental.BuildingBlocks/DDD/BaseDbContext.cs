using CarRental.BuildingBlocks.DDD;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace CarRental.BuildingBlocks;

public class BaseDbContext: DbContext, IUnitOfWork
{
    private readonly DbContextOptions _options;
    private readonly IMediator _mediator;

    public BaseDbContext(DbContextOptions options, IMediator mediator)
    {
        _options = options;
        _mediator = mediator;
    }
    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        await DispatchDomainEventsAsync();
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await DispatchDomainEventsAsync();
        return await base.SaveChangesAsync(cancellationToken);
    }
    
    private async Task DispatchDomainEventsAsync()
    {
        var enumerator = this.ChangeTracker.Entries().GetEnumerator();
        List<DomainEvent> domainEvents = new();
        while(enumerator.MoveNext())
        {
            if(enumerator.Current.Metadata.ClrType.BaseType.Name.Equals(typeof(AggregateRoot).Name))
            {
                var aggregateRoot = (enumerator.Current.Entity as AggregateRoot);
                domainEvents.AddRange(aggregateRoot.Events);
                aggregateRoot.Events.Clear();
            }
        }

        if (domainEvents.Any())
        {
            foreach (DomainEvent @event in domainEvents)
            {
                _mediator.Publish(@event);
            }
        }
            
    }
}
