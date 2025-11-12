using LeadDirecter.Data.Context;
using LeadDirecter.Model.Requests;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadDirecter.Data.Repository.LeadValidationsRepository
{
    public class LeadValidationRepository : ILeadValidationRepository
    {
        private readonly DataContext _context;

        public LeadValidationRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> EmailExistsGloballyAsync(LeadRequest lead, CancellationToken cancellationToken)
        {
            return await _context.Leads.AnyAsync(x => x.Email == lead.Email, cancellationToken);
        }

        public async Task<bool> EmailExistsInCampaignAsync(LeadRequest lead, CancellationToken cancellationToken)
        {
            return await _context.Leads.AnyAsync(x =>
                x.Email == lead.Email &&
                x.CampaignId == lead.CampaignId,
                cancellationToken);
        }

        public async Task<bool> EmailAlreadySentToCrmAsync(LeadRequest lead, CancellationToken cancellationToken)
        {
            return await _context.Leads.AnyAsync(x =>
                x.Email == lead.Email &&
                x.DestinationCrmConfigurationId == lead.DestinationCrmConfigurationId &&
                x.IsSentSuccessfully, 
                cancellationToken);
        }

        public async Task<bool> PhoneExistsGloballyAsync(LeadRequest lead, CancellationToken cancellationToken)
        {
            return await _context.Leads.AnyAsync(x =>
                x.Phone == lead.Phone &&
                x.Prefix == lead.Prefix,
                cancellationToken);
        }

        public async Task<bool> PhoneExistsInCampaignAsync(LeadRequest lead, CancellationToken cancellationToken)
        {
            return await _context.Leads.AnyAsync(x =>
                x.Phone == lead.Phone &&
                x.Prefix == lead.Prefix &&
                x.CampaignId == lead.CampaignId,
                cancellationToken);
        }

        public async Task<bool> PhoneAlreadySentToCrmAsync(LeadRequest lead, CancellationToken cancellationToken)
        {
            return await _context.Leads.AnyAsync(x =>
                x.Phone == lead.Phone &&
                x.Prefix == lead.Prefix &&
                x.DestinationCrmConfigurationId == lead.DestinationCrmConfigurationId &&
                x.IsSentSuccessfully,
                cancellationToken);
        }
    }
}
