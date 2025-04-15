using System;
using Microsoft.AspNetCore.Mvc;
using OntuPhdApi.Models.Defense;
using OntuPhdApi.Services.Defense;


namespace OntuPhdApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DefensesController : ControllerBase
    {
        private readonly IDefenseService _defenseService;
        private readonly ILogger<DefensesController> _logger;

        public DefensesController(IDefenseService defenseService, ILogger<DefensesController> logger)
        {
            _defenseService = defenseService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetDefenses()
        {
            _logger.LogInformation("Fetching all defenses.");
            try
            {
                var defenses = await _defenseService.GetDefensesAsync();
                return Ok(defenses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch defenses.");
                return StatusCode(500, "An error occurred while retrieving defenses.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDefense(int id)
        {
            _logger.LogInformation("Fetching defense with ID {DefenseId}.", id);
            try
            {
                var defense = await _defenseService.GetDefenseByIdAsync(id);
                if (defense == null)
                {
                    _logger.LogWarning("Defense with ID {DefenseId} not found.", id);
                    return NotFound("Defense not found.");
                }
                return Ok(defense);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch defense with ID {DefenseId}.", id);
                return StatusCode(500, "An error occurred while retrieving the defense.");
            }
        }


        [HttpGet("degree/{degree}")]
        public async Task<IActionResult> GetDefensesByDegree(string degree)
        {
            _logger.LogInformation("Fetching defenses for degree {Degree}.", degree);
            try
            {
                var defenses = await _defenseService.GetDefensesByDegreeAsync(degree);
                return Ok(defenses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch defenses for degree {Degree}.", degree);
                return StatusCode(500, "An error occurred while retrieving defenses by degree.");
            }
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDefense(int id)
        {
            _logger.LogInformation("Deleting defense with ID {DefenseId}.", id);
            try
            {
                await _defenseService.DeleteDefenseAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Defense with ID {DefenseId} not found for deletion.", id);
                return NotFound("Defense not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete defense with ID {id}.");
                return StatusCode(500, "An error occurred while .");
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddDefense([FromBody] DefenseCreateDto defenseDto)
        {
            _logger.LogInformation("Adding new defense with title {DefenseTitle}.", defenseDto.DefenseTitle);
            try
            {
                var defenseId = await _defenseService.AddDefenseAsync(defenseDto);
                return CreatedAtAction(
                    nameof(GetDefense),
                    new { id = defenseId },
                    defenseDto
                );
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid defense data: {ErrorMessage}", ex.Message);
                return StatusCode(404, "Bad request.");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Program not found: {ErrorMessage}", ex.Message);
                return StatusCode(404, "Program not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add defense with title {DefenseTitle}.", defenseDto.DefenseTitle);
                return StatusCode(500, "An error occurred while .");
            }
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDefense(int id, [FromBody] DefenseCreateDto defenseDto)
        {
            _logger.LogInformation("Updating defense with ID {DefenseId}.", id);
            try
            {
                await _defenseService.UpdateDefenseAsync(id, defenseDto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid defense data: {ErrorMessage}", ex.Message);
                return NotFound("Bad request.");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Defense or Program not found: {ErrorMessage}", ex.Message);
                return NotFound("Defense not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update defense with ID {DefenseId}.", id);
                return StatusCode(500, "An error occurred while .");
            }
        }


    }
}
