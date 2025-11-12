using LeadDirecter.Data.Entities;
using LeadDirecter.Data.Repository.CampaignRepository;
using LeadDirecter.Data.Repository.DestinationCrmConfigurationRepository;
using LeadDirecter.Model.Requests;
using LeadDirecter.Model.Responses;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeadDirecter.Service.Services.DestinationCrmConfigurationService
{
    public class DestinationCrmConfigurationService : IDestinationCrmConfigurationService
    {
        private readonly IDestinationCrmConfigurationRepository _repository;
        private readonly ICampaignRepository _campaignRepository;
        private readonly ILogger<DestinationCrmConfigurationService> _logger;

        public DestinationCrmConfigurationService(
            IDestinationCrmConfigurationRepository repository,
            ICampaignRepository campaignRepository,
            ILogger<DestinationCrmConfigurationService> logger)
        {
            _repository = repository;
            _campaignRepository = campaignRepository;
            _logger = logger;
        }

        public async Task<DestinationCrmConfigurationResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Fetching Destination CRM Configuration with ID: {ConfigId}", id);

                var entity = await _repository.GetByIdAsync(id, cancellationToken);
                if (entity == null)
                {
                    _logger.LogWarning("No Destination CRM Configuration found with ID: {ConfigId}", id);
                    return null;
                }

                _logger.LogInformation("Destination CRM Configuration retrieved successfully with ID: {ConfigId}", id);
                return MapEntityToResponse(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Destination CRM Configuration with ID: {ConfigId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<DestinationCrmConfigurationResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Fetching all Destination CRM Configurations");

                var entities = await _repository.GetAllAsync(cancellationToken);

                _logger.LogInformation("Retrieved {Count} Destination CRM Configurations", entities.Count());
                return entities.Select(MapEntityToResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all Destination CRM Configurations");
                throw;
            }
        }

        public async Task<DestinationCrmConfigurationResponse?> CreateAsync(DestinationCrmConfigurationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Creating Destination CRM Configuration for CRM: {CrmName}, CampaignId: {CampaignId}",
                    request?.CrmName, request?.CampaignId);

                var entity = MapRequestToEntity(request, new DestinationCrmConfiguration { Id = Guid.NewGuid() });

                var createdEntity = await _repository.AddWithCampaignAsync(entity, request.CampaignId, cancellationToken);

                if (createdEntity == null)
                {
                    _logger.LogWarning("Failed to create Destination CRM Configuration for CampaignId: {CampaignId}", request.CampaignId);
                    return null;
                }

                _logger.LogInformation("Destination CRM Configuration created successfully with ID: {ConfigId}", createdEntity.Id);
                return MapEntityToResponse(createdEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Destination CRM Configuration with payload {@Request}", request);
                throw;
            }
        }

        public async Task<DestinationCrmConfigurationResponse?> UpdateAsync(Guid id, DestinationCrmConfigurationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Updating Destination CRM Configuration with ID: {ConfigId}", id);

                var entity = await _repository.GetByIdAsync(id, cancellationToken);
                if (entity == null)
                {
                    _logger.LogWarning("Destination CRM Configuration not found for update. ID: {ConfigId}", id);
                    return null;
                }

                MapRequestToEntity(request, entity);
                var updatedEntity = await _repository.UpdateAsync(entity, cancellationToken);

                _logger.LogInformation("Destination CRM Configuration updated successfully with ID: {ConfigId}", id);
                return MapEntityToResponse(updatedEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Destination CRM Configuration with ID {ConfigId} and payload {@Request}", id, request);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Deleting Destination CRM Configuration with ID: {ConfigId}", id);

                var result = await _repository.DeleteAsync(id, cancellationToken);

                if (result)
                    _logger.LogInformation("Destination CRM Configuration deleted successfully. ID: {ConfigId}", id);
                else
                    _logger.LogWarning("Delete operation failed or configuration not found. ID: {ConfigId}", id);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Destination CRM Configuration with ID {ConfigId}", id);
                throw;
            }
        }

        // --- Mapping Helpers ---

        private static DestinationCrmConfiguration MapRequestToEntity(
            DestinationCrmConfigurationRequest request,
            DestinationCrmConfiguration entity)
        {
            try
            {
                entity.CrmName = request.CrmName;
                entity.CooperationStartDate = request.CooperationStartDate;
                entity.LeadRegistrationEndpoint = request.LeadRegistrationEndpoint;
                entity.LeadRegistrationMethod = request.LeadRegistrationMethod;
                entity.LeadRegistrationHeaders = request.LeadRegistrationHeaders;
                entity.LeadRegistrationBodyTemplate = request.LeadRegistrationBodyTemplate;
                entity.LeadsRegistrationQueryParams = request.LeadsRegistrationQueryParams;
                entity.LeadsRetrievalEndpoint = request.LeadsRetrievalEndpoint;
                entity.LeadsRetrievalMethod = request.LeadsRetrievalMethod;
                entity.LeadsRetrievalHeaders = request.LeadsRetrievalHeaders;
                entity.LeadRetrievalBodyTemplate = request.LeadRetrievalBodyTemplate;
                entity.LeadsRetrievalQueryParams = request.LeadsRetrievalQueryParams;
                entity.AuthType = request.AuthType;
                entity.AuthConfig = request.AuthConfig;
                entity.ResponseMappingProperty = request.ResponseMappingProperty;
                entity.ErrorIdentifier = request.ErrorIdentifier;

                return entity;
            }
            catch (Exception ex)
            {
                // Logging is static method; logger not accessible here
                // We rethrow so the calling method’s try/catch handles it
                throw new Exception("Error mapping DestinationCrmConfigurationRequest to entity", ex);
            }
        }

        private static DestinationCrmConfigurationResponse MapEntityToResponse(DestinationCrmConfiguration entity)
        {
            try
            {
                return new DestinationCrmConfigurationResponse
                {
                    Id = entity.Id,
                    CrmName = entity.CrmName,
                    CooperationStartDate = entity.CooperationStartDate,

                    // --- Lead Registration ---
                    LeadRegistrationEndpoint = entity.LeadRegistrationEndpoint,
                    LeadRegistrationMethod = entity.LeadRegistrationMethod,
                    LeadRegistrationHeaders = entity.LeadRegistrationHeaders ?? new(),
                    LeadRegistrationBodyTemplate = entity.LeadRegistrationBodyTemplate ?? new(),
                    LeadsRegistrationQueryParams = entity.LeadsRegistrationQueryParams ?? new(),

                    // --- Lead Retrieval ---
                    LeadsRetrievalEndpoint = entity.LeadsRetrievalEndpoint,
                    LeadsRetrievalMethod = entity.LeadsRetrievalMethod,
                    LeadsRetrievalHeaders = entity.LeadsRetrievalHeaders ?? new(),
                    LeadRetrievalBodyTemplate = entity.LeadRetrievalBodyTemplate ?? new(),
                    LeadsRetrievalQueryParams = entity.LeadsRetrievalQueryParams ?? new(),

                    // --- Authentication ---
                    AuthType = entity.AuthType,
                    AuthConfig = entity.AuthConfig ?? new(),

                    ResponseMappingProperty = entity.ResponseMappingProperty,
                    ErrorIdentifier = entity.ErrorIdentifier ?? new(),

                    // --- Campaign ---
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error mapping DestinationCrmConfiguration entity to response", ex);
            }
        }
    }
}
