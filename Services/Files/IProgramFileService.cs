using Npgsql;

namespace OntuPhdApi.Services.Files
{
    public interface IProgramFileService
    {
        Task<int> SaveProgramFileAsync(string programName, string filePath, string contentType, long fileSize);
        Task<int> SaveProgramFileAsync(string programName, string filePath, string contentType, long fileSize, NpgsqlConnection connection, NpgsqlTransaction transaction); // С транзакцией
        Task<(string FilePath, int DocumentId)> UpdateProgramFileAsync(int existingDocumentId, string fileName, string filePath, string contentType, long fileSize);
        Task<(string FilePath, int DocumentId)> UpdateProgramFileAsync(int existingDocumentId, string fileName, string filePath, string contentType, long fileSize, NpgsqlConnection connection, NpgsqlTransaction transaction); // С транзакцией
        Task<string> GetProgramFilePathAsync(int documentId);
        Task DeleteProgramFileAsync(int documentId);
        Task DeleteProgramFileAsync(int documentId, NpgsqlConnection connection, NpgsqlTransaction transaction); // С транзакцией
    }
}
