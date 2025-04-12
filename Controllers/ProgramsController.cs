using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Npgsql;
using OntuPhdApi.Models.Programs;
using OntuPhdApi.Services.Programs;
using OntuPhdApi.Services.Files;
using System;
using System.Threading.Tasks;
using OntuPhdApi.Data;

namespace OntuPhdApi.Controllers
{
    public enum DegreeType
    {
        phd,
        doc
    }

    [ApiController]
    [Route("api/[controller]")]
    public class ProgramsController : ControllerBase
    {
        private readonly IProgramService _programService;
        private readonly IProgramFileService _fileService;
        private readonly AppDbContext _context;
        private readonly ILogger<ProgramsController> _logger;

        public ProgramsController(
            IProgramService programService,
            IProgramFileService fileService,
            AppDbContext context,
            ILogger<ProgramsController> logger)
        {
            _programService = programService ?? throw new ArgumentNullException(nameof(programService));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> GetPrograms()
        {
            _logger.LogInformation("Fetching all programs.");
            try
            {
                var programs = await _programService.GetPrograms();
                return Ok(programs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch programs.");
                return StatusCode(500, "An error occurred while retrieving programs.");
            }
        }


        [HttpGet("degrees")]
        public async Task<IActionResult> GetProgramsDegrees([FromQuery] DegreeType? degree)
        {
            _logger.LogInformation("Fetching programs for degree {Degree}.", degree?.ToString() ?? "all");
            try
            {
                var programsDegrees = await _programService.GetProgramsDegrees(degree);
                return Ok(programsDegrees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch programs for degree {Degree}.", degree);
                return StatusCode(500, "An error occurred while retrieving programs by degree.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProgram(int id)
        {
            _logger.LogInformation("Fetching program with ID {ProgramId}.", id);
            try
            {
                var program = await _programService.GetProgram(id);
                if (program == null)
                {
                    _logger.LogWarning("Program with ID {ProgramId} not found.", id);
                    return NotFound("Program not found.");
                }
                return Ok(program);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch program with ID {ProgramId}.", id);
                return StatusCode(500, "An error occurred while retrieving the program.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProgram([FromForm] ProgramRequestDto request)
        {
            _logger.LogInformation("Adding new program with name {ProgramName}.", request.Name);
            try
            {
                if (!IsValidProgramRequest(request, out string errorMessage))
                {
                    _logger.LogWarning("Invalid program data: {ErrorMessage}.", errorMessage);
                    return BadRequest(errorMessage);
                }

                var program = MapToProgramModel(request);
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        if (request.File != null && request.File.Length > 0)
                        {
                            var (filePath, contentType, fileSize, documentId) = await _fileService.SaveProgramFileFromFormAsync(program.Name, request.File);
                            program.ProgramDocumentId = documentId;
                            await _programService.AddProgram(program, filePath, contentType, fileSize);
                        }
                        else
                        {
                            await _programService.AddProgram(program, null, null, 0);
                        }

                        await transaction.CommitAsync();
                        _logger.LogInformation("Program {ProgramName} added with ID {ProgramId}.", program.Name, program.Id);
                        return CreatedAtAction(nameof(GetProgram), new { id = program.Id }, program);
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        _logger.LogError(ex, "Failed to add program {ProgramName}. Rolling back transaction.", request.Name);
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add program {ProgramName}.", request.Name);
                return StatusCode(500, "An error occurred while adding the program.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProgram(int id, [FromForm] ProgramRequestDto request)
        {
            _logger.LogInformation("Updating program with ID {ProgramId}.", id);
            try
            {
                var existingProgram = await _programService.GetProgram(id);
                if (existingProgram == null)
                {
                    _logger.LogWarning("Program with ID {ProgramId} not found.", id);
                    return NotFound("Program not found.");
                }

                UpdateProgramModel(existingProgram, request);
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        if (request.File != null && request.File.Length > 0)
                        {
                            var (filePath, contentType, fileSize, documentId) = await _fileService.SaveProgramFileFromFormAsync(existingProgram.Name, request.File);
                            existingProgram.ProgramDocumentId = documentId;
                            await _programService.UpdateProgramWithDocument(existingProgram, filePath, request.File.FileName, contentType, fileSize);
                        }
                        else
                        {
                            // Если файл не передан, оставляем ProgramDocumentId как есть или очищаем
                            existingProgram.ProgramDocumentId = existingProgram.ProgramDocumentId; // Оставляем как есть
                            await _programService.UpdateProgram(existingProgram);
                        }

                        await transaction.CommitAsync();
                        _logger.LogInformation("Program with ID {ProgramId} updated successfully.", id);
                        return Ok(existingProgram);
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        _logger.LogError(ex, "Failed to update program with ID {ProgramId}. Rolling back transaction.", id);
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update program with ID {ProgramId}.", id);
                return StatusCode(500, "An error occurred while updating the program.");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProgram(int id)
        {
            _logger.LogInformation("Deleting program with ID {ProgramId}.", id);
            try
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await _programService.DeleteProgram(id);
                        await transaction.CommitAsync();
                        _logger.LogInformation("Program with ID {ProgramId} deleted successfully.", id);
                        return NoContent();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        _logger.LogError(ex, "Failed to delete program with ID {ProgramId}. Rolling back transaction.", id);
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found"))
                {
                    _logger.LogWarning("Program with ID {ProgramId} not found for deletion.", id);
                    return NotFound("Program not found.");
                }
                _logger.LogError(ex, "Failed to delete program with ID {ProgramId}.", id);
                return StatusCode(500, "An error occurred while deleting the program.");
            }
        }

        // Additional methods
        private ProgramModel MapToProgramModel(ProgramRequestDto request)
        {
            return new ProgramModel
            {
                Degree = request.Degree,
                Name = request.Name,
                NameCode = request.NameCode,
                FieldOfStudy = IsEmptyFieldOfStudy(request.FieldOfStudy) ? null : request.FieldOfStudy,
                Speciality = IsEmptySpeciality(request.Speciality) ? null : request.Speciality,
                Form = request.Form,
                Objects = request.Objects,
                Directions = request.Directions,
                Descriptions = request.Descriptions,
                Purpose = request.Purpose,
                Years = request.Years,
                Credits = request.Credits,
                ProgramCharacteristics = IsEmptyProgramCharacteristics(request.ProgramCharacteristics) ? null : request.ProgramCharacteristics,
                ProgramCompetence = IsEmptyProgramCompetence(request.ProgramCompetence) ? null : new ProgramCompetence
                {
                    OverallCompetence = request.ProgramCompetence?.OverallCompetence,
                    SpecialCompetence = request.ProgramCompetence?.SpecialCompetence,
                    IntegralCompetence = request.ProgramCompetence?.IntegralCompetence
                },
                Results = request.Results,
                LinkFaculty = request.LinkFaculty,
                Components = request.Components,
                Jobs = request.Jobs,
                Accredited = request.Accredited,
                ProgramDocumentId = 0
            };
        }

        private bool IsValidProgramRequest(ProgramRequestDto request, out string errorMessage)
        {
            if (string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Degree))
            {
                errorMessage = "Invalid program data. Name and Degree are required.";
                return false;
            }
            errorMessage = null;
            return true;
        }

        private bool IsEmptyFieldOfStudy(FieldOfStudy? fieldOfStudy)
        {
            return fieldOfStudy == null ||
                   (string.IsNullOrEmpty(fieldOfStudy.Name) && string.IsNullOrEmpty(fieldOfStudy.Code));
        }

        private bool IsEmptySpeciality(Speciality? speciality)
        {
            return speciality == null ||
                   (string.IsNullOrEmpty(speciality.Name) &&
                    string.IsNullOrEmpty(speciality.Code) &&
                    string.IsNullOrEmpty(speciality.FieldCode));
        }

        private bool IsEmptyArea(Area? area)
        {
            return area == null ||
                   (string.IsNullOrEmpty(area.Aim) &&
                    string.IsNullOrEmpty(area.Object) &&
                    string.IsNullOrEmpty(area.Theory) &&
                    string.IsNullOrEmpty(area.Methods) &&
                    string.IsNullOrEmpty(area.Instruments));
        }

        private bool IsEmptyProgramCharacteristics(ProgramCharacteristics? programCharacteristics)
        {
            return programCharacteristics == null ||
                   (IsEmptyArea(programCharacteristics.Area) &&
                    string.IsNullOrEmpty(programCharacteristics.Focus) &&
                    string.IsNullOrEmpty(programCharacteristics.Features));
        }

        private bool IsEmptyProgramCompetence(ProgramCompetence? programCompetence)
        {
            return programCompetence == null ||
                   (string.IsNullOrEmpty(programCompetence.IntegralCompetence) &&
                    (programCompetence.OverallCompetence == null || !programCompetence.OverallCompetence.Any()) &&
                    (programCompetence.SpecialCompetence == null || !programCompetence.SpecialCompetence.Any()));
        }

        private void UpdateProgramModel(ProgramModel program, ProgramRequestDto request)
        {
            program.Degree = request.Degree;
            program.Name = request.Name;
            program.NameCode = request.NameCode ?? null;
            program.FieldOfStudy = IsEmptyFieldOfStudy(request.FieldOfStudy) ? null : request.FieldOfStudy;
            program.Speciality = IsEmptySpeciality(request.Speciality) ? null : request.Speciality;
            program.Form = request.Form ?? null;
            program.Objects = request.Objects ?? null;
            program.Directions = request.Directions ?? null;
            program.Descriptions = request.Descriptions ?? null;
            program.Purpose = request.Purpose ?? null;
            program.Years = request.Years ?? null;
            program.Credits = request.Credits ?? null;
            program.Results = request.Results ?? null;
            program.LinkFaculty = request.LinkFaculty ?? null;
            program.Accredited = request.Accredited;

            program.ProgramCharacteristics = IsEmptyProgramCharacteristics(request.ProgramCharacteristics) ? null : request.ProgramCharacteristics;
            program.ProgramCompetence = IsEmptyProgramCompetence(request.ProgramCompetence) ? null : new ProgramCompetence
            {
                OverallCompetence = request.ProgramCompetence?.OverallCompetence,
                SpecialCompetence = request.ProgramCompetence?.SpecialCompetence,
                IntegralCompetence = request.ProgramCompetence?.IntegralCompetence
            };
        }


    }
}