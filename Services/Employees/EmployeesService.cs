using Npgsql;
using OntuPhdApi.Models.Employees;
using OntuPhdApi.Repositories.Employee;
using OntuPhdApi.Services.Files;
using OntuPhdApi.Utilities.Mappers;

namespace OntuPhdApi.Services.Employees
{
    public class EmployeesService : IEmployeesService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IProgramFileService _fileService;
        private readonly ILogger<EmployeesService> _logger;
        private readonly string _employeesUploadsPath = Path.Combine("wwwroot", "Files", "Uploads", "Employees"); 

        public EmployeesService(
            IEmployeeRepository employeeRepository,
            IProgramFileService fileService,
            ILogger<EmployeesService> logger)
        {
            _employeeRepository = employeeRepository;
            _fileService = fileService;
            _logger = logger;

            // Создаем директорию для загрузок, если она не существует
            if (!Directory.Exists(_employeesUploadsPath))
            {
                Directory.CreateDirectory(_employeesUploadsPath);
            }
        }

        public async Task<List<EmployeeModelDto>> GetEmployeesAsync()
        {
            _logger.LogInformation("Fetching all employees.");
            try
            {
                var employees = await _employeeRepository.GetAllEmployeesAsync();
                return EmployeeMapper.ToDtoList(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch employees.");
                throw;
            }
        }


        public async Task<EmployeeModelDto> GetEmployeeByIdAsync(int id)
        {
            _logger.LogInformation("Fetching employee with ID {EmployeeId}.", id);
            try
            {
                var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
                if (employee == null)
                {
                    _logger.LogWarning("Employee with ID {EmployeeId} not found.", id);
                    return null;
                }
                return EmployeeMapper.ToDto(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch employee with ID {EmployeeId}.", id);
                throw;
            }
        }



        public async Task AddEmployeeAsync(EmployeeCreateUpdateDto employeeDto)
        {
            if (employeeDto == null || string.IsNullOrEmpty(employeeDto.Name) || string.IsNullOrEmpty(employeeDto.Position))
            {
                _logger.LogWarning("Invalid employee data provided for creation.");
                throw new ArgumentException("Name and Position are required.");
            }

            _logger.LogInformation("Adding new employee with name {EmployeeName}.", employeeDto.Name);
            try
            {
                var employee = EmployeeMapper.ToEntity(employeeDto);

                // Добавляем сотрудника, чтобы получить ID
                await _employeeRepository.AddEmployeeAsync(employee);

                // Создаем директорию для файлов сотрудника
                var employeeDir = Path.Combine(_employeesUploadsPath, employee.Id.ToString());
                if (!Directory.Exists(employeeDir))
                {
                    Directory.CreateDirectory(employeeDir);
                }

                // Сохраняем фотографию, если она есть
                if (employeeDto.Photo != null && employeeDto.Photo.Length > 0)
                {
                    var photoFileName = $"photo{employee.Id}{Path.GetExtension(employeeDto.Photo.FileName)}";
                    var photoPath = Path.Combine(employeeDir, photoFileName);
                    using (var stream = new FileStream(photoPath, FileMode.Create))
                    {
                        await employeeDto.Photo.CopyToAsync(stream);
                    }
                    employee.PhotoPath = $"/Files/Uploads/Employees/{employee.Id}/{photoFileName}";
                    await _employeeRepository.UpdateEmployeeAsync(employee); // Обновляем с новым путем
                }

                // Второй вызов AddEmployeeAsync убран
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add employee with name {EmployeeName}.", employeeDto.Name);
                throw;
            }
        }


        public async Task UpdateEmployeeAsync(int id, EmployeeCreateUpdateDto employeeDto)
        {
            if (employeeDto == null || string.IsNullOrEmpty(employeeDto.Name) || string.IsNullOrEmpty(employeeDto.Position))
            {
                _logger.LogWarning("Invalid employee data provided for update.");
                throw new ArgumentException("Name and Position are required.");
            }

            _logger.LogInformation("Updating employee with ID {EmployeeId}.", id);
            try
            {
                var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
                if (employee == null)
                {
                    _logger.LogWarning("Employee with ID {EmployeeId} not found.", id);
                    throw new KeyNotFoundException("Employee not found.");
                }

                EmployeeMapper.UpdateEntity(employee, employeeDto);

                // Создаем директорию для файлов сотрудника, если её нет
                var employeeDir = Path.Combine(_employeesUploadsPath, employee.Id.ToString());
                if (!Directory.Exists(employeeDir))
                {
                    Directory.CreateDirectory(employeeDir);
                }

                // Обновляем фотографию, если она есть
                if (employeeDto.Photo != null && employeeDto.Photo.Length > 0)
                {
                    // Удаляем старую фотографию, если она есть
                    if (!string.IsNullOrEmpty(employee.PhotoPath))
                    {
                        var oldPhotoPath = Path.Combine("wwwroot", employee.PhotoPath.TrimStart('/'));
                        if (File.Exists(oldPhotoPath))
                        {
                            File.Delete(oldPhotoPath);
                        }
                    }

                    var photoFileName = $"photo{employee.Id}{Path.GetExtension(employeeDto.Photo.FileName)}";
                    var photoPath = Path.Combine(employeeDir, photoFileName);
                    using (var stream = new FileStream(photoPath, FileMode.Create))
                    {
                        await employeeDto.Photo.CopyToAsync(stream);
                    }
                    employee.PhotoPath = $"/Files/Uploads/Employees/{employee.Id}/{photoFileName}";
                }

                await _employeeRepository.UpdateEmployeeAsync(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update employee with ID {EmployeeId}.", id);
                throw;
            }
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            _logger.LogInformation("Deleting employee with ID {EmployeeId}.", id);
            try
            {
                // Находим сотрудника
                var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
                if (employee == null)
                {
                    _logger.LogWarning("Employee with ID {EmployeeId} not found for deletion.", id);
                    throw new KeyNotFoundException("Employee not found.");
                }

                // Удаляем связанные файлы
                var employeeDir = Path.Combine(_employeesUploadsPath, employee.Id.ToString());
                if (Directory.Exists(employeeDir))
                {
                    // Удаляем фотографию
                    if (!string.IsNullOrEmpty(employee.PhotoPath))
                    {
                        var photoPath = Path.Combine("wwwroot", employee.PhotoPath.TrimStart('/'));
                        if (File.Exists(photoPath))
                        {
                            File.Delete(photoPath);
                        }
                    }

                    // Удаляем директорию
                    Directory.Delete(employeeDir, true);
                }

                // Удаляем сотрудника из базы
                await _employeeRepository.DeleteEmployeeAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete employee with ID {EmployeeId}.", id);
                throw;
            }
        }

    }
}
