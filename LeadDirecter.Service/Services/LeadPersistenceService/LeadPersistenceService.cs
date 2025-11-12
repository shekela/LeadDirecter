using LeadDirecter.Data.Context;
using LeadDirecter.Data.Entities;
using LeadDirecter.Data.Repository.LeadRepository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadDirecter.Service.Services.LeadPersistenceService
{
    public class LeadPersistenceService : ILeadPersistenceService
    {
        private readonly ILeadRepository _leadRepository;
        private readonly ILogger<LeadPersistenceService> _logger;

        public LeadPersistenceService(ILeadRepository leadRepository, ILogger<LeadPersistenceService> logger)
        {
            _leadRepository = leadRepository;
            _logger = logger;
        }

        public async Task<bool> SaveLeadWithRetryAsync(Lead lead, CancellationToken cancellationToken)
        {
            const int maxAttempts = 5;
            var delay = TimeSpan.FromSeconds(2);

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    await _leadRepository.AddLeadAsync(lead, cancellationToken);
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to save lead, attempt {Attempt}. Retrying...", attempt);
                    await Task.Delay(delay);
                    delay *= 2;
                }
            }

            _logger.LogCritical("Failed to persist lead; {Lead} after {MaxAttempts} attempts.",lead, maxAttempts);
            return false;
        }
    }
}
