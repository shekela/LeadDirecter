using FluentValidation;
using LeadDirecter.Data.Repository.CampaignRepository;
using LeadDirecter.Model.Requests;

namespace LeadDirecter.Service.RequestValidators
{
    public class DestinationCrmConfigurationRequestValidator : AbstractValidator<DestinationCrmConfigurationRequest>
    {
        public DestinationCrmConfigurationRequestValidator(
            ICampaignRepository campaignRepository,
            CancellationToken cancellationToken = default
        )
        {
            // Required core fields
            RuleFor(x => x.CrmName)
                .NotEmpty().WithMessage("CRM name is required.");

            RuleFor(x => x.ResponseMappingProperty)
                .NotEmpty().WithMessage("Response mapping property is required.");

            // Lead Registration
            RuleFor(x => x.LeadRegistrationEndpoint)
                .NotEmpty().WithMessage("Lead registration endpoint is required.")
                .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .WithMessage("Lead registration endpoint must be a valid URL.");

            RuleFor(x => x.LeadRegistrationMethod)
                .NotEmpty().WithMessage("Lead registration method is required.")
                .Must(m => new[] { "GET", "POST", "PUT", "DELETE" }.Contains(m.ToUpperInvariant()))
                .WithMessage("Lead registration method must be one of GET, POST, PUT, DELETE.");

            RuleFor(x => x.LeadRegistrationContentType)
                .IsInEnum()
                .WithMessage("LeadRegistrationContentType must be a valid ContentType enum value.");

            // Leads Retrieval
            RuleFor(x => x.LeadsRetrievalEndpoint)
                .NotEmpty().WithMessage("Leads retrieval endpoint is required.")
                .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .WithMessage("Leads retrieval endpoint must be a valid URL.");

            RuleFor(x => x.LeadsRetrievalMethod)
                .NotEmpty().WithMessage("Leads retrieval method is required.")
                .Must(m => new[] { "GET", "POST", "PUT", "PATCH", "DELETE" }.Contains(m.ToUpperInvariant()))
                .WithMessage("Leads retrieval method must be one of GET, POST, PUT, DELETE.");

            RuleFor(x => x.LeadRetrievalContentType)
                .IsInEnum()
                .WithMessage("LeadRegistrationContentType must be a valid ContentType enum value.");


            // Optional campaign
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

        }
    }
}
