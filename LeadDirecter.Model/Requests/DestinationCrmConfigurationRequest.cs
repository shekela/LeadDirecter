using LeadDirecter.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadDirecter.Model.Requests
{
    public class DestinationCrmConfigurationRequest
    {
        public string CrmName { get; set; } = null!;
        public DateTime CooperationStartDate { get; set; } = DateTime.UtcNow;

        // Lead Registration
        public string LeadRegistrationEndpoint { get; set; } = null!;
        public string LeadRegistrationMethod { get; set; } = "POST";
        public ContentType LeadRegistrationContentType { get; set; } = ContentType.ApplicationJson;
        public Dictionary<string, string> LeadRegistrationHeaders { get; set; } = new();
        public Dictionary<string, object> LeadRegistrationBodyTemplate { get; set; } = new();
        public Dictionary<string, string> LeadsRegistrationQueryParams { get; set; } = new();

        // Lead List Retrieval
        public string LeadsRetrievalEndpoint { get; set; } = null!;
        public string LeadsRetrievalMethod { get; set; } = "GET";
        public ContentType LeadRetrievalContentType { get; set; } = ContentType.ApplicationJson;
        public Dictionary<string, string> LeadsRetrievalHeaders { get; set; } = new();
        public Dictionary<string, object> LeadRetrievalBodyTemplate { get; set; } = new();
        public Dictionary<string, string> LeadsRetrievalQueryParams { get; set; } = new();

        // Authentication
        public string AuthType { get; set; } = null!;
        public Dictionary<string, object> AuthConfig { get; set; } = new();

        public string ResponseMappingProperty { get; set; } = null!;
        public Dictionary<string, object> ErrorIdentifier { get; set; } = new();

        // Optional: assign to a campaign
        public Guid CampaignId { get; set; }
    }
}
