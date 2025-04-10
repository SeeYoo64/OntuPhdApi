using Microsoft.Extensions.Logging;
using Npgsql;
using OntuPhdApi.Data;
using OntuPhdApi.Models.Programs;
using System.IO;

namespace OntuPhdApi.Services.Files
{
    public class ProgramFileService : IProgramFileService
    {

        private readonly AppDbContext _context;
        private readonly ILogger<ProgramFileService> _logger;
        private readonly string _uploadFolder;

        public ProgramFileService(AppDbContext context, ILogger<ProgramFileService> logger)
        {
            _context = context;
            _logger = logger;
            _uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Files/Uploads/Programs");
        }

        public async Task<int> SaveProgramFileAsync(string programName, string filePath, string contentType, long fileSize)
        {
            if (string.IsNullOrEmpty(filePath) || fileSize <= 0)
            {
                _logger.LogDebug("No file provided for program {ProgramName}.", programName);
                return 0;
            }

            var fileName = programName + Path.GetExtension(filePath);
            var document = new ProgramDocument
            {
                FileName = fileName,
                FilePath = filePath,
                FileSize = fileSize,
                ContentType = contentType ?? "application/octet-stream"
            };
            _context.ProgramDocuments.Add(document);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Saved file for program {ProgramName} with document ID {DocumentId}.", programName, document.Id);
            return document.Id;
        }

        public async Task<(string FilePath, string ContentType, long FileSize, int DocumentId)> SaveProgramFileFromFormAsync(string programName, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogDebug("No file provided for program {ProgramName}.", programName);
                return (null, null, 0, 0);
            }

            if (!Directory.Exists(_uploadFolder))
            {
                Directory.CreateDirectory(_uploadFolder);
            }

            var originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
            var extension = Path.GetExtension(file.FileName);
            var uniqueSuffix = DateTime.Now.ToString("yyyyMMddHHmmss");
            var fileName = $"{originalFileName}_{uniqueSuffix}{extension}";
            var filePath = Path.Combine(_uploadFolder, fileName);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var documentId = await SaveProgramFileAsync(programName, filePath, file.ContentType, file.Length);
                return (filePath, file.ContentType, file.Length, documentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save file for program {ProgramName}.", programName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                throw;
            }
        }


        public async Task<(string FilePath, int DocumentId)> UpdateProgramFileAsync(int existingDocumentId, string fileName, string filePath, string contentType, long fileSize)
        {
            if (existingDocumentId != 0)
            {
                await DeleteProgramFileAsync(existingDocumentId);
            }

            var document = new ProgramDocument
            {
                FileName = fileName,
                FilePath = filePath,
                FileSize = fileSize,
                ContentType = contentType
            };

            _context.ProgramDocuments.Add(document);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated file with document ID {DocumentId} at path {FilePath}.", document.Id, filePath);
            return (filePath, document.Id);
        }

        public async Task<string> GetProgramFilePathAsync(int documentId)
        {
            var document = await _context.ProgramDocuments.FindAsync(documentId);
            return document?.FilePath;
        }

        public async Task DeleteProgramFileAsync(int documentId)
        {
            if (documentId == 0)
            {
                _logger.LogDebug("No file to delete for document ID {DocumentId}.", documentId);
                return;
            }

            var document = await _context.ProgramDocuments.FindAsync(documentId);
            if (document != null)
            {
                _context.ProgramDocuments.Remove(document);
                await _context.SaveChangesAsync();

                if (!string.IsNullOrEmpty(document.FilePath) && File.Exists(document.FilePath))
                {
                    File.Delete(document.FilePath);
                    _logger.LogInformation("Deleted file at {FilePath} for document ID {DocumentId}.", document.FilePath, documentId);
                }
            }
        }


    }
}