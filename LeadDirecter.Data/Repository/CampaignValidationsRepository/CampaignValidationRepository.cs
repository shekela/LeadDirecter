using LeadDirecter.Data.Context;
using LeadDirecter.Data.Entities;
using LeadDirecter.Model.Requests;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadDirecter.Data.Repository.CampaignValidationsRepository
{
    public class CampaignValidationRepository : ICampaignValidationRepository
    {
        private readonly DataContext _context;

        public CampaignValidationRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CampaignValidation>> GetByCampaignAsync(Guid campaignId, CancellationToken cancellationToken)
        {
            return await _context.CampaignValidations
                .Where(v => v.CampaignId == campaignId)
                .ToListAsync(cancellationToken);
        }

        public async Task<CampaignValidation?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.CampaignValidations
                .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
        }
        public async Task AddAsync(CampaignValidation validation, CancellationToken cancellationToken)
        {
            await _context.CampaignValidations.AddAsync(validation, cancellationToken);
        }

        public Task UpdateAsync(CampaignValidation validation)
        {
            _context.CampaignValidations.Update(validation);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await GetByIdAsync(id, cancellationToken);
            if (entity != null)
                _context.CampaignValidations.Remove(entity);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken) => await _context.SaveChangesAsync(cancellationToken);
    }
}
