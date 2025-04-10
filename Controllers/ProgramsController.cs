using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OntuPhdApi.Models.Programs;
using OntuPhdApi.Services.Programs;
using System;
using System.IO;
using System.Threading.Tasks;

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
        private readonly ILogger<ProgramsController> _logger;
        private readonly string _uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Files/Uploads/Programs");

        public ProgramsController(IProgramService programService, ILogger<ProgramsController> logger)
        {
            _programService = programService ?? throw new ArgumentNullException(nameof(programService));
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
                string? filePath = null;
                string? contentType = null;
                long fileSize = 0;

                if (request.File != null && request.File.Length > 0)
                {
                    (filePath, contentType, fileSize) = await SaveFileAsync(request.File);
                }

                await _programService.AddProgram(program, filePath, contentType, fileSize);
                _logger.LogInformation("Program {ProgramName} added with ID {ProgramId}.", program.Name, program.Id);
                return CreatedAtAction(nameof(GetProgram), new { id = program.Id }, program);
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
                if (request.File != null && request.File.Length > 0)
                {
                    var (filePath, contentType, fileSize) = await SaveFileAsync(request.File);
                    await _programService.UpdateProgramWithDocument(existingProgram, filePath, request.File.FileName, contentType, fileSize);
                }
                else
                {
                    await _programService.UpdateProgram(existingProgram);
                }

                _logger.LogInformation("Program with ID {ProgramId} updated successfully.", id);
                return Ok(existingProgram);
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
                await _programService.DeleteProgram(id);
                _logger.LogInformation("Program with ID {ProgramId} deleted successfully.", id);
                return NoContent();
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

        // Вспомогательные методы
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

        private ProgramModel MapToProgramModel(ProgramRequestDto request)
        {
            return new ProgramModel
            {
                Degree = request.Degree,
                Name = request.Name,
                NameCode = request.NameCode,
                FieldOfStudy = request.FieldOfStudy != null
                    ? new FieldOfStudy { Code = request.FieldOfStudy.Code, Name = request.FieldOfStudy.Name }
                    : null,
                Speciality = request.Speciality != null
                    ? new Speciality { Code = request.Speciality.Code, Name = request.Speciality.Name, FieldCode = request.Speciality.FieldCode }
                    : null,
                Form = request.Form,
                Objects = request.Objects,
                Directions = request.Directions,
                Descriptions = request.Descriptions,
                Purpose = request.Purpose,
                Years = request.Years,
                Credits = request.Credits,
                ProgramCharacteristics = request.ProgramCharacteristics != null
                    ? new ProgramCharacteristics
                    {
                        Area = request.ProgramCharacteristics.Area != null
                            ? new Area
                            {
                                Object = request.ProgramCharacteristics.Area.Object,
                                Aim = request.ProgramCharacteristics.Area.Aim,
                                Theory = request.ProgramCharacteristics.Area.Theory,
                                Methods = request.ProgramCharacteristics.Area.Methods,
                                Instruments = request.ProgramCharacteristics.Area.Instruments
                            }
                            : null,
                        Focus = request.ProgramCharacteristics.Focus,
                        Features = request.ProgramCharacteristics.Features
                    }
                    : null,
                ProgramCompetence = request.ProgramCompetence != null
                    ? new ProgramCompetence
                    {
                        OverallCompetence = request.ProgramCompetence.OverallCompetence,
                        SpecialCompetence = request.ProgramCompetence.SpecialCompetence,
                        IntegralCompetence = request.ProgramCompetence.IntegralCompetence
                    }
                    : null,
                Results = request.Results,
                LinkFaculty = request.LinkFaculty,
                Components = request.Components,
                Jobs = request.Jobs,
                Accredited = request.Accredited,
                ProgramDocumentId = 0 // Временно, будет заполнено сервисом
            };
        }

        private void UpdateProgramModel(ProgramModel program, ProgramRequestDto request)
        {
            program.Degree = request.Degree;
            program.Name = request.Name;
            program.NameCode = request.NameCode;
            program.FieldOfStudy = request.FieldOfStudy;
            program.Speciality = request.Speciality;
            program.Form = request.Form;
            program.Objects = request.Objects;
            program.Directions = request.Directions;
            program.Descriptions = request.Descriptions;
            program.Purpose = request.Purpose;
            program.Years = request.Years;
            program.Credits = request.Credits;
            program.Results = request.Results;
            program.LinkFaculty = request.LinkFaculty;
            program.Components = request.Components;
            program.Jobs = request.Jobs;
            program.Accredited = request.Accredited;
            program.ProgramCharacteristics = request.ProgramCharacteristics != null
                ? new ProgramCharacteristics
                {
                    Area = request.ProgramCharacteristics.Area != null
                        ? new Area
                        {
                            Object = request.ProgramCharacteristics.Area.Object,
                            Aim = request.ProgramCharacteristics.Area.Aim,
                            Theory = request.ProgramCharacteristics.Area.Theory,
                            Methods = request.ProgramCharacteristics.Area.Methods,
                            Instruments = request.ProgramCharacteristics.Area.Instruments
                        }
                        : null,
                    Focus = request.ProgramCharacteristics.Focus,
                    Features = request.ProgramCharacteristics.Features
                }
                : null;
            program.ProgramCompetence = request.ProgramCompetence != null
                ? new ProgramCompetence
                {
                    OverallCompetence = request.ProgramCompetence.OverallCompetence,
                    SpecialCompetence = request.ProgramCompetence.SpecialCompetence,
                    IntegralCompetence = request.ProgramCompetence.IntegralCompetence
                }
                : null;
        }

        private async Task<(string FilePath, string ContentType, long FileSize)> SaveFileAsync(IFormFile file)
        {
            if (!Directory.Exists(_uploadFolder))
            {
                Directory.CreateDirectory(_uploadFolder);
            }

            var originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
            var extension = Path.GetExtension(file.FileName);
            var uniqueSuffix = DateTime.Now.ToString("yyyyMMddHHmmss");
            var fileName = $"{originalFileName}_{uniqueSuffix}{extension}";
            var filePath = Path.Combine(_uploadFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return (filePath, file.ContentType, file.Length);
        }
    }
}