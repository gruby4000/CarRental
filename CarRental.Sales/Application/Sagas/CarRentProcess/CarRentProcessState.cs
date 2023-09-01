namespace CarRental.Sales.Application.Sagas.CarRentProcess;

public sealed class CarRentProcessState
{
    public readonly string[] _orderedStates = 
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

    public const string CHECKING_THE_CLIENT = "CheckingTheClient";
    public const string CLIENT_CHECKED = "ClientChecked";
    public const string SIGNING_AGREEMENT = "SigningAgreement";
    public const string AGREEMENT_SIGNED = "AgreementSigned";
    public const string GENERATING_INVOICE = "GeneratingInvoice";
    public const string INVOICE_GENERATED = "InvoiceGenerated";
    public const string CHANGING_CAR_PLACE = "ChangingCarPlace";
    public const string CAR_PLACE_CHANGED = "CarPlaceChanged";
    public const string CHANGING_CAR_AVAILABILITY = "ChangingCarAvailability";
    public const string CAR_AVAILABILITY_CHANGED = "CarAvailabilityChanged";
    public const string RENT_PROCESS_FINISHED = "RentProcessFinished";

    public required string RentNumber { get; init; }
    public required string CurrentState { get; set; }
    public required DateTime ChangedDate { get; set; } 
    public required Guid CorrelationId { get; init; }
}