namespace OntuPhdApi.Models.Employees
{
    public class EmployeesModelDto
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public IFormFile Photo { get; set; } // Файл фотографии

    }
}
