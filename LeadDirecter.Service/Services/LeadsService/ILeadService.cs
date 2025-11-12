using LeadDirecter.Data.Entities;
using LeadDirecter.Model.Requests;
using LeadDirecter.Model.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadDirecter.Service.Services.LeadsService
{
    public interface ILeadService
    {
        Task<LeadResponse> ProcessLeadAsync(LeadRequest request, CancellationToken cancellationToken);
    }
}
