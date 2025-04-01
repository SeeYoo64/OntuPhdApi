using System;
using Microsoft.AspNetCore.Mvc;
using OntuPhdApi.Models.Programs;
using OntuPhdApi.Services;
using OntuPhdApi.Services.Programs;

namespace OntuPhdApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgramsController : ControllerBase
    {
        private readonly IProgramService _programService;

        public ProgramsController(IProgramService programService)
        {
            _programService = programService;
        }

        [HttpGet]
        public IActionResult GetPrograms([FromQuery] string? Degree)
        {
            try
            {
                var programs = _programService.GetPrograms();

                // Сортировка по Degrees - phd -> everything else
                programs = programs
                    .OrderBy(r => r.Degree switch {
                        "phd" => 1,
                        _ => 2
                    })
                    .ThenBy(r => r.Id)
                    .ToList();

                return Ok(programs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("fields")]
        public IActionResult GetProgramsFields()
        {
            try
            {
                var programsFields = _programService.GetProgramsFields();
                return Ok(programsFields);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("degrees")]
        public IActionResult GetProgramsDegrees([FromQuery] string? degree)
        {
            try
            {
                var programsDegrees = _programService.GetProgramsDegrees(degree);
                return Ok(programsDegrees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("{id}")]
        public IActionResult GetProgram(int id)
        {
            try
            {
                var program = _programService.GetProgramById(id);
                if (program == null)
                {
                    return NotFound($"Program with ID {id} not found.");
                }
                return Ok(program);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult AddProgram([FromBody] ProgramView program)
        {
            if (program == null || string.IsNullOrEmpty(program.Name))
            {
                return BadRequest("Invalid program data. Name is required.");
            }

            try
            {
                _programService.AddProgram(program);
                return CreatedAtAction(nameof(GetProgram), new { id = program.Id }, program);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}