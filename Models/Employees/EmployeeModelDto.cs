namespace OntuPhdApi.Models.Employees
{
    public class EmployeeModelDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string? PhotoPath { get; set; }

    }

    public class EmployeeCreateUpdateDto
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public IFormFile? PhotoPath { get; set; } // Файл фотографии для загрузки
    }

}
