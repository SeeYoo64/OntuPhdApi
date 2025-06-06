﻿using Microsoft.AspNetCore.Mvc;
using OntuPhdApi.Services.Programs;
using OntuPhdApi.Utilities.Mappers;
using OntuPhdApi.Models.Programs.Dto;

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
        private readonly IProgramMapper _mapper;

        public ProgramsController(
            IProgramService programService,
            ILogger<ProgramsController> logger,
            IProgramMapper mapper
            )
        {
            _programService = programService ?? throw new ArgumentNullException(nameof(programService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProgramResponseDto>>> GetAllPrograms()
        {
            _logger.LogInformation("Fetching all programs.");
            try
            {
                var programs = await _programService.GetAllProgramsAsync();
                return Ok(programs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch programs.");
                return StatusCode(500, "An error occurred while retrieving programs.");
            }
        }

        [HttpGet("short")]
        public async Task<ActionResult<ProgramShortDto>> GetShortByDegree([FromQuery] string degree = null)
        {
            _logger.LogInformation("Fetching all short programs.");
            try
            {
                var program = await _programService.GetShortByDegreeAsync(degree);
                if (program == null)
                {
                    return NotFound();
                }
                return Ok(program);
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
            string degreeType = degree.ToString();
            _logger.LogInformation("Fetching programs for degree {Degree}.", degree?.ToString() ?? "all");
            try
            {
                var programs = await _programService.GetProgramsByDegreeAsync(degreeType);
                return Ok(programs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch programs for degree {Degree}.", degreeType);
                return StatusCode(500, "An error occurred while retrieving programs by degree.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProgramResponseDto>> GetProgram(int id)
        {
            _logger.LogInformation("Fetching program with ID {ProgramId}.", id);
            try
            {
                var program = await _programService.GetProgramByIdAsync(id);
                if (program == null)
                {
                    return NotFound();
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
        public async Task<ActionResult<ProgramResponseDto>> CreateProgram(ProgramCreateUpdateDto programDto)
        {
            var createdProgram = await _programService.CreateProgramAsync(programDto);
            return CreatedAtAction(nameof(GetProgram), new { id = createdProgram.Id }, createdProgram);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProgram(int id, ProgramCreateUpdateDto programDto)
        {
            _logger.LogInformation("Updating program with ID {Id}", id);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for program update ID {Id}", id);
                return BadRequest(ModelState);
            }

            try
            {
                await _programService.UpdateProgramAsync(id, programDto);
                var updatedProgram = await _programService.GetProgramByIdAsync(id);
                if (updatedProgram == null)
                {
                    _logger.LogError("Failed to retrieve updated program with ID {Id}", id);
                    return StatusCode(500, new { error = "Failed to retrieve updated program" });
                }

                _logger.LogInformation("Program with ID {Id} updated successfully", id);
                return Ok(updatedProgram);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Program with ID {Id} not found: {Message}", id, ex.Message);
                return NotFound(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid input for program update ID {Id}: {Message}", id, ex.Message);
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating program with ID {Id}", id);
                return StatusCode(500, new { error = "An unexpected error occurred" });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProgram(int id)
        {
            var result = await _programService.DeleteProgramAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

       


    }
}