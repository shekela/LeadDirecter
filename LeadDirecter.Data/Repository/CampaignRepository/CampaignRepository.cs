using LeadDirecter.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeadDirecter.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LeadDirecter.Data.Repository.CampaignRepository
{
    public class CampaignRepository : ICampaignRepository
    {
        private readonly DataContext _context;

        public CampaignRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Campaigns.AnyAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<Campaign> AddAsync(Campaign entity, CancellationToken cancellationToken)
        {
            await _context.Campaigns.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<Campaign?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Campaigns
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<Campaign> UpdateAsync(Campaign entity, CancellationToken cancellationToken)
        {
            _context.Campaigns.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _context.Campaigns.FindAsync(id, cancellationToken);
            if (entity == null) return false;

            _context.Campaigns.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<IEnumerable<Campaign>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Campaigns
                .ToListAsync(cancellationToken);
        }
    }
}
