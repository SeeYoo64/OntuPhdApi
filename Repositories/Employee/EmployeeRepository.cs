using Microsoft.EntityFrameworkCore;
using OntuPhdApi.Data;
using OntuPhdApi.Models.Employees;

namespace OntuPhdApi.Repositories.Employee
{
    public class EmployeeRepository : IEmployeeRepository
    {

        private readonly AppDbContext _context;
        private readonly ILogger<EmployeeRepository> _logger;

        public EmployeeRepository(AppDbContext context, ILogger<EmployeeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<EmployeeModel>> GetAllEmployeesAsync()
        {
            _logger.LogInformation("Fetching all employees from database.");
            try
            {
                return await _context.Employees
                    .OrderBy(e => e.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all employees.");
                throw;
            }
        }


        public async Task<EmployeeModel> GetEmployeeByIdAsync(int id)
        {
            _logger.LogInformation("Fetching employee with ID {EmployeeId} from database.", id);
            try
            {
                return await _context.Employees
                    .FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching employee with ID {EmployeeId}.", id);
                throw;
            }
        }

        public async Task AddEmployeeAsync(EmployeeModel employee)
        {
            _logger.LogInformation("Adding new employee with name {EmployeeName}.", employee.Name);
            try
            {
                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding employee with name {EmployeeName}.", employee.Name);
                throw;
            }
        }


        public async Task UpdateEmployeeAsync(EmployeeModel employee)
        {
            _logger.LogInformation("Updating employee with ID {EmployeeId}.", employee.Id);
            try
            {
                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee with ID {EmployeeId}.", employee.Id);
                throw;
            }
        }





    }
}
