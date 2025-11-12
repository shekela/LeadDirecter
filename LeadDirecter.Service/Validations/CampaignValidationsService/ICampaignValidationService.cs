using LeadDirecter.Data.Entities;
using LeadDirecter.Model.Requests;
using LeadDirecter.Model.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadDirecter.Service.Validations.CampaignValidationsService
{
    public interface ICampaignValidationService
    {
        Task<IEnumerable<CampaignValidationResponse>> GetByCampaignAsync(Guid campaignId, CancellationToken cancellationToken);
        Task<CampaignValidationResponse> GetAsync(Guid id, CancellationToken cancellationToken);
        Task<CampaignValidationResponse> AddAsync(CampaignValidationsRequest validation, CancellationToken cancellationToken);
        Task<CampaignValidationResponse> UpdateAsync(CampaignValidationsRequest validation, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
