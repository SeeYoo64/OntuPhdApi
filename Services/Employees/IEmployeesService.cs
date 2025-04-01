using OntuPhdApi.Models.Employees;

namespace OntuPhdApi.Services.Employees
{
    public interface IEmployeesService
    {

        List<EmployeesModel> GetEmployees();
        EmployeesModel GetEmployeeById(int id);
        void UpdateEmployee(EmployeesModel employee);
        void AddEmployee(EmployeesModel employee);
    }
}
