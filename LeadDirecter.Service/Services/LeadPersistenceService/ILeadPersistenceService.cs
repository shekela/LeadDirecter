using LeadDirecter.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadDirecter.Service.Services.LeadPersistenceService
{
    public interface ILeadPersistenceService
    {
        Task<bool> SaveLeadWithRetryAsync(Lead lead, CancellationToken cancellationToken);
    }
}
