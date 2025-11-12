using LeadDirecter.Data.Entities;
using LeadDirecter.Shared.Helpers;
using LeadDirecter.Data.Repository.DestinationCrmConfigurationRepository;
using LeadDirecter.Model.Requests;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LeadDirecter.Service.Services.RequestBuilderService
{
    public class RequestBuilder : IRequestBuilder
    {
        private readonly IDestinationCrmConfigurationRepository _destinationCrmConfigurationRepository;
        private readonly ILogger<RequestBuilder> _logger;

        public RequestBuilder(IDestinationCrmConfigurationRepository destinationCrmConfigurationRepository, ILogger<RequestBuilder> logger)
        {
            _destinationCrmConfigurationRepository = destinationCrmConfigurationRepository;
            _logger = logger;
        }

        public async Task<(Lead lead, HttpRequestMessage crmRequest)> BuildLeadRequestAsync(LeadRequest request, DestinationCrmConfiguration crmConfig)
        {
            var lead = new Lead
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Country = request.Country,
                CountryCodeIso = request.CountryCodeIso,
                Prefix = request.Prefix,
                Phone = request.Phone,
                CampaignId = request.CampaignId,
                DestinationCrmConfigurationId = request.DestinationCrmConfigurationId,
                CustomProperties = request.CustomProperties ?? new Dictionary<string, string>()
            };

            var payload = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            // 🧩 1. Apply the CRM-specific body template, resolving placeholders
            if (crmConfig?.LeadRegistrationBodyTemplate != null)
            {
                foreach (var kvp in crmConfig.LeadRegistrationBodyTemplate)
                {
                    payload[kvp.Key] = ResolvePlaceholders(kvp.Value?.ToString() ?? string.Empty, lead);
                }
            }

            // 🧩 2. Define the internal "standard" placeholders
            var standardFields = new Dictionary<string, string>
            {
                { "firstName", lead.FirstName },
                { "first_name", lead.FirstName },
                { "lastName", lead.LastName },
                { "last_name", lead.LastName },
                { "email", lead.Email },
                { "country", lead.Country },
                { "phone_prefix", lead.Prefix },
                { "prefix", lead.Prefix },
                { "phone", lead.Phone },
                { "campaignId", lead.CampaignId.ToString() },
                { "affiliate_id", lead.CampaignId.ToString() },
                { "full_name", $"{lead.FirstName} {lead.LastName}".Trim() }
            };

            // 🧩 3. Add any standard field that isn’t already defined in the CRM template
            foreach (var kvp in standardFields)
            {
                if (!payload.ContainsKey(kvp.Key))
                    payload[kvp.Key] = kvp.Value ?? string.Empty;
            }

            // 🧩 4. Merge custom properties (never overwrite CRM-defined fields)
            foreach (var custom in lead.CustomProperties)
            {
                if (!payload.ContainsKey(custom.Key))
                    payload[custom.Key] = custom.Value ?? string.Empty;
            }

            // 🧩 5. Prepare the content
            var contentTypeString = EnumHelper.ConvertContentTypeToString(crmConfig.LeadRegistrationContentType);
            HttpContent content;

            if (contentTypeString == "application/json")
            {
                content = new StringContent(JsonSerializer.Serialize(payload, new JsonSerializerOptions { WriteIndented = true }), Encoding.UTF8, contentTypeString);
            }
            else if (contentTypeString == "application/x-www-form-urlencoded")
            {
                content = new FormUrlEncodedContent(payload);
            }
            else if (contentTypeString == "multipart/form-data")
            {
                var multipart = new MultipartFormDataContent();
                foreach (var kv in payload)
                    multipart.Add(new StringContent(kv.Value), kv.Key);
                content = multipart;
            }
            else
            {
                content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, contentTypeString);
            }

            // 🧩 6. Build the request
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, crmConfig.LeadRegistrationEndpoint)
            {
                Content = content
            };

            // 🧩 7. Add headers
            if (crmConfig.LeadRegistrationHeaders != null)
            {
                foreach (var header in crmConfig.LeadRegistrationHeaders)
                {
                    if (!header.Key.Equals("Content-Type", StringComparison.OrdinalIgnoreCase))
                        requestMessage.Headers.Add(header.Key, header.Value);
                }
            }

            return (lead, requestMessage);
        }

        private string ResolvePlaceholders(string template, Lead lead)
        {
            if (string.IsNullOrEmpty(template)) return string.Empty;

            var result = template
                .Replace("{first_name}", lead.FirstName)
                .Replace("{last_name}", lead.LastName)
                .Replace("{email}", lead.Email)
                .Replace("{prefix}", lead.Prefix)
                .Replace("{phone}", lead.Phone)
                .Replace("{country}", lead.Country)
                .Replace("{countryIso}", lead.CountryCodeIso); 

            foreach (var custom in lead.CustomProperties)
            {
                result = result.Replace($"{{{custom.Key}}}", custom.Value ?? string.Empty);
            }

            return result;
        }
    }
}
