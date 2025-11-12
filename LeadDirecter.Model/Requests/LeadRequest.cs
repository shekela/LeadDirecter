using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadDirecter.Model.Requests
{
    public class LeadRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string CountryCodeIso { get; set; }
        public string Prefix { get; set; }
        public string Phone { get; set; }
        public Dictionary<string, string> CampaignMacros { get; set; } = new();
        public Dictionary<string, string> CustomProperties { get; set; } = new();
        public Guid CampaignId { get; set; }
        public Guid DestinationCrmConfigurationId { get; set; }
    }
}
