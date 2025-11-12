using LeadDirecter.Data.Entities;
using LeadDirecter.Model.Requests;
using LeadDirecter.Service.Services.LeadsService;
using LeadDirecter.WebApi.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeadDirecter.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeadsController : ControllerBase
    {
        private readonly ILeadService _leadService;

        public LeadsController(ILeadService leadService)
        {
            _leadService = leadService;
        }

        [HttpPost]
        [ValidateModel(typeof(LeadRequest))]
        public async Task<IActionResult> CreateLead([FromBody] LeadRequest lead, CancellationToken cancellationToken = default)
        {
            try
            {
                var createdLead = await _leadService.ProcessLeadAsync(lead, cancellationToken);
                return Ok(createdLead);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
