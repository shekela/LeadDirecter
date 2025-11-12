using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeadDirecter.Model;
using LeadDirecter.Model.Requests;
using LeadDirecter.Model.Responses;

namespace LeadDirecter.Service.Services.CampaignsService
{
    public interface ICampaignService
    {
        Task<CampaignResponse> CreateCampaignAsync(CampaignRequest request, CancellationToken cancellationToken);
        Task<CampaignResponse?> UpdateCampaignAsync(Guid id, CampaignRequest request, CancellationToken cancellationToken);
        Task<bool> DeleteCampaignAsync(Guid id, CancellationToken cancellationToken);
        Task<CampaignResponse?> GetCampaignByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
