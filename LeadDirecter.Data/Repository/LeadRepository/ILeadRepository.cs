using LeadDirecter.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadDirecter.Data.Repository.LeadRepository
{
    public interface ILeadRepository
    {
        Task<Lead> AddLeadAsync(Lead lead, CancellationToken cancellationToken);
        Task<Lead?> GetLeadByIdAsync(Guid leadId, CancellationToken cancellationToken);
        Task<IEnumerable<Lead>> GetLeadsByCampaignIdAsync(Guid campaignId, CancellationToken cancellationToken);
    }
}
