using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;
using OntuPhdApi.Controllers;
using OntuPhdApi.Data;
using OntuPhdApi.Models.Programs;
using OntuPhdApi.Repositories.Program;
using OntuPhdApi.Services.Files;
using System.Text.Json;

namespace OntuPhdApi.Services.Programs
{
    public class ProgramService : IProgramService
    {
        private readonly IProgramRepository _programRepository;
        private readonly IProgramFileService _fileService;
        private readonly AppDbContext _context;
        private readonly ILogger<ProgramService> _logger;

        public ProgramService(
            IProgramRepository programRepository,
            IProgramFileService fileService,
            AppDbContext context,
            ILogger<ProgramService> logger)
        {
            _programRepository = programRepository;
            _fileService = fileService;
            _context = context;
            _logger = logger;
        }

        public async Task<List<ProgramModelDto>> GetPrograms()
        {
            var programs = await _programRepository.GetAllProgramsAsync();
            return programs.Select(MapToDto).ToList();
        }

        public async Task<ProgramModelDto> GetProgram(int id)
        {
            var program = await _programRepository.GetProgramByIdAsync(id);
            return program != null ? MapToDto(program) : null;
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


        public async Task AddProgram(ProgramModel program, string? filePath, string? contentType, long fileSize, string? instituteName)
        {
            _logger.LogInformation("Adding new program {ProgramName}.", program.Name);
            try
            {
                if (!string.IsNullOrEmpty(instituteName))
                {
                    var institute = await _programRepository.GetOrCreateInstituteAsync(instituteName);
                    program.InstituteId = institute.Id;
                }

                int documentId = await _fileService.SaveProgramFileAsync(program.Name, filePath, contentType, fileSize);
                program.ProgramDocumentId = documentId == 0 ? null : documentId;
                program.Id = await _programRepository.InsertProgramAsync(program);
                _logger.LogInformation("Program {ProgramName} added with ID {ProgramId}, document ID {DocumentId}, and institute ID {InstituteId}.",
                    program.Name, program.Id, program.ProgramDocumentId, program.InstituteId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add program {ProgramName}.", program.Name);
                throw;
            }
        }

        public async Task UpdateProgramWithDocument(ProgramModel program, string filePath, string fileName, string contentType, long fileSize, string? instituteName)
        {
            _logger.LogInformation("Updating program {ProgramName} with ID {ProgramId} and new file.", program.Name, program.Id);
            try
            {
                if (!string.IsNullOrEmpty(instituteName))
                {
                    var institute = await _programRepository.GetOrCreateInstituteAsync(instituteName);
                    program.InstituteId = institute.Id;
                }
                else
                {
                    program.InstituteId = null; // Allow clearing the institute
                }

                var (newFilePath, documentId) = await _fileService.UpdateProgramFileAsync(program.ProgramDocumentId ?? 0, fileName, filePath, contentType, fileSize);
                program.ProgramDocumentId = documentId;
                await _programRepository.UpdateProgramAsync(program);
                _logger.LogInformation("Program {ProgramName} with ID {ProgramId} updated with new document ID {DocumentId} and institute ID {InstituteId}.",
                    program.Name, program.Id, documentId, program.InstituteId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update program {ProgramName} with ID {ProgramId}.", program.Name, program.Id);
                throw;
            }
        }

        public async Task UpdateProgram(ProgramModel program, string? instituteName)
        {
            _logger.LogInformation("Updating program {ProgramName} with ID {ProgramId}.", program.Name, program.Id);
            try
            {
                if (!string.IsNullOrEmpty(instituteName))
                {
                    var institute = await _programRepository.GetOrCreateInstituteAsync(instituteName);
                    program.InstituteId = institute.Id;
                }
                else
                {
                    program.InstituteId = null; // Allow clearing the institute
                }

                await _programRepository.UpdateProgramAsync(program);
                _logger.LogInformation("Program {ProgramName} with ID {ProgramId} updated with institute ID {InstituteId}.",
                    program.Name, program.Id, program.InstituteId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update program {ProgramName} with ID {ProgramId}.", program.Name, program.Id);
                throw;
            }
        }

        public async Task DeleteProgram(int id)
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
                    await _fileService.DeleteProgramFileAsync(program.ProgramDocumentId.Value);
                }

                await _programRepository.DeleteProgramAsync(id);
                _logger.LogInformation("Program with ID {ProgramId} and associated file (if any) deleted.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete program with ID {ProgramId}.", id);
                throw;
            }
        }

        private ProgramModelDto MapToDto(ProgramModel program)
        {
            return new ProgramModelDto
            {
                Id = program.Id,
                Degree = program.Degree,
                Name = program.Name,
                NameCode = program.NameCode,
                FieldOfStudy = program.FieldOfStudy,
                Speciality = program.Speciality,
                Form = program.Form,
                Objects = program.Objects,
                Directions = program.Directions,
                Descriptions = program.Descriptions,
                Purpose = program.Purpose,
                Years = program.Years,
                Credits = program.Credits,
                ProgramCharacteristics = program.ProgramCharacteristics,
                ProgramCompetence = program.ProgramCompetence,
                Results = program.Results,
                LinkFaculty = program.LinkFaculty,
                ProgramDocumentId = program.ProgramDocumentId,
                ProgramDocument = program.ProgramDocument,
                Components = program.Components,
                Jobs = program.Jobs,
                Accredited = program.Accredited,
                Institute = program.Institute?.Name
            };
        }

    }
}
