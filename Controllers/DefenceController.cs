using Microsoft.AspNetCore.Mvc;
using OntuPhdApi.Services.Defense;


namespace OntuPhdApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DefenceController : ControllerBase
    {
        private readonly IDefenseService _defenseService;

        public DefenceController(IDefenseService defenseService)
        {
            _defenseService = defenseService;
        }

        [HttpGet]
        public IActionResult GetDefenses()
        {
            try
            {
                var defenses = _defenseService.GetDefenses();

                return Ok(defenses);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
