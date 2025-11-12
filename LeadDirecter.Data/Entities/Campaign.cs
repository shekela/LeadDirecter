using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadDirecter.Data.Entities
{
    public class Campaign
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string TargetCountry { get; set; }
        public string TrafficBuyerName { get; set; }
        public string TrafficSourceName { get; set; }
        public string FunnelName { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public List<string> CampaignMacros { get; set; } = new List<string>();


        // Relationships
        public ICollection<DestinationCrmConfiguration> DestinationCrmConfigurations { get; set; } = new List<DestinationCrmConfiguration>();
        public ICollection<Lead> Leads { get; set; } = new List<Lead>();
        public ICollection<CampaignValidation> CampaignValidations { get; set; }
    }
}
