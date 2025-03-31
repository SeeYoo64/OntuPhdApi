using System;
using Microsoft.AspNetCore.Mvc;
using OntuPhdApi.Models;
using OntuPhdApi.Services;

namespace OntuPhdApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgramsController(DatabaseService dbService) : ControllerBase
    {
        private readonly DatabaseService _dbService = dbService;

        [HttpGet]
        public IActionResult GetPrograms([FromQuery] string? Degree)
        {
            try
            {
                var programs = _dbService.GetPrograms();

                // Сортировка по Id
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
                var programsFields = _dbService.GetProgramsFields();
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
                var programsDegrees = _dbService.GetProgramsDegrees(degree);
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
                var program = _dbService.GetProgramById(id);
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
                _dbService.AddProgram(program);
                return CreatedAtAction(nameof(GetProgram), new { id = program.Id }, program);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}