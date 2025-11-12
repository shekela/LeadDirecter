using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeadDirecter.Data.Entities;

namespace LeadDirecter.Data.Repository.CampaignRepository
{
    public interface ICampaignRepository
    {
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
        Task<Campaign> AddAsync(Campaign entity, CancellationToken cancellationToken);
        Task<Campaign?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Campaign> UpdateAsync(Campaign entity, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<Campaign>> GetAllAsync(CancellationToken cancellationToken);
    }
}
