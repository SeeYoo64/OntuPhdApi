using Npgsql;
using OntuPhdApi.Models.ApplyDocuments;
using OntuPhdApi.Repositories.ApplyDocument;
using OntuPhdApi.Utilities.Mappers;
using System.Text.Json;

namespace OntuPhdApi.Services.ApplyDocuments
{
    public class ApplyDocumentsService : IApplyDocumentsService
    {
        private readonly IApplyDocumentRepository _applyDocumentRepository;
        private readonly ILogger<ApplyDocumentsService> _logger;

        public ApplyDocumentsService(
            IApplyDocumentRepository applyDocumentRepository,
            ILogger<ApplyDocumentsService> logger)
        {
            _applyDocumentRepository = applyDocumentRepository;
            _logger = logger;
        }

        public async Task<List<ApplyDocumentDto>> GetApplyDocumentsAsync()
        {
            _logger.LogInformation("Fetching all apply documents.");
            try
            {
                var applyDocuments = await _applyDocumentRepository.GetAllApplyDocumentsAsync();
                return ApplyDocumentMapper.ToDtoList(applyDocuments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch apply documents.");
                throw;
            }
        }

        public async Task<ApplyDocumentDto> GetApplyDocumentByIdAsync(int id)
        {
            _logger.LogInformation("Fetching apply document with ID {ApplyDocumentId}.", id);
            try
            {
                var applyDocument = await _applyDocumentRepository.GetApplyDocumentByIdAsync(id);
                return ApplyDocumentMapper.ToDto(applyDocument);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch apply document with ID {ApplyDocumentId}.", id);
                throw;
            }
        }

        public async Task<ApplyDocumentDto> GetApplyDocumentByNameAsync(string name)
        {
            _logger.LogInformation("Fetching apply document for name {Name}.", name);
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    _logger.LogWarning("Name parameter is empty or null.");
                    throw new ArgumentException("Name parameter cannot be empty or null.");
                }

                var applyDocuments = await _applyDocumentRepository.GetApplyDocumentByNameAsync(name);
                return ApplyDocumentMapper.ToDto(applyDocuments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch apply documents for name {Name}.", name);
                throw;
            }
        }

        public async Task<int> AddApplyDocumentAsync(ApplyDocumentCreateUpdateDto applyDocumentDto)
        {
            if (applyDocumentDto == null || string.IsNullOrEmpty(applyDocumentDto.Name) || string.IsNullOrEmpty(applyDocumentDto.Description))
            {
                _logger.LogWarning("Invalid apply document data provided for creation.");
                throw new ArgumentException("Name and Description are required.");
            }

            _logger.LogInformation("Adding new apply document with name {ApplyDocumentName}.", applyDocumentDto.Name);
            try
            {
                var applyDocument = ApplyDocumentMapper.ToEntity(applyDocumentDto);
                await _applyDocumentRepository.AddApplyDocumentAsync(applyDocument);
                return applyDocument.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add apply document with name {ApplyDocumentName}.", applyDocumentDto.Name);
                throw;
            }
        }

        public async Task UpdateApplyDocumentAsync(int id, ApplyDocumentCreateUpdateDto applyDocumentDto)
        {
            if (applyDocumentDto == null || string.IsNullOrEmpty(applyDocumentDto.Name) || string.IsNullOrEmpty(applyDocumentDto.Description))
            {
                _logger.LogWarning("Invalid apply document data provided for update.");
                throw new ArgumentException("Name and Description are required.");
            }

            _logger.LogInformation("Updating apply document with ID {ApplyDocumentId}.", id);
            try
            {
                var existingApplyDocument = await _applyDocumentRepository.GetApplyDocumentByIdAsync(id);
                if (existingApplyDocument == null)
                {
                    _logger.LogWarning("Apply document with ID {ApplyDocumentId} not found for update.", id);
                    throw new KeyNotFoundException("Apply document not found.");
                }

                ApplyDocumentMapper.UpdateEntity(existingApplyDocument, applyDocumentDto);
                await _applyDocumentRepository.UpdateApplyDocumentAsync(existingApplyDocument);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update apply document with ID {ApplyDocumentId}.", id);
                throw;
            }
        }

        public async Task DeleteApplyDocumentAsync(int id)
        {
            _logger.LogInformation("Deleting apply document with ID {ApplyDocumentId}.", id);
            try
            {
                await _applyDocumentRepository.DeleteApplyDocumentAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete apply document with ID {ApplyDocumentId}.", id);
                throw;
            }
        }

    }
}
