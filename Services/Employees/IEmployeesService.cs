using OntuPhdApi.Models.Employees;

namespace OntuPhdApi.Services.Employees
{
    public interface IEmployeesService
    {
        Task<List<EmployeeModelDto>> GetEmployeesAsync();
        Task<EmployeeModelDto> GetEmployeeByIdAsync(int id);
        Task AddEmployeeAsync(EmployeeCreateUpdateDto employeeDto);
        Task UpdateEmployeeAsync(int id, EmployeeCreateUpdateDto employeeDto);
    }
}
