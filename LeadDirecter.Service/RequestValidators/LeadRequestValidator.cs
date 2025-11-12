using FluentValidation;
using LeadDirecter.Data.Entities;
using LeadDirecter.Data.Repository.CampaignRepository;
using LeadDirecter.Data.Repository.DestinationCrmConfigurationRepository;
using LeadDirecter.Model.Requests;
using LeadDirecter.Service.Validations.LeadValidationsService;

namespace LeadDirecter.Service.RequestValidators
{
    public class LeadRequestValidator : AbstractValidator<LeadRequest>
    {
        public LeadRequestValidator(
            ICampaignRepository campaignRepository,
            IDestinationCrmConfigurationRepository crmConfigRepository,
            ILeadValidationService leadValidationService,
            CancellationToken cancellationToken = default)
        {
            RuleFor(x => x.FirstName)
                .NotEmpty();

            RuleFor(x => x.LastName)
                .NotEmpty();

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Prefix)
                .NotEmpty();

            RuleFor(x => x.CampaignId)
                .Must(id => id != Guid.Empty)
                .WithMessage("If provided, CampaignId must be a valid GUID.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.CampaignId)
                        .MustAsync(async (id, cancellation) =>
                        {
                            if (id == Guid.Empty)
                                return true; // skip existence check if not provided
                            return await campaignRepository.ExistsAsync(id, cancellationToken);
                        })
                        .WithMessage("Specified campaign does not exist.");
                });

            RuleFor(x => x.DestinationCrmConfigurationId)
                .Must(id => id != Guid.Empty)
                .WithMessage("If provided, DestinationCrmConfigurationId must be a valid GUID.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.DestinationCrmConfigurationId)
                        .MustAsync(async (id, cancellation) =>
                        {
                            if (id == Guid.Empty)
                                return true;
                            return await crmConfigRepository.ExistsAsync(id, cancellationToken);
                        })
                        .WithMessage("Specified CRM configuration does not exist.");
                });

            RuleFor(x => x)
                .MustAsync(async (lead, cancellation) =>
                {
                    var request = new LeadRequest
                    {
                        Email = lead.Email,
                        Phone = lead.Phone,
                        Prefix = lead.Prefix,
                        CampaignId = lead.CampaignId,
                        DestinationCrmConfigurationId = lead.DestinationCrmConfigurationId
                    };

                    var result = await leadValidationService.ValidateAsync(request, cancellationToken);
                    return result.IsValid;
                })
                .WithMessage((lead, value) =>
                {
                    var failed = leadValidationService.ValidateAsync(new LeadRequest
                    {
                        Email = lead.Email,
                        Phone = lead.Phone,
                        Prefix = lead.Prefix,
                        CampaignId = lead.CampaignId,
                        DestinationCrmConfigurationId = lead.DestinationCrmConfigurationId
                    }, cancellationToken).Result;

                    return failed.FailedRules.Count > 0
                        ? $"Lead violates rules: {string.Join(", ", failed.FailedRules)}"
                        : "Lead violates one or more campaign validation rules.";
                });


        }
    }

}
