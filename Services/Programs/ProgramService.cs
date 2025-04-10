using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;
using OntuPhdApi.Controllers;
using OntuPhdApi.Models.Programs;
using OntuPhdApi.Repositories;
using OntuPhdApi.Services.Files;
using System.Text.Json;

namespace OntuPhdApi.Services.Programs
{
    public class ProgramService : IProgramService
    {

        private readonly IProgramRepository _programRepository;
        private readonly IProgramFileService _fileService;
        private readonly ILogger<ProgramService> _logger;
        private readonly string _connectionString;

        public ProgramService(
            IProgramRepository programRepository,
            IProgramFileService fileService,
            IConfiguration configuration,
            ILogger<ProgramService> logger)
        {
            _programRepository = programRepository;
            _fileService = fileService;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }


        public async Task<List<ProgramModel>> GetPrograms()
        {
            try
            {
                _logger.LogInformation("Fetching all programs from the repository.");
                var programs = await _programRepository.GetAllProgramsAsync();

                foreach (var program in programs)
                {
                    _logger.LogDebug("Fetching components and jobs for program ID {ProgramId}", program.Id);
                    program.Components = await _programRepository.GetProgramComponentsAsync(program.Id);
                    program.Jobs = await _programRepository.GetProgramJobsAsync(program.Id);
                }

                _logger.LogInformation("Successfully retrieved {ProgramCount} programs.", programs.Count);
                // Сортировка по Degrees - phd -> everything else
                programs = programs
                    .OrderBy(r => r.Degree switch {
                        "phd" => 1,
                        _ => 2
                    })
                    .ThenBy(r => r.Id)
                    .ToList();
                return programs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve programs.");
                throw; // i can return null/error depends on needs
            }
        }

        public async Task<ProgramModel> GetProgram(int id)
        {
            _logger.LogInformation("Fetching program with ID {ProgramId}.", id);

            try
            {
                var program = await _programRepository.GetProgramByIdAsync(id);
                if (program == null)
                {
                    _logger.LogWarning("Program with ID {ProgramId} not found.", id);
                    return null;
                }

                _logger.LogDebug("Fetching components and jobs for program ID {ProgramId}.", id);
                program.Components = await _programRepository.GetProgramComponentsAsync(id);
                program.Jobs = await _programRepository.GetProgramJobsAsync(id);

                _logger.LogInformation("Successfully retrieved program with ID {ProgramId}.", id);
                return program;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve program with ID {ProgramId}.", id);
                throw; // Или вернуть null, в зависимости от требований API
            }
        }

        public async Task<List<ProgramsDegreeDto>> GetProgramsDegrees(DegreeType? degree)
        {
            _logger.LogInformation("Fetching programs for degree {Degree}.", degree?.ToString() ?? "all");

            try
            {
                var programs = await _programRepository.GetProgramsByDegreeAsync(degree);
                _logger.LogInformation("Successfully retrieved {ProgramCount} programs for degree {Degree}.", programs.Count, degree?.ToString() ?? "all");
                return programs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve programs for degree {Degree}.", degree);
                throw;
            }
        }


        public async Task AddProgram(ProgramModel program, string? filePath, string? contentType, long fileSize)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    await AddProgram(program, filePath, contentType, fileSize, connection, transaction);
                    await transaction.CommitAsync();
                }
            }
        }

        public async Task AddProgram(ProgramModel program, string? filePath, string? contentType, long fileSize, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            _logger.LogInformation("Adding new program {ProgramName}.", program.Name);
            try
            {
                int documentId = await _fileService.SaveProgramFileAsync(program.Name, filePath, contentType, fileSize, connection, transaction);
                program.ProgramDocumentId = documentId == 0 ? null : documentId;
                program.Id = await _programRepository.InsertProgramAsync(program, connection, transaction);
                _logger.LogInformation("Program {ProgramName} added with ID {ProgramId} and document ID {DocumentId}.", program.Name, program.Id, program.ProgramDocumentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add program {ProgramName}.", program.Name);
                throw;
            }
        }


        public async Task UpdateProgram(ProgramModel program)
        {
            _logger.LogInformation("Updating program {ProgramName} with ID {ProgramId}.", program.Name, program.Id);

            try
            {
                // Обновление программы в базе данных
                await _programRepository.UpdateProgramAsync(program);

                _logger.LogInformation("Program {ProgramName} with ID {ProgramId} updated successfully.", program.Name, program.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update program {ProgramName} with ID {ProgramId}.", program.Name, program.Id);
                throw;
            }
        }

        public async Task UpdateProgramWithDocument(ProgramModel program, string filePath, string fileName, string contentType, long fileSize)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    await UpdateProgramWithDocument(program, filePath, fileName, contentType, fileSize, connection, transaction);
                    await transaction.CommitAsync();
                }
            }
        }

        public async Task UpdateProgramWithDocument(ProgramModel program, string filePath, string fileName, string contentType, long fileSize, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            _logger.LogInformation("Updating program {ProgramName} with ID {ProgramId} and new file.", program.Name, program.Id);
            try
            {
                var (newFilePath, documentId) = await _fileService.UpdateProgramFileAsync(program.ProgramDocumentId ?? 0, fileName, filePath, contentType, fileSize, connection, transaction);
                program.ProgramDocumentId = documentId;
                await _programRepository.UpdateProgramAsync(program, connection, transaction);
                _logger.LogInformation("Program {ProgramName} with ID {ProgramId} updated with new document ID {DocumentId}.", program.Name, program.Id, documentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update program {ProgramName} with ID {ProgramId}.", program.Name, program.Id);
                throw;
            }
        }

        public async Task DeleteProgram(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    await DeleteProgram(id, connection, transaction);
                    await transaction.CommitAsync();
                }
            }
        }

        public async Task DeleteProgram(int id, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            _logger.LogInformation("Deleting program with ID {ProgramId}.", id);
            try
            {
                var program = await _programRepository.GetProgramByIdAsync(id);
                if (program == null)
                {
                    _logger.LogWarning("Program with ID {ProgramId} not found for deletion.", id);
                    throw new Exception($"Program with ID {id} not found.");
                }

                if (program.ProgramDocumentId.HasValue && program.ProgramDocumentId != 0)
                {
                    await _fileService.DeleteProgramFileAsync(program.ProgramDocumentId.Value, connection, transaction);
                }

                await _programRepository.DeleteProgramAsync(id, connection, transaction);
                _logger.LogInformation("Program with ID {ProgramId} and associated file (if any) deleted.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete program with ID {ProgramId}.", id);
                throw;
            }
        }



    }
}
