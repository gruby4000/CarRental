namespace CarRental.Sales.Application.Sagas.CarRentProcess;

public sealed class CarRentProcessState
{
    public static readonly string[] AVAILABLE_STATES = new[]
    {
        "CheckingTheClient",
        "ClientChecked",
        "SigningAgreement",
        "AgreementSigned",
        "GeneratingInvoice",
        "InvoiceGenerated",
        "ChangingCarPlace",
        "CarPlaceChanged",
        "ChangingCarAvailability",
        "CarAvailabilityChanged",
        "RentProcessFinished"
    };
    public required string RentNumber { get; init; }
    public required string CurrentState { get; init; }
    public required DateTime ChangedDate { get; set; } 
}