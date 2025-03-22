using Npgsql;
using OntuPhdApi.Models;

namespace OntuPhdApi.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Doctor> GetDoctors()
        {
            var doctors = new List<Doctor>();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("SELECT Id, Name, Degree FROM Doctors", connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        doctors.Add(new Doctor
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Degree = reader.GetString(2)
                        });
                    }
                }
            }
            return doctors;
        }

        public void AddDoctor(Doctor doctor)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("INSERT INTO Doctors (Name, Degree) VALUES (@name, @degree)", connection))
                {
                    cmd.Parameters.AddWithValue("name", doctor.Name);
                    cmd.Parameters.AddWithValue("degree", doctor.Degree);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}