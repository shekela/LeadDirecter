using LeadDirecter.Data.Entities;
using LeadDirecter.Data.Repository.DestinationCrmConfigurationRepository;
using LeadDirecter.Model.ApiClientResponses;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LeadDirecter.Service.Services.SenderService
{
    public class LeadSender : ILeadSender
    {
        private readonly HttpClient _httpClient;
        private readonly IDestinationCrmConfigurationRepository _destinationCrmConfigurationRepository;
        private readonly ILogger<LeadSender> _logger;

        public LeadSender(HttpClient httpClient, ILogger<LeadSender> logger, IDestinationCrmConfigurationRepository destinationCrmConfigurationRepository)
        {
            _httpClient = httpClient;
            _destinationCrmConfigurationRepository = destinationCrmConfigurationRepository;
            _logger = logger;
        }

        public async Task<CrmResponse> SendToCrmAsync(
             HttpRequestMessage request,
             string correlationId,
             DestinationCrmConfiguration crmConfig,
             CancellationToken cancellationToken)
        {
            var requestDetails = await GetHttpRequestMessageDetailsAsync(request);
            LogRequestStart(requestDetails, correlationId);

            using var cts = CreateLinkedTimeoutToken(cancellationToken, TimeSpan.FromSeconds(5));

            try
            {
                var response = await _httpClient.SendAsync(request, cts.Token);
                var rawResponse = await response.Content.ReadAsStringAsync(cts.Token);

                LogResponse(rawResponse, correlationId, response.StatusCode);

                var jsonResponse = TryDeserializeJson(rawResponse);
                var (isError, externalLeadId) = EvaluateCrmResponse(jsonResponse, crmConfig, correlationId);

                if (isError || !response.IsSuccessStatusCode)
                {
                    LogCrmFailure(requestDetails, rawResponse, correlationId, response.StatusCode);
                }

                return new CrmResponse(!isError && response.IsSuccessStatusCode, jsonResponse, externalLeadId);
            }
            catch (OperationCanceledException ex)
            {
                if (!cancellationToken.IsCancellationRequested)
                    _logger.LogWarning(ex, "CRM request timed out after 5s. {CorrelationId}", correlationId);

                return new CrmResponse(false, new { error = "timeout" }, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error sending lead to CRM. {CorrelationId}", correlationId);
                return new CrmResponse(false, new { error = ex.Message }, null);
            }
        }

        private static CancellationTokenSource CreateLinkedTimeoutToken(CancellationToken baseToken, TimeSpan timeout)
        {
            var cts = CancellationTokenSource.CreateLinkedTokenSource(baseToken);
            cts.CancelAfter(timeout);
            return cts;
        }

        private void LogRequestStart(object requestDetails, string correlationId)
        {
            _logger.LogInformation("Sending lead to CRM. {CorrelationId} {@RequestDetails}", correlationId, requestDetails);
        }

        private void LogResponse(string rawResponse, string correlationId, HttpStatusCode statusCode)
        {
            _logger.LogInformation("← CRM responded ({StatusCode}). {CorrelationId} {Response}",
                statusCode, correlationId, rawResponse); 
        }

        private void LogCrmFailure(object requestDetails, string rawResponse, string correlationId, HttpStatusCode statusCode)
        {
            _logger.LogError("CRM call failed. {CorrelationId} | Status: {StatusCode} | Request: {@RequestDetails} | Response: {Response}",
                correlationId, statusCode, requestDetails, rawResponse);
        }

        private static object? TryDeserializeJson(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return null;

            try
            {
                return JsonSerializer.Deserialize<object>(raw);
            }
            catch (JsonException)
            {
                return raw; // keep original string if not valid JSON
            }
        }

        private static (bool IsError, string? ExternalLeadId) EvaluateCrmResponse(
    object? jsonResponse, DestinationCrmConfiguration crmConfig, string correlationId)
        {
            bool isError = false;
            string? externalLeadId = null;

            if (jsonResponse is not JsonElement root)
                return (false, null);

            var props = root.EnumerateObject().ToDictionary(p => p.Name, p => p.Value, StringComparer.OrdinalIgnoreCase);

            // Detect error
            if (crmConfig.ErrorIdentifier?.Count > 0)
            {
                foreach (var kvp in crmConfig.ErrorIdentifier)
                {
                    if (props.TryGetValue(kvp.Key, out var value))
                    {
                        var expected = kvp.Value?.ToString()?.Trim('"', ' ');
                        var actual = value.ToString()?.Trim('"', ' ');
                        if (string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase))
                        {
                            isError = true;
                            break;
                        }
                    }
                }
            }

            // Extract lead ID
            if (!isError && !string.IsNullOrWhiteSpace(crmConfig.ResponseMappingProperty))
            {
                var key = crmConfig.ResponseMappingProperty.Trim();
                if (root.TryGetProperty(key, out var idValue))
                {
                    externalLeadId = idValue.ToString();
                }
            }

            return (isError, externalLeadId);
        }

        private async Task<object> GetHttpRequestMessageDetailsAsync(HttpRequestMessage request)
        {
            var details = new Dictionary<string, object?>
            {
                ["method"] = request.Method.Method,
                ["url"] = request.RequestUri?.ToString(),
                ["httpVersion"] = request.Version?.ToString()
            };

            var headers = request.Headers.ToDictionary(h => h.Key, h => string.Join(", ", h.Value));
            if (request.Content != null)
            {
                foreach (var header in request.Content.Headers)
                    headers[header.Key] = string.Join(", ", header.Value);
            }
            details["headers"] = headers;

            // Body
            if (request.Content != null)
            {
                try
                {
                    var body = await request.Content.ReadAsStringAsync();

                    if (!string.IsNullOrWhiteSpace(body))
                    {
                        try
                        {
                            using var jsonDoc = JsonDocument.Parse(body);
                            details["body"] = ConvertJsonElement(jsonDoc.RootElement);
                        }
                        catch
                        {
                            details["body"] = body; // fallback to raw string if not JSON
                        }
                    }
                }
                catch (Exception ex)
                {
                    details["body"] = $"<Failed to read body: {ex.Message}>";
                }
            }

            return details;
        }

        private static object? ConvertJsonElement(JsonElement element)
        {
            return element.ValueKind switch
            {
                JsonValueKind.Object => element.EnumerateObject()
                    .ToDictionary(p => p.Name, p => ConvertJsonElement(p.Value)),
                JsonValueKind.Array => element.EnumerateArray()
                    .Select(ConvertJsonElement).ToList(),
                JsonValueKind.String => element.GetString(),
                JsonValueKind.Number => element.TryGetInt64(out var l) ? l :
                                        element.TryGetDouble(out var d) ? d : (object?)element.ToString(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null,
                _ => element.ToString()
            };
        }

    }
}
