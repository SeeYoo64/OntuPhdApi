using Npgsql;

namespace OntuPhdApi.Services.Files
{
    public interface IProgramFileService
    {
        Task<int> SaveProgramFileAsync(string programName, string filePath, string contentType, long fileSize);
        Task<(string FilePath, int DocumentId)> UpdateProgramFileAsync(int existingDocumentId, string fileName, string filePath, string contentType, long fileSize);
        Task<string> GetProgramFilePathAsync(int documentId);
        Task DeleteProgramFileAsync(int documentId);
        Task<(string FilePath, string ContentType, long FileSize, int DocumentId)> SaveProgramFileFromFormAsync(string programName, IFormFile file);

    }
}
