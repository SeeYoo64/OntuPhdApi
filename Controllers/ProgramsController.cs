using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Npgsql;
using OntuPhdApi.Models.Programs;
using OntuPhdApi.Services.Programs;
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
        private readonly ILogger<ProgramsController> _logger;

        public ProgramsController(IProgramService programService, ILogger<ProgramsController> logger)
        {
            _programService = programService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPrograms()
        {
            _logger.LogInformation("Fetching all programs");
            try
            {
                var programs = await _programService.GetAllProgramsAsync();
                return Ok(programs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch programs");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProgramById(int id)
        {
            _logger.LogInformation("Fetching program with ID: {ProgramId}", id);
            try
            {
                var program = await _programService.GetProgramByIdAsync(id);
                return Ok(program);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Program with ID {ProgramId} not found", id);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch program with ID: {ProgramId}", id);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }


        [HttpGet("degrees")]
        public async Task<IActionResult> GetProgramsDegrees([FromQuery] DegreeType? degreeType)
        {
            string degree = degreeType.ToString();
            _logger.LogInformation("Fetching programs with DegreeType: {DegreeType}", degreeType);
            try
            {
                var programs = await _programService.GetProgramsByDegreeAsync(degree);
                return Ok(programs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch programs with DegreeType: {DegreeType}", degreeType);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddProgram([FromForm] ProgramCreateUpdateDto programDto)
        {
            _logger.LogInformation("Adding new program with name: {ProgramName}", programDto.Name);
            try
            {
                await _programService.AddProgramAsync(programDto);
                return CreatedAtAction(nameof(GetProgramById), new { id = 0 }, programDto);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid program data: {ErrorMessage}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add program with name: {ProgramName}", programDto.Name);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProgram(int id, [FromForm] ProgramCreateUpdateDto programDto)
        {
            _logger.LogInformation("Updating program with ID: {ProgramId}", id);
            try
            {
                await _programService.UpdateProgramAsync(id, programDto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid program data: {ErrorMessage}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Program with ID {ProgramId} not found", id);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update program with ID: {ProgramId}", id);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProgram(int id)
        {
            _logger.LogInformation("Deleting program with ID: {ProgramId}", id);
            try
            {
                await _programService.DeleteProgramAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Program with ID {ProgramId} not found", id);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete program with ID: {ProgramId}", id);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }


    }
}