using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using CarRental.BuildingBlocks.DDD;
using CarRental.BuildingBlocks.ErrorNotification;
using CarRental.BuildingBlocks.Validation;
using CarRental.HR.Events.AgreementCatalog;

namespace CarRental.HR.Model.AgreementCatalog;

public sealed class AgreementCatalog : AggregateRoot
{
    public required int Year {get; init;}
    public required List<Agreement> Agreements { get; init; } = new();
    private readonly Regex takeAgreementNumber = new("[0-9]{6}", RegexOptions.Compiled);
    private AgreementCatalog(): base() {}
    public AgreementCatalog(List<Agreement> agreementsFromThisYear): base()
    {
        Agreements = agreementsFromThisYear;
    }

    public Agreement SignNewAgreementWithEmployee(Agreement agreement) {
        
        var existingAgreement = Agreements.FirstOrDefault(x => x.FirstName.Equals(agreement.FirstName) && x.LastName.Equals(agreement.LastName));
        
        if(existingAgreement is not null && existingAgreement.EndDate <= agreement.SignDate) 
        {
            Notification.AddError(new() { Message ="There is another agreement with this person", Source = nameof(AgreementCatalog)});
        }
        
        agreement.AgreementNumber = GenerateNewAgreementNumber();

        if(!agreement.Validate()) {
            Notification.AddErrors(agreement.Notification.Errors);
        }        
        
        if (Notification.HasErrors) throw new DomainException(Notification);
        
        Agreements.Add(agreement);
        
        Events.Add(new AgreementSigned()
        {
            FirstName = agreement.FirstName,
            LastName = agreement.LastName,
            Address = agreement.Address,
            Salary = agreement.Salary,
            SignDate = agreement.SignDate
        });
    
        return agreement;
    }

    public override bool Validate()
    {
        return !Notification.HasErrors && Agreements.All(x => x.Validate());
    }

    private string GenerateNewAgreementNumber() 
    {
        if(Agreements.Any()) {
            var lastNumber = int.Parse(takeAgreementNumber.Match(Agreements.OrderByDescending(x => x.AgreementNumber).First().AgreementNumber).Value);

            return $"AG/{DateTime.Now.Year}/{lastNumber+1}";
        }
        else return $"AG/{DateTime.Now.Year}/000001";
    }
}
