using OntuPhdApi.Models.Employees;

namespace OntuPhdApi.Services.Employees
{
    public interface IEmployeesService
    {
        Task<List<EmployeeModelDto>> GetEmployeesAsync();
        Task<EmployeeModelDto> GetEmployeeByIdAsync(int id);
        Task<EmployeeModel> AddEmployeeAsync(EmployeeCreateUpdateDto employeeDto);
        Task UpdateEmployeeAsync(int id, EmployeeCreateUpdateDto employeeDto);
        Task DeleteEmployeeAsync(int id);
    }
}
