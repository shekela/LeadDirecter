using LeadDirecter.Data.Entities;
using LeadDirecter.Data.Repository.CampaignValidationsRepository;
using LeadDirecter.Data.Repository.LeadValidationsRepository;
using LeadDirecter.Model.Requests;
using LeadDirecter.Model.Responses;
using LeadDirecter.Shared.Enums;

namespace LeadDirecter.Service.Validations.LeadValidationsService
{
    public class LeadValidationService : ILeadValidationService
    {
        private readonly ILeadValidationRepository _leadValidationRepository;
        private readonly ICampaignValidationRepository _campaignValidationRepository;
        public LeadValidationService(ILeadValidationRepository leadValidationRepository, ICampaignValidationRepository campaignValidationRepository)
        {
            _leadValidationRepository = leadValidationRepository;
            _campaignValidationRepository = campaignValidationRepository;
        }

        public async Task<LeadValidationResponse> ValidateAsync(LeadRequest lead, CancellationToken cancellationToken)
        {
            var validations = await _campaignValidationRepository.GetByCampaignAsync(lead.CampaignId, cancellationToken);
            var result = new LeadValidationResponse();

            foreach (var validation in validations.Where(v => v.IsEnabled))
            {
                var passed = await ApplyRuleAsync(lead, validation.ValidationType, cancellationToken);
                if (!passed)
                {
                    result.FailedRules.Add(validation.ValidationType.ToString());
                }
            }

            return result;
        }


        private async Task<bool> ApplyRuleAsync(LeadRequest lead, ValidationType validationType, CancellationToken cancellationToken)
        {
            switch (validationType)
            {
                case ValidationType.EmailExistsGlobally:
                    return !await _leadValidationRepository.EmailExistsGloballyAsync(lead, cancellationToken);

                case ValidationType.EmailExistsInCampaign:
                    return !await _leadValidationRepository.EmailExistsInCampaignAsync(lead, cancellationToken);

                case ValidationType.EmailAlreadySentToCrm:
                    return !await _leadValidationRepository.EmailAlreadySentToCrmAsync(lead, cancellationToken);

                case ValidationType.PhoneExistsGlobally:
                    return !await _leadValidationRepository.PhoneExistsGloballyAsync(lead, cancellationToken);

                case ValidationType.PhoneExistsInCampaign:
                    return !await _leadValidationRepository.PhoneExistsInCampaignAsync(lead, cancellationToken);

                case ValidationType.PhoneAlreadySentToCrm:
                    return !await _leadValidationRepository.PhoneAlreadySentToCrmAsync(lead, cancellationToken);

                default:
                    return true;
            }
        }
    }
}
