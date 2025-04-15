using Microsoft.AspNetCore.Mvc;
using OntuPhdApi.Models.ApplyDocuments;
using OntuPhdApi.Services;
using OntuPhdApi.Services.ApplyDocuments;

namespace OntuPhdApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplyDocumentsController : ControllerBase
    {
        private readonly IApplyDocumentsService _applyDocumentsService;
        private readonly ILogger<ApplyDocumentsController> _logger;

        public ApplyDocumentsController(IApplyDocumentsService applyDocumentsService, ILogger<ApplyDocumentsController> logger)
        {
            _applyDocumentsService = applyDocumentsService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetApplyDocuments()
        {
            _logger.LogInformation("Fetching all apply documents.");
            try
            {
                var applyDocuments = await _applyDocumentsService.GetApplyDocumentsAsync();
                return Ok(applyDocuments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch apply documents.");
                return StatusCode(500, "An error occurred while retrieving applydocuments.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetApplyDocument(int id)
        {
            _logger.LogInformation("Fetching apply document with ID {ApplyDocumentId}.", id);
            try
            {
                var applyDocument = await _applyDocumentsService.GetApplyDocumentByIdAsync(id);
                if (applyDocument == null)
                {
                    _logger.LogWarning("Apply document with ID {ApplyDocumentId} not found.", id);
                    return StatusCode(404, $"Apply document with ID {id} was not found.");
                }
                return Ok(applyDocument);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch apply document with ID {ApplyDocumentId}.", id);
                return StatusCode(500, "An error occurred while retrieving applydocuments.");
            }
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetApplyDocumentsByName(string name)
        {
            _logger.LogInformation("Fetching apply documents for name {Name}.", name);
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    _logger.LogWarning("Name parameter is empty or null.");
                    return StatusCode(400, "Bad request.");
                }

                var applyDocuments = await _applyDocumentsService.GetApplyDocumentsByNameAsync(name);
                return Ok(applyDocuments);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid name parameter {Name}: {ErrorMessage}", name, ex.Message);
                return StatusCode(400, "Bad request.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch apply documents for name {Name}.", name);
                return StatusCode(500, "An error occurred while retrieving applydocuments.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddApplyDocument([FromBody] ApplyDocumentCreateUpdateDto applyDocumentDto)
        {
            _logger.LogInformation("Adding new apply document with name {ApplyDocumentName}.", applyDocumentDto.Name);
            try
            {
                var applyDocumentId = await _applyDocumentsService.AddApplyDocumentAsync(applyDocumentDto);
                return CreatedAtAction(
                    nameof(GetApplyDocument),
                    new { id = applyDocumentId },
                    applyDocumentDto
                );
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid apply document data: {ErrorMessage}", ex.Message);
                return StatusCode(400, "Bad request.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add apply document with name {ApplyDocumentName}.", applyDocumentDto.Name);
                return StatusCode(500, "An error occurred while retrieving applydocuments.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateApplyDocument(int id, [FromBody] ApplyDocumentCreateUpdateDto applyDocumentDto)
        {
            _logger.LogInformation("Updating apply document with ID {ApplyDocumentId}.", id);
            try
            {
                await _applyDocumentsService.UpdateApplyDocumentAsync(id, applyDocumentDto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid apply document data: {ErrorMessage}", ex.Message);
                return StatusCode(400, "Bad request.");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Apply document with ID {ApplyDocumentId} not found for update.", id);
                return StatusCode(404, $"Apply document with ID {id} was not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update apply document with ID {ApplyDocumentId}.", id);
                return StatusCode(500, "An error occurred while retrieving applydocuments.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplyDocument(int id)
        {
            _logger.LogInformation("Deleting apply document with ID {ApplyDocumentId}.", id);
            try
            {
                await _applyDocumentsService.DeleteApplyDocumentAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Apply document with ID {ApplyDocumentId} not found for deletion.", id);
                return StatusCode(404, $"Apply document with ID {id} was not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete apply document with ID {ApplyDocumentId}.", id);
                return StatusCode(500, "An error occurred while retrieving applydocuments.");
            }
        }

    }
}