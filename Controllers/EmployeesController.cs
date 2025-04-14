using Microsoft.AspNetCore.Mvc;
using OntuPhdApi.Models;
using OntuPhdApi.Models.Employees;
using OntuPhdApi.Services;
using OntuPhdApi.Services.Employees;

namespace OntuPhdApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesService _employeesService;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(IEmployeesService employeesService, ILogger<EmployeesController> logger)
        {
            _employeesService = employeesService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            _logger.LogInformation("Fetching all employees.");
            try
            {
                var employees = await _employeesService.GetEmployeesAsync();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch employees.");
                return StatusCode(500, "An error occurred while retrieving employees.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            _logger.LogInformation("Fetching employee with ID {EmployeeId}.", id);
            try
            {
                var employee = await _employeesService.GetEmployeeByIdAsync(id);
                if (employee == null)
                {
                    _logger.LogWarning("Employee with ID {EmployeeId} not found.", id);
                    return NotFound("Employee not found.");
                }
                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch employee with ID {EmployeeId}.", id);
                return StatusCode(500, "An error occurred while retrieving the employee.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromForm] EmployeeCreateUpdateDto employeeDto)
        {
            _logger.LogInformation("Adding new employee with name {EmployeeName}.", employeeDto.Name);
            try
            {
                await _employeesService.AddEmployeeAsync(employeeDto);
                return CreatedAtAction(nameof(GetEmployee), new { id = 0 }, employeeDto); // ID будет известен после добавления, если нужно
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid employee data: {ErrorMessage}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add employee with name {EmployeeName}.", employeeDto.Name);
                return StatusCode(500, "An error occurred while adding the employee.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromForm] EmployeeCreateUpdateDto employeeDto)
        {
            _logger.LogInformation("Updating employee with ID {EmployeeId}.", id);
            try
            {
                await _employeesService.UpdateEmployeeAsync(id, employeeDto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid employee data: {ErrorMessage}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Employee with ID {EmployeeId} not found.", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update employee with ID {EmployeeId}.", id);
                return StatusCode(500, "An error occurred while updating the employee.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            _logger.LogInformation("Deleting employee with ID {EmployeeId}.", id);
            try
            {
                await _employeesService.DeleteEmployeeAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Employee with ID {EmployeeId} not found for deletion.", id);
                return StatusCode(400, $"Employee with ID {id} not found for deletion.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete employee with ID {EmployeeId}.", id);
                return StatusCode(500, "An error occurred while updating the employee.");
            }
        }


    }
}