using Microsoft.AspNetCore.Mvc;
using OntuPhdApi.Models;
using OntuPhdApi.Services;

namespace OntuPhdApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoadmapsController : ControllerBase
    {
        private readonly DatabaseService _dbService;

        public RoadmapsController(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public IActionResult GetRoadmaps()
        {
            try
            {
                var roadmaps = _dbService.GetRoadmaps();
                return Ok(roadmaps);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetRoadmap(int id)
        {
            try
            {
                var roadmap = _dbService.GetRoadmapById(id);
                if (roadmap == null)
                {
                    return NotFound($"Roadmap with ID {id} not found.");
                }
                return Ok(roadmap);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult AddRoadmap([FromBody] Roadmap roadmap)
        {
            if (roadmap == null || string.IsNullOrEmpty(roadmap.Type) || string.IsNullOrEmpty(roadmap.Description))
            {
                return BadRequest("Invalid roadmap data. Type and Description are required.");
            }

            try
            {
                _dbService.AddRoadmap(roadmap);
                return CreatedAtAction(nameof(GetRoadmap), new { id = roadmap.Id }, roadmap);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}