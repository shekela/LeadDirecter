using LeadDirecter.Data.Repository.DestinationCrmConfigurationRepository;
using LeadDirecter.Model.Requests;
using LeadDirecter.Model.Responses;
using LeadDirecter.Service.Services.RequestBuilderService;
using LeadDirecter.Service.Services.LeadPersistenceService;
using LeadDirecter.Service.Services.SenderService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using LeadDirecter.Model.ApiClientResponses;
using LeadDirecter.Data.Entities;
using LeadDirecter.Service.Mappers;

namespace LeadDirecter.Service.Services.LeadsService
{
    public class LeadService : ILeadService
    {
        private readonly IRequestBuilder _leadBuilder;
        private readonly ILeadSender _leadSender;
        private readonly ILeadPersistenceService _leadPersistence;
        private readonly IDestinationCrmConfigurationRepository _destinationCrmConfigurationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<LeadService> _logger;

        public LeadService(
            IRequestBuilder leadBuilder,
            ILeadSender leadSender,
            ILeadPersistenceService leadPersistence,
            IDestinationCrmConfigurationRepository destinationCrmConfigurationRepository,
            IHttpContextAccessor httpContextAccessor,
            ILogger<LeadService> logger)
        {
            _leadBuilder = leadBuilder;
            _leadSender = leadSender;
            _leadPersistence = leadPersistence;
            _destinationCrmConfigurationRepository = destinationCrmConfigurationRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<LeadResponse> ProcessLeadAsync(LeadRequest request, CancellationToken cancellationToken)
        {
            var correlationId = _httpContextAccessor.HttpContext?.Request.Headers["X-Correlation-ID"].ToString()
                                ?? Guid.NewGuid().ToString();

            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                _logger.LogInformation("Processing new lead. {CorrelationId}", correlationId);
                var lead = request.MapToLead();
                var saved = await SaveLeadAsync(lead, cancellationToken);
                //var crmConfig = await GetCrmConfigurationAsync(request, cancellationToken);
                //var (lead, crmRequest) = await _leadBuilder.BuildLeadRequestAsync(request, crmConfig);
                //var crmResponse = await SendLeadToCrmAsync(crmRequest, correlationId, crmConfig);
                

                //_logger.LogInformation(
                //    "Lead processed. Success={Success}, Saved={Saved}, CorrelationId={CorrelationId}",
                //    crmResponse.IsSuccess, saved, correlationId);

                return new LeadResponse
                {
                    LeadSentSuccessfully = false,
                    LeadSaved = saved,
                    ApiResponse = null,
                    TraceId = correlationId
                };
            }
        }

        private async Task<DestinationCrmConfiguration> GetCrmConfigurationAsync(LeadRequest request, CancellationToken cancellationToken)
        {
            var crmConfig = await _destinationCrmConfigurationRepository
                .GetByIdAsync(request.DestinationCrmConfigurationId, cancellationToken);

            if (crmConfig == null)
            {
                _logger.LogError(
                    "Could not find configuration for campaign: {CampaignId} with id: {CrmConfigurationId}",
                    request.CampaignId, request.DestinationCrmConfigurationId);

                throw new KeyNotFoundException(nameof(request));
            }

            return crmConfig;
        }

        private async Task<CrmResponse> SendLeadToCrmAsync(HttpRequestMessage crmRequest, string correlationId, DestinationCrmConfiguration crmConfig)
        {
            using var crmCts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            try
            {
                return await _leadSender.SendToCrmAsync(crmRequest, correlationId, crmConfig, crmCts.Token);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("CRM request timed out after {TimeoutSeconds}s. Continuing to save lead locally.", 10);
                return new CrmResponse(
                    IsSuccess: false,
                    ExternalLeadId: "UNKNOWN",
                    JsonResponse: "{\"error\": \"CRM timeout\"}"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending lead to CRM");
                return new CrmResponse(
                    IsSuccess: false,
                    ExternalLeadId: "UNKNOWN",
                    JsonResponse: "{\"error\": \"CRM error\"}"
                );
            }
        }

        private async Task<bool> SaveLeadAsync(Lead lead, CancellationToken cancellationToken)
        {
            lead.LeadIdInExternalCrm = "Pending";
            lead.IsSentSuccessfully = false;

            return await _leadPersistence.SaveLeadWithRetryAsync(lead, cancellationToken);
        }

    }
}
