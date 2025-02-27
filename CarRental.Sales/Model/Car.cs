﻿using CarRental.BuildingBlocks.DDD;
using CarRental.BuildingBlocks.ErrorNotification;
using CarRental.BuildingBlocks.Validation;

namespace CarRental.Sales.Model;

public record Car : IValidatable, INotificationProducer<DomainError>
{
    public required int CarNumber { get; init; }
    public Notification<DomainError> Notification { get; } = new();
    
    public bool Validate()
    {
        if(CarNumber >= 100000)
            Notification.AddError(new() {Message = "Car number has wrong format", Source = nameof(Car)});

        return !Notification.HasErrors;
    }
}