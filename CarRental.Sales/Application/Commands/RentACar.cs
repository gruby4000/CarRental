using CarRental.HR.Model;

namespace CarRental.Sales.Application.Commands;

public record RentACar(
    int CarNumber,
    string FirstName,
    string LastName,
    Address CorrespondencyAddress,
    string IdNumber,
    string? CompanyName,
    Address? CompanyAddress,
    string? CompanyTaxId,
    DateTime Start,
    DateTime End,
    string RentalIdentifier,
    string RentalEmployeeFirstName,
    string RentalEmployeeLastName
    );