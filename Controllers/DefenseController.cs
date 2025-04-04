﻿using System;
using Microsoft.AspNetCore.Mvc;
using OntuPhdApi.Services.Defense;


namespace OntuPhdApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DefenseController : ControllerBase
    {
        private readonly IDefenseService _defenseService;

        public DefenseController(IDefenseService defenseService)
        {
            _defenseService = defenseService;
        }

        [HttpGet]
        public IActionResult GetDefenses()
        {
            try
            {
                var defenses = _defenseService.GetDefenses();
                defenses = defenses
                .OrderByDescending(r => r.DateOfPublication)
                .ToList();
                return Ok(defenses);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("{id}")]
        public IActionResult GetDefenseById(int id)
        {
            try
            {
                var defense = _defenseService.GetDefenseById(id);
                if (defense == null)
                {
                    return NotFound($"News with ID {id} not found.");
                }
                return Ok(defense);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("degree")]
        public IActionResult GetProgramsDegrees([FromQuery] string? degree)
        {
            try
            {
                var defensesDegrees = _defenseService.GetDefensesByDegree(degree);
                defensesDegrees = defensesDegrees
                .OrderByDescending(r => r.DateOfPublication)
                .ToList();
                return Ok(defensesDegrees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
