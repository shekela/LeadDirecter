using LeadDirecter.Data.Entities;
using LeadDirecter.Model.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadDirecter.Service.Services.RequestBuilderService
{
    public interface IRequestBuilder
    {
        Task<(Lead lead, HttpRequestMessage crmRequest)> BuildLeadRequestAsync(LeadRequest request, DestinationCrmConfiguration crmConfig);
    }
}