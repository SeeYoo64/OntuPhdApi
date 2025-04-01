using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using OntuPhdApi.Models;
using OntuPhdApi.Services;
using OntuPhdApi.Services.Documents;

namespace OntuPhdApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocumentsController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpGet]
        public IActionResult GetDocuments([FromQuery] string? type)
        {
            try
            {
                if (!string.IsNullOrEmpty(type))
                {
                    var documents = _documentService.GetDocumentsByType(type);
                    return Ok(documents);
                }
                else
                {
                    var documents = _documentService.GetDocuments();

                    // Сортировка по Status: Completed -> Ontime -> NotStarted, затем по DataStart
                    documents = documents
                        .OrderBy(r => r.Type switch
                        {
                            "Entry" => 1,
                            "Normative" => 2,
                            _ => 3
                        })
                        .ThenBy(r => r.Type)
                        .ThenBy(r => r.Id)
                        .ToList();



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
                var document = _documentService.GetDocumentById(id);
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
        public IActionResult AddDocument([FromBody] DocumentsModel document)
        {
            if (document == null || string.IsNullOrEmpty(document.Name) || string.IsNullOrEmpty(document.Type) || string.IsNullOrEmpty(document.Link))
            {
                return BadRequest("Invalid document data. Name, Type, and Link are required.");
            }

            try
            {
                _documentService.AddDocument(document);
                return CreatedAtAction(nameof(GetDocument), new { id = document.Id }, document);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    
    
    
    
    }
}