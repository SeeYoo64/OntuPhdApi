using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using OntuPhdApi.Models;
using OntuPhdApi.Services;

namespace OntuPhdApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController(DatabaseService dbService) : ControllerBase
    {
        private readonly DatabaseService _dbService = dbService;

        [HttpGet]
        public IActionResult GetDocuments([FromQuery] string? type)
        {
            try
            {
                if (!string.IsNullOrEmpty(type))
                {
                    var documents = _dbService.GetDocumentsByType(type);
                    return Ok(documents);
                }
                else
                {
                    var documents = _dbService.GetDocuments();
                    return Ok(documents);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetDocument(int id)
        {
            try
            {
                var document = _dbService.GetDocumentById(id);
                if (document == null)
                {
                    return NotFound($"Document with ID {id} not found.");
                }
                return Ok(document);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult AddDocument([FromBody] Documents document)
        {
            if (document == null || string.IsNullOrEmpty(document.Name) || string.IsNullOrEmpty(document.Type) || string.IsNullOrEmpty(document.Link))
            {
                return BadRequest("Invalid document data. Name, Type, and Link are required.");
            }

            try
            {
                _dbService.AddDocument(document);
                return CreatedAtAction(nameof(GetDocument), new { id = document.Id }, document);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}