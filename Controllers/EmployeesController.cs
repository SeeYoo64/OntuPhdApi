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
        private readonly IEmployeesService _employeeService;
        private readonly IWebHostEnvironment _environment;

        public EmployeesController(IEmployeesService employeeService, IWebHostEnvironment environment)
        {
            _employeeService = employeeService;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult GetEmployees()
        {
            try
            {
                var employees = _employeeService.GetEmployees();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetEmployee(int id)
        {
            try
            {
                var employee = _employeeService.GetEmployeeById(id);
                if (employee == null)
                {
                    return NotFound($"Employee with ID {id} not found.");
                }
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromForm] EmployeesModelDto employeeDto)
        {
            if (employeeDto == null ||
                string.IsNullOrEmpty(employeeDto.Name) ||
                string.IsNullOrEmpty(employeeDto.Position) ||
                employeeDto.Photo == null)
            {
                return BadRequest("Invalid employee data. Name, Position, and Photo are required.");
            }

            try
            {
                // Создаём объект Employee
                var employee = new EmployeesModel
                {
                    Name = employeeDto.Name,
                    Position = employeeDto.Position,
                    Photo = "" // Временное значение, обновим после загрузки файла
                };

                // Сохраняем сотрудника в базе, чтобы получить Id
                _employeeService.AddEmployee(employee);

                // Создаём директорию для файла сотрудника
                var employeeDir = Path.Combine(_environment.ContentRootPath, "Files", "Uploads", "Employees", employee.Id.ToString());
                Directory.CreateDirectory(employeeDir);

                // Сохраняем Photo
                var photoExtension = Path.GetExtension(employeeDto.Photo.FileName);
                var photoPath = Path.Combine(employeeDir, $"photo{photoExtension}");
                using (var stream = new FileStream(photoPath, FileMode.Create))
                {
                    await employeeDto.Photo.CopyToAsync(stream);
                }
                employee.Photo = Path.Combine("Files", "Uploads", "Employees", employee.Id.ToString(), $"photo{photoExtension}").Replace("\\", "/");

                // Обновляем запись в базе с путём к файлу
                _employeeService.UpdateEmployee(employee);

                return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("download")]
        public IActionResult DownloadFile([FromQuery] string filePath)
        {
            try
            {
                // Проверяем, что путь начинается с "Files/Uploads/Employees"
                if (string.IsNullOrEmpty(filePath) || !filePath.StartsWith("Files/Uploads/Employees"))
                {
                    return BadRequest("Invalid file path.");
                }

                var fullPath = Path.Combine(_environment.ContentRootPath, filePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
                if (!System.IO.File.Exists(fullPath))
                {
                    return NotFound($"File {filePath} not found.");
                }

                var fileBytes = System.IO.File.ReadAllBytes(fullPath);
                var fileName = Path.GetFileName(fullPath);
                return File(fileBytes, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}