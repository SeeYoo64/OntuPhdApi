using Microsoft.EntityFrameworkCore;
using OntuPhdApi.Data;
using OntuPhdApi.Models.Documents;

namespace OntuPhdApi.Repositories.Document
{
    public class DocumentRepository : IDocumentRepository
    {

        private readonly AppDbContext _context;
        private readonly ILogger<DocumentRepository> _logger;

        public DocumentRepository(AppDbContext context, ILogger<DocumentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<DocumentModel>> GetAllDocumentsAsync()
        {
            _logger.LogInformation("Fetching all documents from database.");
            try
            {
                return await _context.Documents.OrderBy(dc => dc.Id).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all documents.");
                throw;
            }
        }

        public async Task<DocumentModel> GetDocumentByIdAsync(int id)
        {
            _logger.LogInformation("Fetching document with ID {DocumentId} from database.", id);
            try
            {
                return await _context.Documents.FirstOrDefaultAsync(d => d.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching document with ID {DocumentId}.", id);
                throw;
            }
        }

        public async Task<List<DocumentModel>> GetDocumentsByTypeAsync(string type)
        {
            _logger.LogInformation("Fetching documents for type {Type} from database.", type);
            try
            {
                return await _context.Documents
                    .Where(d => d.Type == type)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching documents for type {Type}.", type);
                throw;
            }
        }

        public async Task AddDocumentAsync(DocumentModel document)
        {
            _logger.LogInformation("Adding new document with name {DocumentName}.", document.Name);
            try
            {
                await _context.Documents.AddAsync(document);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding document with name {DocumentName}.", document.Name);
                throw;
            }
        }

        public async Task UpdateDocumentAsync(DocumentModel document)
        {
            _logger.LogInformation("Updating document with ID {DocumentId} in database.", document.Id);
            try
            {
                _context.Documents.Update(document);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating document with ID {DocumentId}.", document.Id);
                throw;
            }
        }

        public async Task DeleteDocumentAsync(int id)
        {
            _logger.LogInformation("Deleting document with ID {DocumentId} from database.", id);
            try
            {
                var document = await _context.Documents.FirstOrDefaultAsync(d => d.Id == id);
                if (document == null)
                {
                    _logger.LogWarning("Document with ID {DocumentId} not found for deletion.", id);
                    throw new KeyNotFoundException("Document not found.");
                }

                _context.Documents.Remove(document);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting document with ID {DocumentId}.", id);
                throw;
            }
        }

    }
}
