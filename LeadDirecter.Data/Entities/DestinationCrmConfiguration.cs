using LeadDirecter.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadDirecter.Data.Entities
{
    public class DestinationCrmConfiguration
    {
        public Guid Id { get; set; }

        public string CrmName { get; set; }
        public DateTime CooperationStartDate { get; set; }

        // --- Lead Registration ---
        public string LeadRegistrationEndpoint { get; set; }
        public string LeadRegistrationMethod { get; set; } = "POST";
        public ContentType LeadRegistrationContentType { get; set; } = ContentType.ApplicationJson;
        public Dictionary<string, string> LeadRegistrationHeaders { get; set; } = new();
        public Dictionary<string, object> LeadRegistrationBodyTemplate { get; set; } = new();
        public Dictionary<string, string> LeadsRegistrationQueryParams { get; set; } = new();

        // --- Lead List Retrieval ---
        public string LeadsRetrievalEndpoint { get; set; }
        public string LeadsRetrievalMethod { get; set; } = "GET";
        public ContentType LeadRetrievalContentType { get; set; } = ContentType.ApplicationJson;
        public Dictionary<string, string> LeadsRetrievalHeaders { get; set; } = new();
        public Dictionary<string, object> LeadRetrievalBodyTemplate { get; set; } = new();
        public Dictionary<string, string> LeadsRetrievalQueryParams { get; set; } = new();

        // --- Authentication ---
        public string AuthType { get; set; } // "ApiKey", "Bearer", "OAuth2", etc.
        public Dictionary<string, object> AuthConfig { get; set; } = new();

        // Property that defines how to map CRM response back to your leads
        public string ResponseMappingProperty { get; set; }
        // Property that defines how to identify errors in CRM responses
        public Dictionary<string, object> ErrorIdentifier { get; set; } = new();
        // --- Relations ---
        public Guid? CampaignId { get; set; }
        public Campaign Campaign { get; set; }
        public ICollection<Lead> Leads { get; set; } = new List<Lead>();
    }
}