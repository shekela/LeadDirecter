using LeadDirecter.Data.Entities;
using LeadDirecter.Model.Requests;
using LeadDirecter.Model.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadDirecter.Service.Services.DestinationCrmConfigurationService
{
    public interface IDestinationCrmConfigurationService
    {
        Task<DestinationCrmConfigurationResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<DestinationCrmConfigurationResponse>> GetAllAsync(CancellationToken cancellationToken);
        Task<DestinationCrmConfigurationResponse> CreateAsync(DestinationCrmConfigurationRequest request, CancellationToken cancellationToken);
        Task<DestinationCrmConfigurationResponse?> UpdateAsync(Guid id, DestinationCrmConfigurationRequest request, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
