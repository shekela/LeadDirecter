using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadDirecter.Model.Responses
{
    public class LeadResponse
    {
        public bool LeadSentSuccessfully { get; set; }
        public bool LeadSaved { get; set; }
        public object ApiResponse { get; set; }
        public string TraceId { get; set; }
    }
}
