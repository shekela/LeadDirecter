using Azure.Core;
using LeadDirecter.Data.Entities;
using LeadDirecter.Data.Repository.CampaignValidationsRepository;
using LeadDirecter.Model.Requests;
using LeadDirecter.Model.Responses;
using LeadDirecter.Service.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadDirecter.Service.Validations.CampaignValidationsService
{
    public class CampaignValidationService : ICampaignValidationService
    {
        private readonly ICampaignValidationRepository _repository;

        public CampaignValidationService(ICampaignValidationRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CampaignValidationResponse>> GetByCampaignAsync(Guid campaignId, CancellationToken cancellationToken)
        {
            var entities = await _repository.GetByCampaignAsync(campaignId, cancellationToken);
            return entities.ToResponseList();
        }

        public async Task<CampaignValidationResponse> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            return entity?.ToResponse();
        }

        public async Task<CampaignValidationResponse> AddAsync(CampaignValidationsRequest request, CancellationToken cancellationToken)
        {
            var entity = request.ToEntity();
            await _repository.AddAsync(entity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            return entity.ToResponse();
        }

        public async Task<CampaignValidationResponse> UpdateAsync(CampaignValidationsRequest request, CancellationToken cancellationToken)
        {
            var entity = request.ToEntity();
            await _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync(cancellationToken);
            return entity.ToResponse();
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            await _repository.DeleteAsync(id, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
        }
    }
}
