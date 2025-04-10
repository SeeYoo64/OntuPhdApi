using Microsoft.Extensions.Logging;
using Npgsql;
using System.IO;

namespace OntuPhdApi.Services.Files
{
    public class ProgramFileService : IProgramFileService
    {

        private readonly string _connectionString;
        private readonly ILogger<ProgramFileService> _logger;

        public ProgramFileService(IConfiguration configuration, ILogger<ProgramFileService> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public async Task<int> SaveProgramFileAsync(string programName, string filePath, string contentType, long fileSize)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                return await SaveProgramFileAsync(programName, filePath, contentType, fileSize, connection, null);
            }
        }

        public async Task<int> SaveProgramFileAsync(string programName, string filePath, string contentType, long fileSize, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            if (string.IsNullOrEmpty(filePath) || fileSize <= 0)
            {
                _logger.LogDebug("No file provided for program {ProgramName}.", programName);
                return 0;
            }

            var fileName = programName + (Path.GetExtension(filePath) ?? ".unknown");
            var query = @"
                INSERT INTO Programdocuments (FileName, FilePath, FileSize, ContentType) 
                VALUES (@FileName, @FilePath, @FileSize, @ContentType) 
                RETURNING Id";

            try
            {
                using (var cmd = new NpgsqlCommand(query, connection) { Transaction = transaction })
                {
                    cmd.Parameters.AddWithValue("FileName", fileName);
                    cmd.Parameters.AddWithValue("FilePath", filePath);
                    cmd.Parameters.AddWithValue("FileSize", fileSize);
                    cmd.Parameters.AddWithValue("ContentType", contentType ?? "application/octet-stream");

                    var documentId = (int)await cmd.ExecuteScalarAsync();
                    _logger.LogInformation("Saved file for program {ProgramName} with document ID {DocumentId}.", programName, documentId);
                    return documentId;
                }
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(ex, "Failed to save file for program {ProgramName}.", programName);
                throw;
            }
        }



        public async Task<(string FilePath, int DocumentId)> UpdateProgramFileAsync(int existingDocumentId, string fileName, string filePath, string contentType, long fileSize)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                return await UpdateProgramFileAsync(existingDocumentId, fileName, filePath, contentType, fileSize, connection, null);
            }
        }

        public async Task<(string FilePath, int DocumentId)> UpdateProgramFileAsync(int existingDocumentId, string fileName, string filePath, string contentType, long fileSize, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            if (existingDocumentId != 0)
            {
                await DeleteProgramFileAsync(existingDocumentId, connection, transaction);
            }

            var query = @"
                INSERT INTO Programdocuments (FileName, FilePath, FileSize, ContentType) 
                VALUES (@FileName, @FilePath, @FileSize, @ContentType) 
                RETURNING Id";

            try
            {
                using (var cmd = new NpgsqlCommand(query, connection) { Transaction = transaction })
                {
                    cmd.Parameters.AddWithValue("FileName", fileName);
                    cmd.Parameters.AddWithValue("FilePath", filePath);
                    cmd.Parameters.AddWithValue("FileSize", fileSize);
                    cmd.Parameters.AddWithValue("ContentType", contentType);

                    var documentId = (int)await cmd.ExecuteScalarAsync();
                    _logger.LogInformation("Updated file with document ID {DocumentId} at path {FilePath}.", documentId, filePath);
                    return (filePath, documentId);
                }
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(ex, "Failed to update file for document ID {DocumentId}.", existingDocumentId);
                throw;
            }
        }


        public async Task DeleteProgramFileAsync(int documentId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                await DeleteProgramFileAsync(documentId, connection, null);
            }
        }
        public async Task DeleteProgramFileAsync(int documentId, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            if (documentId == 0)
            {
                _logger.LogDebug("No file to delete for document ID {DocumentId}.", documentId);
                return;
            }

            var filePath = await GetProgramFilePathAsync(documentId);
            var query = "DELETE FROM Programdocuments WHERE Id = @Id";

            try
            {
                using (var cmd = new NpgsqlCommand(query, connection) { Transaction = transaction })
                {
                    cmd.Parameters.AddWithValue("Id", documentId);
                    await cmd.ExecuteNonQueryAsync();
                }

                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                {
                    File.Delete(filePath);
                    _logger.LogInformation("Deleted file at {FilePath} for document ID {DocumentId}.", filePath, documentId);
                }
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(ex, "Failed to delete file from database for document ID {DocumentId}.", documentId);
                throw;
            }
            catch (IOException ex)
            {
                _logger.LogError(ex, "Failed to delete file from disk at {FilePath} for document ID {DocumentId}.", filePath, documentId);
                throw;
            }
        }


        public async Task<string> GetProgramFilePathAsync(int documentId)
        {
            if (documentId == 0)
            {
                return null;
            }

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT FilePath FROM Programdocuments WHERE Id = @Id";

                try
                {
                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("Id", documentId);
                        var result = await cmd.ExecuteScalarAsync();
                        return result as string;
                    }
                }
                catch (NpgsqlException ex)
                {
                    _logger.LogError(ex, "Failed to retrieve file path for document ID {DocumentId}.", documentId);
                    throw;
                }
            }
        }



    }
}