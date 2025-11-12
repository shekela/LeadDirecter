using LeadDirecter.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadDirecter.Model.Requests
{
    public class CampaignValidationsRequest
    {
        public Guid CampaignId { get; set; }
        public ValidationType ValidationType { get; set; }
        public bool IsEnabled { get; set; }
    }
}
