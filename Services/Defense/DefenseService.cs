using Npgsql;
using OntuPhdApi.Models.Defense;
using OntuPhdApi.Models.Programs;
using System.Text.Json;

namespace OntuPhdApi.Services.Defense
{
    public class DefenseService : IDefenseService
    {
        private readonly string _connectionString;

        public DefenseService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<DefensePhdModel> GetDefenses()
        {
            var defenseList = new List<DefensePhdModel>();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("SELECT Id, Name_Surname, defense_name, science_teachers, " +
                    "date_of_defense, address, description, members, files, date_of_publication, program_id " +
                    "FROM Defense", connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        try
                        {
                            defenseList.Add(new DefensePhdModel
                            {
                                Id = reader.GetInt32(0),
                                NameSurname = reader.GetString(1),
                                DefenseName = reader.GetString(2),
                                ScienceTeachers = reader.GetString(3),
                                DateOfDefense = reader.GetDateTime(4),
                                ProgramInfo = new ProgramDefense(),
                                Address = reader.GetString(5),
                                Description = reader.GetString(6),
                                Members = JsonSerializer.Deserialize<List<MemberOfRada>>(reader.GetString(7), jsonOptions), 
                                Files = JsonSerializer.Deserialize<List<Files>>(reader.GetString(8), jsonOptions),
                                DateOfPublication = reader.GetDateTime(9),
                                ProgramId = reader.GetInt32(10)
                            });
                        }
                        catch (JsonException ex)
                        {
                            Console.WriteLine($"Error deserializing News with ID {reader.GetInt32(0)}: {ex.Message}");
                        }
                    }
                }

                foreach (var defense in defenseList)
                {

                    using (var cmd = new NpgsqlCommand(
                        "SELECT Id, Name, Degree, Field_of_study, Speciality " +
                        "FROM Program " +
                        "WHERE id = @programId", connection))
                    {
                        cmd.Parameters.AddWithValue("programId", defense.ProgramId);
                        using var reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            defense.ProgramInfo = new ProgramDefense
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Degree = reader.GetString(2),
                                FieldOfStudy = reader.IsDBNull(3) ? null :
                                    JsonSerializer.Deserialize<FieldOfStudy>(reader.GetString(3), jsonOptions),
                                Speciality = reader.IsDBNull(4) ? null :
                                    JsonSerializer.Deserialize<ShortSpeciality>(reader.GetString(4), jsonOptions),
                            };
                        }
                    }
                }
            }
            return defenseList;
        }


        public List<DefensePhdModel> GetDefenses()
        {
            var defenseList = new List<DefensePhdModel>();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("SELECT Id, Name_Surname, defense_name, science_teachers, " +
                    "date_of_defense, address, description, members, files, date_of_publication, program_id " +
                    "FROM Defense", connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        try
                        {
                            defenseList.Add(new DefensePhdModel
                            {
                                Id = reader.GetInt32(0),
                                NameSurname = reader.GetString(1),
                                DefenseName = reader.GetString(2),
                                ScienceTeachers = reader.GetString(3),
                                DateOfDefense = reader.GetDateTime(4),
                                ProgramInfo = new ProgramDefense(),
                                Address = reader.GetString(5),
                                Description = reader.GetString(6),
                                Members = JsonSerializer.Deserialize<List<MemberOfRada>>(reader.GetString(7), jsonOptions),
                                Files = JsonSerializer.Deserialize<List<Files>>(reader.GetString(8), jsonOptions),
                                DateOfPublication = reader.GetDateTime(9),
                                ProgramId = reader.GetInt32(10)
                            });
                        }
                        catch (JsonException ex)
                        {
                            Console.WriteLine($"Error deserializing News with ID {reader.GetInt32(0)}: {ex.Message}");
                        }
                    }
                }

                foreach (var defense in defenseList)
                {

                    using (var cmd = new NpgsqlCommand(
                        "SELECT Id, Name, Degree, Field_of_study, Speciality " +
                        "FROM Program " +
                        "WHERE id = @programId", connection))
                    {
                        cmd.Parameters.AddWithValue("programId", defense.ProgramId);
                        using var reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            defense.ProgramInfo = new ProgramDefense
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Degree = reader.GetString(2),
                                FieldOfStudy = reader.IsDBNull(3) ? null :
                                    JsonSerializer.Deserialize<FieldOfStudy>(reader.GetString(3), jsonOptions),
                                Speciality = reader.IsDBNull(4) ? null :
                                    JsonSerializer.Deserialize<ShortSpeciality>(reader.GetString(4), jsonOptions),
                            };
                        }
                    }
                }
            }
            return defenseList;
        }


    }
}
