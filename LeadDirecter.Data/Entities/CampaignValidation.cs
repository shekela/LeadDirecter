using LeadDirecter.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LeadDirecter.Data.Entities
{
    public class CampaignValidation
    {
        public Guid Id { get; set; }
        public Guid CampaignId { get; set; }

        [JsonIgnore]
        public Campaign Campaign { get; set; }
        public ValidationType ValidationType { get; set; } 
        public bool IsEnabled { get; set; }
    }
}
