using LeadDirecter.Data.Entities;
using LeadDirecter.Model.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadDirecter.Service.Mappers
{
    public static class LeadMapper
    {
        public static Lead MapToLead(this LeadRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return new Lead
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Country = request.Country,
                CountryCodeIso = request.CountryCodeIso,
                Prefix = request.Prefix,
                Phone = request.Phone,
                CampaignId = request.CampaignId,
                DestinationCrmConfigurationId = request.DestinationCrmConfigurationId,
                CampaignMacros = new Dictionary<string, string>(request.CampaignMacros),
                CustomProperties = new Dictionary<string, string>(request.CustomProperties),
                LeadIdInExternalCrm = "UNKNOWN",
                IsSentSuccessfully = false,
                IsFtd = false,
                IsVerifiedFtd = false,
                FtdAmount = 0
            };
        }
    }
}
