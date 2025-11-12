using LeadDirecter.Data.Entities;
using LeadDirecter.Model.Requests;
using LeadDirecter.Model.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadDirecter.Service.Validations.LeadValidationsService
{
    public interface ILeadValidationService
    {
        Task<LeadValidationResponse> ValidateAsync(LeadRequest lead, CancellationToken cancellationToken);
    }
}
