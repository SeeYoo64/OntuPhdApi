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
        public IActionResult GetRoadmaps([FromQuery] string? type)
        {
            try
            {
                List<Roadmap> roadmaps;
                if (!string.IsNullOrEmpty(type))
                {
                    roadmaps = _dbService.GetRoadmapsByType(type);
                }
                else
                {
                    roadmaps = _dbService.GetRoadmaps();
                }

                // Сортировка по Status: Completed -> Ontime -> NotStarted, затем по DataStart
                roadmaps = roadmaps
                    .OrderBy(r => r.Status switch
                    {
                        RoadmapStatus.Completed => 1,
                        RoadmapStatus.Ontime => 2,
                        RoadmapStatus.NotStarted => 3,
                        _ => 4
                    })
                    .ThenBy(r => r.DataStart)
                    .ToList();

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