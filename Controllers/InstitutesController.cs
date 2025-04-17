using Microsoft.AspNetCore.Mvc;
using OntuPhdApi.Models.Institutes;
using OntuPhdApi.Services.Institutes;

namespace OntuPhdApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstitutesController : ControllerBase
    {
        private readonly IInstituteService _instituteService;
        private readonly ILogger<InstitutesController> _logger;

        public InstitutesController(
            IInstituteService instituteService,
            ILogger<InstitutesController> logger)
        {
            _instituteService = instituteService ?? throw new ArgumentNullException(nameof(instituteService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> GetInstitutes()
        {
            _logger.LogInformation("Fetching all institutes.");
            try
            {
                var institutes = await _instituteService.GetInstitutesAsync();
                return Ok(institutes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch institutes.");
                return StatusCode(500, "An error occurred while retrieving institutes.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInstitute(int id)
        {
            _logger.LogInformation("Fetching institute with ID {InstituteId}.", id);
            try
            {
                var institute = await _instituteService.GetInstituteAsync(id);
                if (institute == null)
                {
                    _logger.LogWarning("Institute with ID {InstituteId} not found.", id);
                    return NotFound("Institute not found.");
                }
                return Ok(institute);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch institute with ID {InstituteId}.", id);
                return StatusCode(500, "An error occurred while retrieving the institute.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddInstitute([FromBody] InstituteDto instituteDto)
        {
            _logger.LogInformation("Adding new institute with name {InstituteName}.", instituteDto.Name);
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid institute data.");
                    return BadRequest(ModelState);
                }

                var createdInstitute = await _instituteService.AddInstituteAsync(instituteDto);
                _logger.LogInformation("Institute {InstituteName} added with ID {InstituteId}.", createdInstitute.Name, createdInstitute.Id);
                return CreatedAtAction(nameof(GetInstitute), new { id = createdInstitute.Id }, createdInstitute);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add institute {InstituteName}.", instituteDto.Name);
                return StatusCode(500, "An error occurred while adding the institute.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInstitute(int id, [FromBody] InstituteDto instituteDto)
        {
            _logger.LogInformation("Updating institute with ID {InstituteId}.", id);
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid institute data.");
                    return BadRequest(ModelState);
                }

                var updatedInstitute = await _instituteService.UpdateInstituteAsync(id, instituteDto);
                if (updatedInstitute == null)
                {
                    _logger.LogWarning("Institute with ID {InstituteId} not found.", id);
                    return NotFound("Institute not found.");
                }

                _logger.LogInformation("Institute {InstituteName} with ID {InstituteId} updated.", updatedInstitute.Name, id);
                return Ok(updatedInstitute);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update institute {InstituteName} with ID {InstituteId}.", instituteDto.Name, id);
                return StatusCode(500, "An error occurred while updating the institute.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInstitute(int id)
        {
            _logger.LogInformation("Deleting institute with ID {InstituteId}.", id);
            try
            {
                await _instituteService.DeleteInstituteAsync(id);
                _logger.LogInformation("Institute with ID {InstituteId} deleted.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found"))
                {
                    _logger.LogWarning("Institute with ID {InstituteId} not found for deletion.", id);
                    return NotFound("Institute not found.");
                }
                if (ex.Message.Contains("referenced by one or more programs"))
                {
                    _logger.LogWarning("Cannot delete institute with ID {InstituteId} because it is referenced by programs.", id);
                    return BadRequest("Cannot delete institute because it is referenced by one or more programs.");
                }
                _logger.LogError(ex, "Failed to delete institute with ID {InstituteId}.", id);
                return StatusCode(500, "An error occurred while deleting the institute.");
            }
        }
    }
}
