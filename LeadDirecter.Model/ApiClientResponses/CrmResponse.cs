using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadDirecter.Model.ApiClientResponses
{
    public record CrmResponse(
        bool IsSuccess,
        object? JsonResponse,
        string? ExternalLeadId = null
    );
}
