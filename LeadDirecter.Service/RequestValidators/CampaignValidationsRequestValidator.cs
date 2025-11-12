using FluentValidation;
using LeadDirecter.Data.Repository.CampaignRepository;
using LeadDirecter.Model.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadDirecter.Service.RequestValidators
{
    public class CampaignValidationsRequestValidator : AbstractValidator<CampaignValidationsRequest>
    {
        public CampaignValidationsRequestValidator(
        ICampaignRepository campaignRepository,
        CancellationToken cancellationToken = default
        )
        {
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

            RuleFor(x => x.ValidationType)
                .NotNull().WithMessage("Validations list cannot be null.");
        }
    }
}
