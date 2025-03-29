using Microsoft.AspNetCore.Mvc;
using OntuPhdApi.Models;
using OntuPhdApi.Services;

namespace OntuPhdApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplyDocumentsController : ControllerBase
    {
        private readonly DatabaseService _dbService;

        public ApplyDocumentsController(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public IActionResult GetApplyDocuments([FromQuery] string? name)
        {
            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                    var applyDocuments = _dbService.GetApplyDocumentsByName(name);
                    return Ok(applyDocuments);
                }
                else
                {
                    var applyDocuments = _dbService.GetApplyDocuments();
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
                var applyDocument = _dbService.GetApplyDocumentById(id);
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
        public IActionResult AddApplyDocument([FromBody] ApplyDocuments applyDocument)
        {
            if (applyDocument == null || string.IsNullOrEmpty(applyDocument.Name) || string.IsNullOrEmpty(applyDocument.Description))
            {
                return BadRequest("Invalid ApplyDocument data. Name and Description are required.");
            }

            try
            {
                _dbService.AddApplyDocument(applyDocument);
                return CreatedAtAction(nameof(GetApplyDocument), new { id = applyDocument.Id }, applyDocument);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}