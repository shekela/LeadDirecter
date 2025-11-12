using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadDirecter.Shared.Enums
{
    public enum ValidationType
    {
        EmailExistsGlobally,
        EmailExistsInCampaign,
        EmailAlreadySentToCrm,
        PhoneExistsGlobally,
        PhoneExistsInCampaign,
        PhoneAlreadySentToCrm
    }
}
