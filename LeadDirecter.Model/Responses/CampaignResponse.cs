using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadDirecter.Model.Responses
{
    public class CampaignResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string TargetCountry { get; set; } = null!;
        public string TrafficBuyerName { get; set; } = null!;
        public string FunnelName { get; set; } = null!;
        public List<string> CampaignMacros { get; set; } = new List<string>();
    }
}
