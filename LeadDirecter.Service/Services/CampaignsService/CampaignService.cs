using LeadDirecter.Model.Requests;
using LeadDirecter.Model.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeadDirecter.Data;
using LeadDirecter.Data.Entities;
using LeadDirecter.Data.Repository.CampaignRepository;
using Microsoft.Extensions.Logging;

namespace LeadDirecter.Service.Services.CampaignsService
{
    public class CampaignService : ICampaignService
    {
        private readonly ICampaignRepository _campaignRepository;
        private readonly ILogger<CampaignService> _logger;

        public CampaignService(ICampaignRepository campaignRepository, ILogger<CampaignService> logger)
        {
            _campaignRepository = campaignRepository;
            _logger = logger;
        }

        public async Task<CampaignResponse> CreateCampaignAsync(CampaignRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Creating new campaign with name: {Name}, buyer: {Buyer}, country: {Country}",
                    request?.Name, request?.TrafficBuyerName, request?.TargetCountry);

                if(request == null)
                {
                    _logger.LogWarning("Campaign creation request is null.");
                    throw new ArgumentNullException(nameof(request));
                }

                var entity = new Campaign
                {
                    Name = request.Name,
                    Description = request.Description,
                    TargetCountry = request.TargetCountry,
                    TrafficBuyerName = request.TrafficBuyerName,
                    FunnelName = request.FunnelName,
                    TrafficSourceName = request.TrafficSourceName,
                    CampaignMacros = request.CampaignMacros,
                };

                var created = await _campaignRepository.AddAsync(entity, cancellationToken);
                _logger.LogInformation("Campaign created successfully with ID: {CampaignId}", created.Id);

                return MapEntityToResponse(created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating campaign with payload {@Request}", request);
                throw;
            }
        }

        public async Task<CampaignResponse?> UpdateCampaignAsync(Guid id, CampaignRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Updating campaign with ID: {CampaignId}", id);

                var entity = await _campaignRepository.GetByIdAsync(id, cancellationToken);
                if (entity == null)
                {
                    _logger.LogWarning("Campaign not found for update. ID: {CampaignId}", id);
                    return null;
                }

                entity.Name = request.Name;
                entity.Description = request.Description;
                entity.TargetCountry = request.TargetCountry;
                entity.TrafficBuyerName = request.TrafficBuyerName;
                entity.FunnelName = request.FunnelName;
                entity.StartDate = request.StartDate;
                entity.CampaignMacros = request.CampaignMacros;

                var updated = await _campaignRepository.UpdateAsync(entity, cancellationToken);
                _logger.LogInformation("Campaign updated successfully with ID: {CampaignId}", id);

                return MapEntityToResponse(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating campaign with ID {CampaignId} and payload {@Request}", id, request);
                throw;
            }
        }

        public async Task<bool> DeleteCampaignAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Deleting campaign with ID: {CampaignId}", id);

                var result = await _campaignRepository.DeleteAsync(id, cancellationToken);

                if (result)
                    _logger.LogInformation("Campaign deleted successfully. ID: {CampaignId}", id);
                else
                    _logger.LogWarning("Delete operation failed or campaign not found. ID: {CampaignId}", id);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting campaign with ID {CampaignId}", id);
                throw;
            }
        }

        public async Task<CampaignResponse?> GetCampaignByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Fetching campaign with ID: {CampaignId}", id);

                var entity = await _campaignRepository.GetByIdAsync(id, cancellationToken);
                if (entity == null)
                {
                    _logger.LogWarning("No campaign found with ID: {CampaignId}", id);
                    return null;
                }

                _logger.LogInformation("Campaign retrieved successfully with ID: {CampaignId}", id);
                return MapEntityToResponse(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching campaign with ID {CampaignId}", id);
                throw;
            }
        }

        private CampaignResponse MapEntityToResponse(Campaign entity)
        {
            try
            {
                if (entity == null)
                {
                    _logger.LogWarning("Attempted to map null Campaign entity to response.");
                    throw new ArgumentNullException(nameof(entity));
                }

                return new CampaignResponse
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Description,
                    TargetCountry = entity.TargetCountry,
                    TrafficBuyerName = entity.TrafficBuyerName,
                    CampaignMacros = entity.CampaignMacros,
                    FunnelName = entity.FunnelName,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error mapping Campaign entity to response. Entity: {@Entity}", entity);
                throw;
            }
        }
    }
}