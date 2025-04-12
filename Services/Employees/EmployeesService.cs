using Npgsql;
using OntuPhdApi.Models.Employees;
using OntuPhdApi.Repositories.Employee;
using OntuPhdApi.Services.Files;
using OntuPhdApi.Utilities;

namespace OntuPhdApi.Services.Employees
{
    public class EmployeesService : IEmployeesService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IProgramFileService _fileService; 
        private readonly ILogger<EmployeesService> _logger;

        public EmployeesService(
            IEmployeeRepository employeeRepository,
            IProgramFileService fileService,
            ILogger<EmployeesService> logger)
        {
            _employeeRepository = employeeRepository;
            _fileService = fileService;
            _logger = logger;
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

                // Сохраняем фотографию, если она есть
                if (employeeDto.Photo != null && employeeDto.Photo.Length > 0)
                {
                    var (filePath, _, _, _) = await _fileService.SaveProgramFileFromFormAsync(employeeDto.Name, employeeDto.Photo);
                    employee.PhotoPath = filePath;
                }

                await _employeeRepository.AddEmployeeAsync(employee);
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

                // Обновляем фотографию, если она есть
                if (employeeDto.Photo != null && employeeDto.Photo.Length > 0)
                {
                    var (filePath, _, _, _) = await _fileService.SaveProgramFileFromFormAsync(employeeDto.Name, employeeDto.Photo);
                    employee.PhotoPath = filePath;
                }

                await _employeeRepository.UpdateEmployeeAsync(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update employee with ID {EmployeeId}.", id);
                throw;
            }
        }



    }
}
