using Microsoft.EntityFrameworkCore;
using Npgsql;
using OntuPhdApi.Data;
using OntuPhdApi.Models.Documents;
using OntuPhdApi.Repositories.Document;
using OntuPhdApi.Utilities.Mappers;

namespace OntuPhdApi.Services.Documents
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly AppDbContext _context;
        private readonly ILogger<DocumentService> _logger;

        public DocumentService(
            IDocumentRepository documentRepository,
            AppDbContext context,
            ILogger<DocumentService> logger)
        {
            _documentRepository = documentRepository;
            _context = context;
            _logger = logger;
        }

        public async Task<List<DocumentDto>> GetDocumentsAsync()
        {
            _logger.LogInformation("Fetching all documents.");
            try
            {
                var documents = await _documentRepository.GetAllDocumentsAsync();
                return DocumentMapper.ToDtoList(documents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch documents.");
                throw;
            }
        }

        public async Task<DocumentDto> GetDocumentByIdAsync(int id)
        {
            _logger.LogInformation("Fetching document with ID {DocumentId}.", id);
            try
            {
                var document = await _documentRepository.GetDocumentByIdAsync(id);
                return DocumentMapper.ToDto(document);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch document with ID {DocumentId}.", id);
                throw;
            }
        }

        public async Task<List<DocumentDto>> GetDocumentsByTypeAsync(string type)
        {
            _logger.LogInformation("Fetching documents for type {Type}.", type);
            try
            {
                if (string.IsNullOrEmpty(type))
                {
                    _logger.LogWarning("Type parameter is empty or null.");
                    throw new ArgumentException("Type parameter cannot be empty or null.");
                }

                var documents = await _documentRepository.GetDocumentsByTypeAsync(type);
                return DocumentMapper.ToDtoList(documents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch documents for type {Type}.", type);
                throw;
            }
        }

        public async Task<int> AddDocumentAsync(DocumentCreateUpdateDto documentDto)
        {
            if (documentDto == null || string.IsNullOrEmpty(documentDto.Name) || string.IsNullOrEmpty(documentDto.Type) || string.IsNullOrEmpty(documentDto.Link))
            {
                _logger.LogWarning("Invalid document data provided for creation.");
                throw new ArgumentException("Name, Type, and Link are required.");
            }

            _logger.LogInformation("Adding new document with name {DocumentName}.", documentDto.Name);
            try
            {
                // Проверяем, существует ли Program, если ProgramId указан
                if (documentDto.ProgramId.HasValue)
                {
                    var programExists = await _context.Programs.AnyAsync(p => p.Id == documentDto.ProgramId.Value);
                    if (!programExists)
                    {
                        _logger.LogWarning("Program with ID {ProgramId} not found for document creation.", documentDto.ProgramId);
                        throw new KeyNotFoundException("Program not found.");
                    }
                }

                var document = DocumentMapper.ToEntity(documentDto);
                await _documentRepository.AddDocumentAsync(document);
                return document.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add document with name {DocumentName}.", documentDto.Name);
                throw;
            }
        }

        public async Task UpdateDocumentAsync(int id, DocumentCreateUpdateDto documentDto)
        {
            if (documentDto == null || string.IsNullOrEmpty(documentDto.Name) || string.IsNullOrEmpty(documentDto.Type) || string.IsNullOrEmpty(documentDto.Link))
            {
                _logger.LogWarning("Invalid document data provided for update.");
                throw new ArgumentException("Name, Type, and Link are required.");
            }

            _logger.LogInformation("Updating document with ID {DocumentId}.", id);
            try
            {
                var existingDocument = await _documentRepository.GetDocumentByIdAsync(id);
                if (existingDocument == null)
                {
                    _logger.LogWarning("Document with ID {DocumentId} not found for update.", id);
                    throw new KeyNotFoundException("Document not found.");
                }

                // Проверяем, существует ли Program, если ProgramId указан
                if (documentDto.ProgramId.HasValue)
                {
                    var programExists = await _context.Programs.AnyAsync(p => p.Id == documentDto.ProgramId.Value);
                    if (!programExists)
                    {
                        _logger.LogWarning("Program with ID {ProgramId} not found for document update.", documentDto.ProgramId);
                        throw new KeyNotFoundException("Program not found.");
                    }
                }

                DocumentMapper.UpdateEntity(existingDocument, documentDto);
                await _documentRepository.UpdateDocumentAsync(existingDocument);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update document with ID {DocumentId}.", id);
                throw;
            }
        }

        public async Task DeleteDocumentAsync(int id)
        {
            _logger.LogInformation("Deleting document with ID {DocumentId}.", id);
            try
            {
                await _documentRepository.DeleteDocumentAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete document with ID {DocumentId}.", id);
                throw;
            }
        }


    }
}
