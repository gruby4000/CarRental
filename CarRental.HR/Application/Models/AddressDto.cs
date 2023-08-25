using CarRental.HR.Model;

namespace CarRental.HR.Application.Models;

public record AddressDto()
{
    public required string Street {get; init;}
    public required string HouseNumber {get; init;}
    public required string ZipCode {get; init;}
    public required string City {get; init;}
    
    public static explicit operator AddressDto(Address a)
    {
        return new AddressDto()
        {
            Street = a.Street,
            HouseNumber = a.HouseNumber,
            ZipCode = a.ZipCode,
            City = a.City
        };
    }
};