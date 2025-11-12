using LeadDirecter.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadDirecter.Data.Repository.DestinationCrmConfigurationRepository
{
    public interface IDestinationCrmConfigurationRepository
    {
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
        Task<DestinationCrmConfiguration?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<DestinationCrmConfiguration>> GetAllAsync(CancellationToken cancellationToken);
        Task<DestinationCrmConfiguration?> AddWithCampaignAsync(DestinationCrmConfiguration entity, Guid? campaignId, CancellationToken cancellationToken);
        Task<DestinationCrmConfiguration> UpdateAsync(DestinationCrmConfiguration entity, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
