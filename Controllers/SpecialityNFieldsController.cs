using Microsoft.AspNetCore.Mvc;
using OntuPhdApi.Models.Programs;
using OntuPhdApi.Services;
using OntuPhdApi.Services.News;

namespace OntuPhdApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecialityNFieldsController : ControllerBase
    {
        private readonly ISpecialityNFieldsService _specNFieldsService;

        public SpecialityNFieldsController(ISpecialityNFieldsService specNFieldsService, IWebHostEnvironment environment)
        {
            _specNFieldsService = specNFieldsService;
            _environment = environment;
        }
        private readonly IWebHostEnvironment _environment;


        [HttpGet]
        public ActionResult<List<FieldOfStudyDto>> GetFieldsWithSpecialities([FromQuery] string? degree)
        {
            try
            {
                var fields = _specNFieldsService.GetSpecialitiesNFields(degree);
                return Ok(fields);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{code}")]
        public IActionResult GetSpecialitiesByCode(string code)
        {
            try
            {
                var specs = _specNFieldsService.GetSpecialitiesByCode(code);
                if (specs == null)
                {
                    return NotFound($"Specs with code {code} not found.");
                }
                return Ok(specs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
