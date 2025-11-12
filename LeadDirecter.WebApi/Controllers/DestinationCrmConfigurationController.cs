using LeadDirecter.Data.Entities;
using LeadDirecter.Model.Requests;
using LeadDirecter.Service.Services.DestinationCrmConfigurationService;
using LeadDirecter.WebApi.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeadDirecter.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DestinationCrmConfigurationController : ControllerBase
    {
        private readonly IDestinationCrmConfigurationService _service;

        public DestinationCrmConfigurationController(IDestinationCrmConfigurationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _service.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _service.GetByIdAsync(id, cancellationToken);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [ValidateModel(typeof(DestinationCrmConfigurationRequest))]
        public async Task<IActionResult> Create([FromBody] DestinationCrmConfigurationRequest request, CancellationToken cancellationToken = default)
        {
            var created = await _service.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] DestinationCrmConfigurationRequest request, CancellationToken cancellationToken = default)
        {
            var updated = await _service.UpdateAsync(id, request, cancellationToken);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var deleted = await _service.DeleteAsync(id, cancellationToken);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
