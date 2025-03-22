using Microsoft.AspNetCore.Mvc;
using OntuPhdApi.Models;
using OntuPhdApi.Services;

namespace OntuPhdApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // URL будет /api/doctors
    public class DoctorsController : ControllerBase
    {
        private readonly DatabaseService _dbService;

        public DoctorsController(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet] // GET /api/doctors
        public IActionResult GetDoctors()
        {
            var doctors = _dbService.GetDoctors();
            return Ok(doctors); // Возвращает JSON
        }


        [HttpPost] // POST /api/doctors
        public IActionResult AddDoctor([FromBody] Doctor doctor)
        {
            if (doctor == null || string.IsNullOrEmpty(doctor.Name) || string.IsNullOrEmpty(doctor.Degree))
            {
                return BadRequest("Некорректные данные");
            }

            _dbService.AddDoctor(doctor);
            return CreatedAtAction(nameof(GetDoctors), new { id = doctor.Id }, doctor);
        }

    }
}
