using Microsoft.AspNetCore.Mvc;
using OntuPhdApi.Models.Roadmap;
using OntuPhdApi.Services;
using OntuPhdApi.Services.Roadmap;

namespace OntuPhdApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoadmapsController : ControllerBase
    {
        private readonly IRoadmapService _service;

        public RoadmapsController(IRoadmapService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoadmaps([FromQuery] string? type)
        {
            var result = await _service.GetAllAsync(type);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoadmap(int id)
        {
            var roadmap = await _service.GetByIdAsync(id);
            return roadmap is null ? NotFound() : Ok(roadmap);
        }

        [HttpPost]
        public async Task<IActionResult> AddRoadmap([FromBody] RoadmapModelDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Type) || string.IsNullOrWhiteSpace(dto.Description))
                return BadRequest("Type and Description are required.");

            var result = await _service.AddAsync(dto);
            return CreatedAtAction(nameof(GetRoadmap), new { id = result.Id }, result);
        }

    }
}