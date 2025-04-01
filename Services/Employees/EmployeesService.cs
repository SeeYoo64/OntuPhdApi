using Npgsql;
using OntuPhdApi.Models.Employees;

namespace OntuPhdApi.Services.Employees
{
    public class EmployeesService : IEmployeesService
    {
        private readonly string _connectionString;

        public EmployeesService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<EmployeesModel> GetEmployees()
        {
            var employees = new List<EmployeesModel>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("SELECT Id, Name, Position, Photo FROM Employees", connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new EmployeesModel
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Position = reader.GetString(2),
                            Photo = reader.GetString(3)
                        });
                    }
                }
            }

            return employees;
        }

        public EmployeesModel GetEmployeeById(int id)
        {
            EmployeesModel employee = null;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("SELECT Id, Name, Position, Photo FROM Employees WHERE Id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            employee = new EmployeesModel
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Position = reader.GetString(2),
                                Photo = reader.GetString(3)
                            };
                        }
                    }
                }
            }

            return employee;
        }

        public void UpdateEmployee(EmployeesModel employee)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand(
                    "UPDATE Employees SET Name = @name, Position = @position, Photo = @photo WHERE Id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("id", employee.Id);
                    cmd.Parameters.AddWithValue("name", employee.Name);
                    cmd.Parameters.AddWithValue("position", employee.Position);
                    cmd.Parameters.AddWithValue("photo", employee.Photo);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void AddEmployee(EmployeesModel employee)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand(
                    "INSERT INTO Employees (Name, Position, Photo) " +
                    "VALUES (@name, @position, @photo) RETURNING Id", connection))
                {
                    cmd.Parameters.AddWithValue("name", employee.Name);
                    cmd.Parameters.AddWithValue("position", employee.Position);
                    cmd.Parameters.AddWithValue("photo", employee.Photo ?? "");
                    employee.Id = (int)cmd.ExecuteScalar();
                }
            }
        }
    
    
    
    
    }
}
