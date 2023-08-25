using CarRental.BuildingBlocks.DDD;
using CarRental.BuildingBlocks.ErrorNotification;
using CarRental.BuildingBlocks.Validation;
using MediatR;

namespace CarRental.HR.Model.Manager;

public record Employee: IValidatable, INotificationProducer<DomainError>
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public Notification<DomainError> Notification { get; } = new();
    
    public bool Validate()
    {
         if(
            string.IsNullOrEmpty(FirstName)
         || string.IsNullOrEmpty(LastName))
            Notification.AddError(new () {Message = "FirstName and LastName must be provided.", Source = nameof(Employee)});

         return !Notification.HasErrors;
    }
}
