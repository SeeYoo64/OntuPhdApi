using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using OntuPhdApi.Models.Documents;
using OntuPhdApi.Services;
using OntuPhdApi.Services.Documents;

namespace OntuPhdApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly ILogger<DocumentsController> _logger;

        public DocumentsController(IDocumentService documentService, ILogger<DocumentsController> logger)
        {
            _documentService = documentService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetDocuments()
        {
            _logger.LogInformation("Fetching all documents.");
            try
            {
                var documents = await _documentService.GetDocumentsAsync();
                return Ok(documents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch documents.");
                return StatusCode(500, "An error occurred while retrieving defenses.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocument(int id)
        {
            _logger.LogInformation("Fetching document with ID {DocumentId}.", id);
            try
            {
                var document = await _documentService.GetDocumentByIdAsync(id);
                if (document == null)
                {
                    _logger.LogWarning("Document with ID {DocumentId} not found.", id);
                    return StatusCode(404, $"Document with ID {id} not found.");
                }
                return Ok(document);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch document with ID {DocumentId}.", id);
                return StatusCode(500, "An error occurred while retrieving defenses.");
            }
        }

        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetDocumentsByType(string type)
        {
            _logger.LogInformation("Fetching documents for type {Type}.", type);
            try
            {
                if (string.IsNullOrEmpty(type))
                {
                    _logger.LogWarning("Type parameter is empty or null.");
                    return StatusCode(400, "Bad request.");
                }

                var documents = await _documentService.GetDocumentsByTypeAsync(type);
                return Ok(documents);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid type parameter {Type}: {ErrorMessage}", type, ex.Message);
                return StatusCode(400, "Bad request.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch documents for type {Type}.", type);
                return StatusCode(500, "An error occurred while retrieving defenses.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddDocument([FromBody] DocumentCreateUpdateDto documentDto)
        {
            _logger.LogInformation("Adding new document with name {DocumentName}.", documentDto.Name);
            try
            {
                var documentId = await _documentService.AddDocumentAsync(documentDto);
                return CreatedAtAction(
                    nameof(GetDocument),
                    new { id = documentId },
                    documentDto
                );
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid document data: {ErrorMessage}", ex.Message);
                return StatusCode(400, "Bad request.");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Program not found: {ErrorMessage}", ex.Message);
                return StatusCode(404, "Program not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add document with name {DocumentName}.", documentDto.Name);
                return StatusCode(500, "An error occurred while retrieving defenses.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocument(int id, [FromBody] DocumentCreateUpdateDto documentDto)
        {
            _logger.LogInformation("Updating document with ID {DocumentId}.", id);
            try
            {
                await _documentService.UpdateDocumentAsync(id, documentDto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid document data: {ErrorMessage}", ex.Message);
                return StatusCode(400, "Bad request.");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Document or Program not found: {ErrorMessage}", ex.Message);
                return StatusCode(404, "Document or Program not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update document with ID {DocumentId}.", id);
                return StatusCode(500, "An error occurred while retrieving defenses.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            _logger.LogInformation("Deleting document with ID {DocumentId}.", id);
            try
            {
                await _documentService.DeleteDocumentAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Document with ID {DocumentId} not found for deletion.", id);
                return StatusCode(404, $"Document with ID {id} not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete document with ID {DocumentId}.", id);
                return StatusCode(500, "An error occurred while retrieving defenses.");
            }
        }




    }
}