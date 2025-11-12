using LeadDirecter.Model.Requests;
using LeadDirecter.Service.Services.CampaignsService;
using LeadDirecter.WebApi.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeadDirecter.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignsController : ControllerBase
    {
        private readonly ICampaignService _campaignService;

        public CampaignsController(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        // POST api/campaign
        [HttpPost]
        [ValidateModel(typeof(CampaignRequest))]
        public async Task<IActionResult> CreateCampaign([FromBody] CampaignRequest request, CancellationToken cancellationToken)
        {
            var created = await _campaignService.CreateCampaignAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetCampaignById), new { id = created.Id }, created);
        }

        // PATCH api/campaign/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCampaign(Guid id, [FromBody] CampaignRequest request, CancellationToken cancellationToken)
        {
            var updated = await _campaignService.UpdateCampaignAsync(id, request, cancellationToken);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // DELETE api/campaign/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampaign(Guid id, CancellationToken cancellationToken)
        {
            var deleted = await _campaignService.DeleteCampaignAsync(id, cancellationToken);
            if (!deleted) return NotFound();
            return NoContent();
        }

        // GET api/campaign/{id} (for CreatedAtAction)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCampaignById(Guid id, CancellationToken cancellationToken)
        {
            var campaign = await _campaignService.GetCampaignByIdAsync(id, cancellationToken);
            if (campaign == null) return NotFound();
            return Ok(campaign);
        }
    }
}
