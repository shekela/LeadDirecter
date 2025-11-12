using FluentValidation;
using LeadDirecter.Model.Requests;

namespace LeadDirecter.Service.RequestValidators
{
    public class CampaignRequestValidator : AbstractValidator<CampaignRequest>
    {
        public CampaignRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.");

            RuleFor(x => x.TargetCountry)
                .NotEmpty().WithMessage("TargetCountry is required.");

            RuleFor(x => x.TrafficBuyerName)
                .NotEmpty().WithMessage("TrafficBuyerName is required.");

            RuleFor(x => x.TrafficSourceName)
                .NotEmpty().WithMessage("TrafficSourceName is required.");

            RuleFor(x => x.FunnelName)
                .NotEmpty().WithMessage("FunnelName is required.");

            RuleFor(x => x.CampaignMacros)
                .NotNull().WithMessage("CampaignMacros cannot be null.")
                .Must(list => list.Count > 0).WithMessage("At least one CampaignMacro is required.");
        }
    }
}
