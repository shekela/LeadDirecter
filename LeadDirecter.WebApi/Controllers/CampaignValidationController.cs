using LeadDirecter.Model.Requests;
using LeadDirecter.Service.Validations.CampaignValidationsService;
using LeadDirecter.WebApi.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LeadDirecter.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignValidationController : ControllerBase
    {
        private readonly ICampaignValidationService _service;

        public CampaignValidationController(ICampaignValidationService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all validation rules for a specific campaign.
        /// </summary>
        [HttpGet("campaign/{campaignId:guid}")]
        public async Task<IActionResult> GetByCampaign(Guid campaignId, CancellationToken cancellationToken = default)
        {
            var validations = await _service.GetByCampaignAsync(campaignId, cancellationToken);
            if (!validations.Any())
                return NotFound($"No validations found for campaign {campaignId}.");

            return Ok(validations);
        }

        /// <summary>
        /// Get a single validation rule by ID.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken = default)
        {
            var validation = await _service.GetAsync(id, cancellationToken);
            if (validation == null)
                return NotFound($"Validation {id} not found.");

            return Ok(validation);
        }

        /// <summary>
        /// Create a new validation rule for a campaign.
        /// </summary>
        [HttpPost]
        [ValidateModel(typeof(CampaignValidationsRequest))]
        public async Task<IActionResult> Create([FromBody] CampaignValidationsRequest request, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.AddAsync(request, cancellationToken);

            // Return DTO with correct 201 Created response
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        /// <summary>
        /// Update an existing validation rule.
        /// </summary>
        [HttpPut("{id:guid}")]
        [ValidateModel(typeof(CampaignValidationsRequest))]
        public async Task<IActionResult> Update(Guid id, [FromBody] CampaignValidationsRequest request, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _service.GetAsync(id, cancellationToken);
            if (existing == null)
                return NotFound($"Validation {id} not found.");

            var updated = await _service.UpdateAsync(request, cancellationToken);
            return Ok(updated);
        }

        /// <summary>
        /// Delete a validation rule.
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            var existing = await _service.GetAsync(id, cancellationToken);
            if (existing == null)
                return NotFound($"Validation {id} not found.");

            await _service.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
