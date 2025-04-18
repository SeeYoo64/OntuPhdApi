﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Npgsql;
using OntuPhdApi.Models.Programs;
using OntuPhdApi.Services.Programs;
using OntuPhdApi.Services.Files;
using System;
using System.Threading.Tasks;
using OntuPhdApi.Data;
using OntuPhdApi.Models.Programs.Components;
using Microsoft.EntityFrameworkCore;
using OntuPhdApi.Models.Institutes;
using OntuPhdApi.Utilities.Mappers;

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
            ILogger<ProgramsController> logger
            )
        {
            _programService = programService ?? throw new ArgumentNullException(nameof(programService));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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


        [HttpGet("degrees")]
        public async Task<IActionResult> GetProgramsDegrees([FromQuery] DegreeType? degreeType)
        {
            string degree = degreeType.ToString();
            _logger.LogInformation("Fetching programs for degree {Degree}.", degree?.ToString() ?? "all");
            try
            {
                var programs = await _programService.GetProgramsByDegreeAsync(degree);
                return Ok(programs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch programs for degree {Degree}.", degree);
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
        public async Task<ActionResult<ProgramResponseDto>> CreateProgram(ProgramCreateDto programDto)
        {
            var createdProgram = await _programService.CreateProgramAsync(programDto);
            return CreatedAtAction(nameof(GetProgram), new { id = createdProgram.Id }, createdProgram);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProgram(int id, ProgramUpdateDto programDto)
        {
            var result = await _programService.UpdateProgramAsync(id, programDto);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
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