using Npgsql;
using OntuPhdApi.Models.Employees;
using OntuPhdApi.Repositories.Employee;
using OntuPhdApi.Utilities.Mappers;

namespace OntuPhdApi.Services.Employees
{
    public class EmployeesService : IEmployeesService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeesService> _logger;
        private readonly string _employeesUploadsPath = Path.Combine("wwwroot", "Files", "Uploads", "Employees");

        public EmployeesService(
            IEmployeeRepository employeeRepository,
            ILogger<EmployeesService> logger)
        {
            _employeeRepository = employeeRepository;
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



        public async Task<EmployeeModel> AddEmployeeAsync(EmployeeCreateUpdateDto employeeDto)
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
                employee.PhotoPath = "blank.png";

                await _employeeRepository.AddEmployeeAsync(employee);

                var safeName = string.Join("_", employee.Id);
                var employeeDir = Path.Combine(_employeesUploadsPath, safeName);
                if (!Directory.Exists(employeeDir))
                {
                    Directory.CreateDirectory(employeeDir);
                    _logger.LogInformation("Created directory for employee: {Directory}", employeeDir);
                }

                var sourceBlankPath = Path.Combine("wwwroot", "Files", "Uploads", "blank.png");
                var destBlankPath = Path.Combine(employeeDir, "blank.png");
                if (System.IO.File.Exists(sourceBlankPath))
                {
                    System.IO.File.Copy(sourceBlankPath, destBlankPath, overwrite: false);
                    _logger.LogInformation("Copied blank.png to: {DestPath}", destBlankPath);
                }
                else
                {
                    _logger.LogWarning("Source blank.png not found at: {SourcePath}, proceeding without copy", sourceBlankPath);
                }

                if (employeeDto.PhotoPath != null && employeeDto.PhotoPath.Length > 0)
                {
                    var allowedTypes = new[] { "image/png", "image/jpeg", "image/jpg" };
                    if (!allowedTypes.Contains(employeeDto.PhotoPath.ContentType))
                    {
                        _logger.LogError("Invalid photo type for employee: {EmployeeName}, Type: {Type}", employeeDto.Name, employeeDto.PhotoPath.ContentType);
                        throw new ArgumentException("Only PNG, JPG, JPEG files are allowed.");
                    }

                    if (employeeDto.PhotoPath.Length > 5 * 1024 * 1024)
                    {
                        _logger.LogError("Photo too large for employee: {EmployeeName}, Size: {Size}", employeeDto.Name, employeeDto.PhotoPath.Length);
                        throw new ArgumentException("File size exceeds 5 MB.");
                    }

                    var extension = Path.GetExtension(employeeDto.PhotoPath.FileName).ToLower();
                    var photoFileName = $"photo_{DateTime.UtcNow:yyyyMMddHHmmss}{extension}";
                    var photoPath = Path.Combine(employeeDir, photoFileName);
                    using (var stream = new FileStream(photoPath, FileMode.Create))
                    {
                        await employeeDto.PhotoPath.CopyToAsync(stream);
                    }
                    employee.PhotoPath = photoFileName;
                    _logger.LogInformation("Uploaded photo for employee: {EmployeeName}, Path: {PhotoPath}", employeeDto.Name, photoPath);

                    await _employeeRepository.UpdateEmployeeAsync(employee);
                }

                return employee;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add employee with name {EmployeeName}.", employeeDto.Name);
                throw;
            }
        }


        public async Task UpdateEmployeeAsync(int id, EmployeeCreateUpdateDto employeeDto)
        {
            if (employeeDto == null || string.IsNullOrEmpty(employeeDto.Name) || string.IsNullOrEmpty(employeeDto.Position)) { 
                _logger.LogWarning("Invalid employee data provided for update."); 
                throw new ArgumentException("Name and Position are required."); 
            } 
            _logger.LogInformation("Updating employee with ID {EmployeeId}.", id); 
            try { 
                var employee = await _employeeRepository.GetEmployeeByIdAsync(id); 
                if (employee == null) { 
                    _logger.LogWarning("Employee with ID {EmployeeId} not found.", id); 
                    throw new KeyNotFoundException("Employee not found."); 
                } 
                EmployeeMapper.UpdateEntity(employee, employeeDto);
                var safeName = string.Join("_", employee.Id);
                var employeeDir = Path.Combine(_employeesUploadsPath, safeName); 
                if (!Directory.Exists(employeeDir)) { Directory.CreateDirectory(employeeDir); 
                    _logger.LogInformation("Created directory for employee: {Directory}", employeeDir); 
                } 
                try { 
                    foreach (var oldFile in Directory.GetFiles(employeeDir)) { 
                        var fileName = Path.GetFileName(oldFile); 
                        if (fileName != "blank.png") { 
                            System.IO.File.Delete(oldFile); 
                            _logger.LogInformation("Deleted old photo for employee ID: {EmployeeId}, File: {File}", id, oldFile); 
                        } 
                    } 
                } catch (Exception ex) { 
                    _logger.LogError(ex, "Failed to delete old photos for employee ID: {EmployeeId}, Directory: {Directory}", id, employeeDir); 
                } 
                if (employeeDto.PhotoPath != null && employeeDto.PhotoPath.Length != 0) { 
                    var allowedTypes = new[] { "image/png", "image/jpeg", "image/jpg" }; 
                    if (!allowedTypes.Contains(employeeDto.PhotoPath.ContentType)) { 
                        _logger.LogError("Invalid photo type for employee ID: {EmployeeId}, Type: {Type}", id, employeeDto.PhotoPath.ContentType); 
                        throw new ArgumentException("Only PNG, JPG, JPEG files are allowed."); 
                    } 
                    if (employeeDto.PhotoPath.Length > 5 * 1024 * 1024) 
                    { 
                        _logger.LogError("Photo too large for employee ID: {EmployeeId}, Size: {Size}", id, employeeDto.PhotoPath.Length); 
                        throw new ArgumentException("File size exceeds 5 MB."); 
                    } 
                    var extension = Path.GetExtension(employeeDto.PhotoPath.FileName).ToLower(); 
                    var photoFileName = $"photo_{DateTime.UtcNow:yyyyMMddHHmmss}{extension}"; 
                    var photoPath = Path.Combine(employeeDir, photoFileName); 
                    using (var stream = new FileStream(photoPath, FileMode.Create)) { 
                        await employeeDto.PhotoPath.CopyToAsync(stream); 
                    } 
                    employee.PhotoPath = photoFileName; 
                    _logger.LogInformation("Uploaded photo for employee ID: {EmployeeId}, Path: {PhotoPath}", id, photoPath); 
                } 
                else { 
                    employee.PhotoPath = "blank.png"; 
                    _logger.LogInformation("Set blank.png for employee ID: {EmployeeId}", id); 
                } 
                await _employeeRepository.UpdateEmployeeAsync(employee); 
            } 
            catch (Exception ex) { 
                _logger.LogError(ex, "Failed to update employee with ID {EmployeeId}.", id); 
                throw; 
            }
        }


        public async Task UploadEmployeePhotoAsync(int id, IFormFile? photo) 
        { 
            _logger.LogInformation("Uploading photo for employee with ID {EmployeeId}.", id); try 
            { 
                var employee = await _employeeRepository.GetEmployeeByIdAsync(id); 
                if (employee == null) { _logger.LogWarning("Employee with ID {EmployeeId} not found.", id); 
                    throw new KeyNotFoundException("Employee not found."); 
                }
                var safeName = string.Join("_", employee.Id);
                var employeeDir = Path.Combine(_employeesUploadsPath, safeName); 
                if (!Directory.Exists(employeeDir)) 
                { 
                    Directory.CreateDirectory(employeeDir); 
                    _logger.LogInformation("Created directory for employee: {Directory}", employeeDir); 
                    var sourceBlankPath = Path.Combine("wwwroot", "Files", "Uploads", "blank.png"); 
                    var destBlankPath = Path.Combine(employeeDir, "blank.png"); 
                    if (System.IO.File.Exists(sourceBlankPath)) { 
                        System.IO.File.Copy(sourceBlankPath, destBlankPath, overwrite: false); 
                        _logger.LogInformation("Copied blank.png to: {DestPath}", destBlankPath); 
                    } else { 
                        _logger.LogWarning("Source blank.png not found at: {SourcePath}, proceeding without copy", sourceBlankPath); 
                    } 
                } 
                try { 
                    foreach (var oldFile in Directory.GetFiles(employeeDir)) { 
                        var fileName = Path.GetFileName(oldFile); 
                        if (fileName != "blank.png") { 
                            System.IO.File.Delete(oldFile); 
                            _logger.LogInformation("Deleted old photo for employee ID: {EmployeeId}, File: {File}", id, oldFile); 
                        } 
                    } 
                } catch (Exception ex) { 
                    _logger.LogError(ex, "Failed to delete old photos for employee ID: {EmployeeId}, Directory: {Directory}", id, employeeDir); 
                } 
                string photoPath; 
                if (photo != null && photo.Length !=  0) 
                { 
                    var allowedTypes = new[] { "image/png", "image/jpeg", "image/jpg" }; 
                    if (!allowedTypes.Contains(photo.ContentType)) { 
                        _logger.LogError("Invalid photo type for employee ID: {EmployeeId}, Type: {Type}", id, photo.ContentType); 
                        throw new ArgumentException("Only PNG, JPG, JPEG files are allowed."); 
                    } 
                    if (photo.Length > 5 * 1024 * 1024) 
                    { 
                        _logger.LogError("Photo too large for employee ID: {EmployeeId}, Size: {Size}", id, photo.Length); 
                        throw new ArgumentException("File size exceeds 5 MB."); 
                    } 
                    var extension = Path.GetExtension(photo.FileName).ToLower();
                    var photoFileName = $"photo_{DateTime.UtcNow:yyyyMMddHHmmss}{extension}"; 
                    var filePath = Path.Combine(employeeDir, photoFileName); 
                    using (var stream = new FileStream(filePath, FileMode.Create)) 
                    { 
                        await photo.CopyToAsync(stream); 
                    } 
                    employee.PhotoPath = photoFileName; 
                    photoPath = $"Files/Uploads/Employees/{safeName}/{photoFileName}"; 
                    _logger.LogInformation("Uploaded photo for employee ID: {EmployeeId}, Path: {PhotoPath}", id, photoPath); 
                } 

                else 
                { 
                    employee.PhotoPath = "blank.png"; 
                    photoPath = $"Files/Uploads/Employees/{safeName}/blank.png"; 
                    _logger.LogInformation("Set blank.png for employee ID: {EmployeeId}, PhotoPath: {PhotoPath}", id, photoPath); 
                } 

                await _employeeRepository.UpdateEmployeeAsync(employee); 
            } 
                catch (Exception ex) { 
                _logger.LogError(ex, "Failed to upload photo for employee with ID {EmployeeId}.", id); throw;
            } 
        }
        
        
        public async Task DeleteEmployeeAsync(int id) { 
            _logger.LogInformation("Deleting employee with ID {EmployeeId}.", id); 
            try 
            { 
                var employee = await _employeeRepository.GetEmployeeByIdAsync(id); 
                if (employee == null) { 
                    _logger.LogWarning("Employee with ID {EmployeeId} not found for deletion.", id); 
                    throw new KeyNotFoundException("Employee not found."); 
                } 
                var safeName = string.Join("_", employee.Id, StringSplitOptions.RemoveEmptyEntries);
                var employeeDir = Path.Combine(_employeesUploadsPath, safeName); 

                if (Directory.Exists(employeeDir)) { 
                    Directory.Delete(employeeDir, true); 
                    _logger.LogInformation("Deleted directory for employee ID: {EmployeeId}, Directory: {Directory}", id, employeeDir); 
                } 
                await _employeeRepository.DeleteEmployeeAsync(id); 
            } catch (Exception ex) 
            { 
                _logger.LogError(ex, "Failed to delete employee with ID {EmployeeId}.", id); throw; 
            } 
        }

    }
}

