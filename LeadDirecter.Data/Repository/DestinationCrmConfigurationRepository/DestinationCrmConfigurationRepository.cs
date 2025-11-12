using LeadDirecter.Data.Context;
using LeadDirecter.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LeadDirecter.Data.Repository.DestinationCrmConfigurationRepository
{
    public class DestinationCrmConfigurationRepository : IDestinationCrmConfigurationRepository
    {
        private readonly DataContext _context;

        public DestinationCrmConfigurationRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.DestinationCrmConfigurations.AnyAsync(c => c.Id == id, cancellationToken);
        }
        public async Task<DestinationCrmConfiguration?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.DestinationCrmConfigurations
                                 .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<DestinationCrmConfiguration>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.DestinationCrmConfigurations
                                 .ToListAsync(cancellationToken);
        }

        public async Task<DestinationCrmConfiguration?> AddWithCampaignAsync(
            DestinationCrmConfiguration entity,
            Guid? campaignId,
            CancellationToken cancellationToken)
        {
            if (campaignId.HasValue)
            {
                var campaign = await _context.Campaigns.FindAsync(campaignId.Value, cancellationToken);
                if (campaign == null)
                    return null;

                entity.CampaignId = campaignId;
                entity.Campaign = campaign;
            }

            await _context.DestinationCrmConfigurations.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task<DestinationCrmConfiguration> UpdateAsync(DestinationCrmConfiguration entity, CancellationToken cancellationToken)
        {
            _context.DestinationCrmConfigurations.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _context.DestinationCrmConfigurations.FindAsync(id, cancellationToken);
            if (entity == null) return false;

            _context.DestinationCrmConfigurations.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
