using Microsoft.EntityFrameworkCore;
using OntuPhdApi.Data;
using OntuPhdApi.Models.ApplyDocuments;

namespace OntuPhdApi.Repositories.ApplyDocument
{
    public class ApplyDocumentRepository : IApplyDocumentRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ApplyDocumentRepository> _logger;

        public ApplyDocumentRepository(AppDbContext context, ILogger<ApplyDocumentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<ApplyDocumentModel>> GetAllApplyDocumentsAsync()
        {
            _logger.LogInformation("Fetching all apply documents from database.");
            try
            {
                return await _context.ApplyDocuments.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all apply documents.");
                throw;
            }
        }

        public async Task<ApplyDocumentModel> GetApplyDocumentByIdAsync(int id)
        {
            _logger.LogInformation("Fetching apply document with ID {ApplyDocumentId} from database.", id);
            try
            {
                return await _context.ApplyDocuments.FirstOrDefaultAsync(d => d.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching apply document with ID {ApplyDocumentId}.", id);
                throw;
            }
        }

        public async Task<ApplyDocumentModel> GetApplyDocumentByNameAsync(string name)
        {
            _logger.LogInformation("Fetching apply documents for name {Name} from database.", name);
            try
            {
                return await _context.ApplyDocuments
                .Where(d => EF.Functions.ILike(d.Name, $"%{name}%"))
                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching apply documents for name {Name}.", name);
                throw;
            }
        }

        public async Task AddApplyDocumentAsync(ApplyDocumentModel applyDocument)
        {
            _logger.LogInformation("Adding new apply document with name {ApplyDocumentName}.", applyDocument.Name);
            try
            {
                await _context.ApplyDocuments.AddAsync(applyDocument);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding apply document with name {ApplyDocumentName}.", applyDocument.Name);
                throw;
            }
        }

        public async Task UpdateApplyDocumentAsync(ApplyDocumentModel applyDocument)
        {
            _logger.LogInformation("Updating apply document with ID {ApplyDocumentId} in database.", applyDocument.Id);
            try
            {
                _context.ApplyDocuments.Update(applyDocument);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating apply document with ID {ApplyDocumentId}.", applyDocument.Id);
                throw;
            }
        }

        public async Task DeleteApplyDocumentAsync(int id)
        {
            _logger.LogInformation("Deleting apply document with ID {ApplyDocumentId} from database.", id);
            try
            {
                var applyDocument = await _context.ApplyDocuments.FirstOrDefaultAsync(d => d.Id == id);
                if (applyDocument == null)
                {
                    _logger.LogWarning("Apply document with ID {ApplyDocumentId} not found for deletion.", id);
                    throw new KeyNotFoundException("Apply document not found.");
                }

                _context.ApplyDocuments.Remove(applyDocument);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting apply document with ID {ApplyDocumentId}.", id);
                throw;
            }
        }

    }
}
