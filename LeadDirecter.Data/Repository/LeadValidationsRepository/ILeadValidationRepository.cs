using LeadDirecter.Model.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadDirecter.Data.Repository.LeadValidationsRepository
{
    public interface ILeadValidationRepository
    {
        Task<bool> EmailExistsGloballyAsync(LeadRequest lead, CancellationToken cancellationToken);
        Task<bool> EmailExistsInCampaignAsync(LeadRequest lead, CancellationToken cancellationToken);
        Task<bool> EmailAlreadySentToCrmAsync(LeadRequest lead, CancellationToken cancellationToken);
        Task<bool> PhoneExistsGloballyAsync(LeadRequest lead, CancellationToken cancellationToken);
        Task<bool> PhoneExistsInCampaignAsync(LeadRequest lead, CancellationToken cancellationToken);
        Task<bool> PhoneAlreadySentToCrmAsync(LeadRequest lead, CancellationToken cancellationToken);
    }
}
