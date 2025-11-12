using LeadDirecter.Data.Entities;
using LeadDirecter.Model.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadDirecter.Data.Repository.CampaignValidationsRepository
{
    public interface ICampaignValidationRepository
    {
        Task<IEnumerable<CampaignValidation>> GetByCampaignAsync(Guid campaignId, CancellationToken cancellationToken);
        Task<CampaignValidation> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task AddAsync(CampaignValidation validation, CancellationToken cancellationToken);
        Task UpdateAsync(CampaignValidation validation);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }

}
