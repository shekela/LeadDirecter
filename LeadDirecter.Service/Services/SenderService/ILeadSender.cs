using LeadDirecter.Data.Entities;
using LeadDirecter.Model.ApiClientResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadDirecter.Service.Services.SenderService
{
    public interface ILeadSender
    {
        Task<CrmResponse> SendToCrmAsync(HttpRequestMessage request, string correlationId, DestinationCrmConfiguration crmConfig, CancellationToken cancellationToken);
    }
}
