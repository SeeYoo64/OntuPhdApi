using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OntuPhdApi.Data;

namespace OntuPhdApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {

        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var stats = new
            {
                ProgramsCount = await _context.Programs.CountAsync(),
                EmployeesCount = await _context.Employees.CountAsync(),
                DefensesCount = await _context.Defenses.CountAsync()

            };

            return Ok(stats);
        }

    }
}
