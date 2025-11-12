using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadDirecter.Model.Responses
{
    public class LeadValidationResponse
    {
        public bool IsValid => !FailedRules.Any();
        public List<string> FailedRules { get; set; } = new();
    }
}
