using Microsoft.AspNetCore.Mvc;
using OntuPhdApi.Models.Programs;
using OntuPhdApi.Models.Programs.Components;
using OntuPhdApi.Services.News;
using OntuPhdApi.Services.SpecAndFields;

namespace OntuPhdApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecialityNFieldsController : ControllerBase
    {
        private readonly ISpecialityNFieldsService _specNFieldsService;

        public SpecialityNFieldsController(ISpecialityNFieldsService specNFieldsService)
        {
            _specNFieldsService = specNFieldsService;
        }


        [HttpGet]
        public async Task<ActionResult<List<FieldOfStudyDto>>> GetFieldsWithSpecialities([FromQuery] string? degree)
        {
            try
            {
                var fields = await _specNFieldsService.GetFieldsWithSpecialitiesAsync(degree);
                return Ok(fields);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetSpecialitiesByCode(string code)
        {
            try
            {
                var specialities = await _specNFieldsService.GetSpecialitiesByCodeAsync(code);
                return Ok(specialities);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
