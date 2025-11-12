using LeadDirecter.Data.Entities;
using LeadDirecter.Model.Requests;
using LeadDirecter.Model.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadDirecter.Service.Mappers
{
    public static class CampaignValidationMapper
    {
        public static CampaignValidation ToEntity(this CampaignValidationsRequest request)
        {
            return new CampaignValidation
            {
                Id = Guid.NewGuid(), // include if your request has Id (for updates)
                CampaignId = request.CampaignId,
                ValidationType = request.ValidationType,
                IsEnabled = request.IsEnabled
            };
        }

        public static CampaignValidationResponse ToResponse(this CampaignValidation entity)
        {
            return new CampaignValidationResponse
            {
                Id = entity.Id,
                CampaignId = entity.CampaignId,
                ValidationType = entity.ValidationType,
                IsEnabled = entity.IsEnabled
            };
        }

        public static IEnumerable<CampaignValidationResponse> ToResponseList(this IEnumerable<CampaignValidation> entities)
        {
            return entities.Select(e => e.ToResponse());
        }
    }
}
