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
        public ActionResult<List<FieldOfStudyDto>> GetFieldsWithSpecialities()
        {
            try
            {
                var news = _specNFieldsService.GetSpecialitiesNFields();
                return Ok(news);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
