using LeadDirecter.Data.Context;
using LeadDirecter.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadDirecter.Data.Repository.LeadRepository
{
    public class LeadRepository : ILeadRepository
    {
        private readonly DataContext _context;

        public LeadRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Lead> AddLeadAsync(Lead lead, CancellationToken cancellationToken)
        {
            if (lead == null) throw new ArgumentNullException(nameof(lead));

            await _context.Leads.AddAsync(lead, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return lead;
        }

        public async Task<Lead?> GetLeadByIdAsync(Guid leadId, CancellationToken cancellationToken)
        {
            return await _context.Leads
                .FirstOrDefaultAsync(l => l.Id == leadId, cancellationToken);
        }

        public async Task<IEnumerable<Lead>> GetLeadsByCampaignIdAsync(Guid campaignId, CancellationToken cancellationToken)
        {
            return await _context.Leads
                .Where(l => l.CampaignId == campaignId)
                .ToListAsync(cancellationToken);
        }
    }
}
