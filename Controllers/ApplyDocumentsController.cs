using Microsoft.AspNetCore.Mvc;
using OntuPhdApi.Models;
using OntuPhdApi.Services;
using OntuPhdApi.Services.ApplyDocuments;

namespace OntuPhdApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplyDocumentsController : ControllerBase
    {
        private readonly IApplyDocumentsService _applyDocumentService;

        public ApplyDocumentsController(IApplyDocumentsService applyDocumentService)
        {
            _applyDocumentService = applyDocumentService;
        }

        [HttpGet]
        public IActionResult GetApplyDocuments([FromQuery] string? name)
        {
            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                    var applyDocuments = _applyDocumentService.GetApplyDocumentsByName(name);
                    return Ok(applyDocuments);
                }
                else
                {
                    var applyDocuments = _applyDocumentService.GetApplyDocuments();
                    return Ok(applyDocuments);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetApplyDocument(int id)
        {
            try
            {
                var applyDocument = _applyDocumentService.GetApplyDocumentById(id);
                if (applyDocument == null)
                {
                    return NotFound($"ApplyDocument with ID {id} not found.");
                }
                return Ok(applyDocument);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult AddApplyDocument([FromBody] ApplyDocumentsModel applyDocument)
        {
            if (applyDocument == null || string.IsNullOrEmpty(applyDocument.Name) || string.IsNullOrEmpty(applyDocument.Description))
            {
                return BadRequest("Invalid ApplyDocument data. Name and Description are required.");
            }

            try
            {
                _applyDocumentService.AddApplyDocument(applyDocument);
                return CreatedAtAction(nameof(GetApplyDocument), new { id = applyDocument.Id }, applyDocument);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}