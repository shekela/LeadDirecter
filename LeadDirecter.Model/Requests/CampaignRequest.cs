using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadDirecter.Model.Requests
{
    public class CampaignRequest
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string TargetCountry { get; set; } = null!;
        public string TrafficBuyerName { get; set; } = null!;
        public string TrafficSourceName { get; set; } = null!;
        public string FunnelName { get; set; } = null!;
        public List<string> CampaignMacros { get; set; } = new List<string>();
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
    }
}
