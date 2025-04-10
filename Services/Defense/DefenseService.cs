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

        public List<DefenseModel> GetDefenses()
        {
            var defenseList = new List<DefenseModel>();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("SELECT Id, Name_Surname, defense_name, science_teachers, " +
                    "date_of_defense, address, message, placeholder, members, files, date_of_publication, program_id " +
                    "FROM Defense", connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        try
                        {

                            defenseList.Add(new DefenseModel
                            {
                                Id = reader.GetInt32(0),
                                NameSurname = reader.GetString(1),
                                DefenseName = reader.GetString(2),
                                ScienceTeachers = reader.IsDBNull(3) ? null : JsonSerializer.Deserialize<List<string>>(reader.GetString(3), jsonOptions),
                                DateOfDefense = reader.GetDateTime(4),
                                ProgramInfo = new ProgramDefense(),
                                Address = reader.IsDBNull(5) ? null : reader.GetString(5),
                                Message = reader.IsDBNull(6) ? null : reader.GetString(6),
                                Placeholder = reader.IsDBNull(7) ? null : reader.GetString(7),
                                Members = reader.IsDBNull(8) ? null : JsonSerializer.Deserialize<List<CompositionOfRada>>(reader.GetString(8), jsonOptions),
                                FilesDefense = JsonSerializer.Deserialize<List<FilesDefense>>(reader.GetString(9), jsonOptions),
                                DateOfPublication = reader.GetDateTime(10),
                                ProgramId = reader.GetInt32(11)
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


        public DefenseModel GetDefenseById(int id)
        {
            var defense = new DefenseModel();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("SELECT Id, Name_Surname, defense_name, science_teachers, " +
                    "date_of_defense, address, message, placeholder, members, files, date_of_publication, program_id " +
                    "FROM Defense WHERE id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            try
                            {
                                defense = new DefenseModel
                                {
                                    Id = reader.GetInt32(0),
                                    NameSurname = reader.GetString(1),
                                    DefenseName = reader.GetString(2),
                                    ScienceTeachers = reader.IsDBNull(3) ? null : JsonSerializer.Deserialize<List<string>>(reader.GetString(3), jsonOptions),
                                    DateOfDefense = reader.GetDateTime(4),
                                    ProgramInfo = new ProgramDefense(),
                                    Address = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    Message = reader.IsDBNull(6) ? null : reader.GetString(6),
                                    Placeholder = reader.IsDBNull(7) ? null : reader.GetString(7),
                                    Members = reader.IsDBNull(8) ? null : JsonSerializer.Deserialize<List<CompositionOfRada>>(reader.GetString(8), jsonOptions),
                                    FilesDefense = JsonSerializer.Deserialize<List<FilesDefense>>(reader.GetString(9), jsonOptions),
                                    DateOfPublication = reader.GetDateTime(10),
                                    ProgramId = reader.GetInt32(11)
                                };
                            }
                            catch (JsonException ex)
                            {
                                Console.WriteLine($"Error deserializing News with ID {reader.GetInt32(0)}: {ex.Message}");
                            }
                        }
                    }
                }
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
            return defense;
        }


        public List<DefenseModel> GetDefensesByDegree(string degree)
        {
            var defenseList = new List<DefenseModel>();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                // Выбираем только те защиты, у которых степень совпадает с указанной
                using (var cmd = new NpgsqlCommand(@"
                    SELECT d.Id, d.Name_Surname, d.defense_name, d.science_teachers, 
                    d.date_of_defense, d.address, d.message, d.placeholder, 
                    d.members, d.files, d.date_of_publication, d.program_id,
                    p.Name, p.Degree, p.Field_of_study, p.Speciality
                    FROM Defense d
                    JOIN Program p ON d.program_id = p.Id
                    WHERE p.Degree = @degree", connection))
                {
                    cmd.Parameters.AddWithValue("degree", degree);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                var defense = new DefenseModel
                                {
                                    Id = reader.GetInt32(0),
                                    NameSurname = reader.GetString(1),
                                    DefenseName = reader.GetString(2),
                                    ScienceTeachers = reader.IsDBNull(3) ? null : JsonSerializer.Deserialize<List<string>>(reader.GetString(3), jsonOptions),
                                    DateOfDefense = reader.GetDateTime(4),
                                    Address = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    Message = reader.IsDBNull(6) ? null : reader.GetString(6),
                                    Placeholder = reader.IsDBNull(7) ? null : reader.GetString(7),
                                    Members = reader.IsDBNull(8) ? null :
                                        JsonSerializer.Deserialize<List<CompositionOfRada>>(reader.GetString(8), jsonOptions),
                                    FilesDefense = reader.IsDBNull(9) ? new List<FilesDefense>() :
                                        JsonSerializer.Deserialize<List<FilesDefense>>(reader.GetString(9), jsonOptions),
                                    DateOfPublication = reader.GetDateTime(10),
                                    ProgramId = reader.GetInt32(11),
                                    ProgramInfo = new ProgramDefense
                                    {
                                        Id = reader.GetInt32(11),
                                        Name = reader.GetString(12),
                                        Degree = reader.GetString(13),
                                        FieldOfStudy = reader.IsDBNull(14) ? null :
                                            JsonSerializer.Deserialize<FieldOfStudy>(reader.GetString(14), jsonOptions),
                                        Speciality = reader.IsDBNull(15) ? null :
                                            JsonSerializer.Deserialize<ShortSpeciality>(reader.GetString(15), jsonOptions),
                                    }
                                };

                                defenseList.Add(defense);
                            }
                            catch (JsonException ex)
                            {
                                Console.WriteLine($"Error deserializing Defense with ID {reader.GetInt32(0)}: {ex.Message}");
                            }
                        }
                    }
                }
            }

            return defenseList;
        }



    }
}
