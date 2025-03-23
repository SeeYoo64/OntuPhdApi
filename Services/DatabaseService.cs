using Npgsql;
using OntuPhdApi.Models;
using System.Text.Json;

namespace OntuPhdApi.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<ProgramView> GetPrograms()
        {
            var programs = new List<ProgramView>();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand(
                    "SELECT Id, Name, FieldOfStudy, Speciality, Form, Years, Credits, Sum, Costs, " +
                    "ProgramCharacteristics, ProgramCompetence, ProgramResults, LinkFaculty, LinkFile " +
                    "FROM Programs", connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        try
                        {
                            var program = new ProgramView
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                FieldOfStudy = reader.IsDBNull(2) ? null : JsonSerializer.Deserialize<FieldOfStudy>(reader.GetString(2), jsonOptions),
                                Speciality = reader.IsDBNull(3) ? null : JsonSerializer.Deserialize<Speciality>(reader.GetString(3), jsonOptions),
                                Form = reader.IsDBNull(4) ? null : JsonSerializer.Deserialize<List<string>>(reader.GetString(4), jsonOptions),
                                Years = reader.GetInt32(5),
                                Credits = reader.GetInt32(6),
                                Sum = reader.GetDecimal(7),
                                Costs = reader.IsDBNull(8) ? null : JsonSerializer.Deserialize<List<decimal>>(reader.GetString(8), jsonOptions),
                                ProgramCharacteristics = reader.IsDBNull(9) ? null : JsonSerializer.Deserialize<ProgramCharacteristics>(reader.GetString(9), jsonOptions),
                                ProgramCompetence = reader.IsDBNull(10) ? null : JsonSerializer.Deserialize<ProgramCompetence>(reader.GetString(10), jsonOptions),
                                ProgramResults = reader.IsDBNull(11) ? null : JsonSerializer.Deserialize<ProgramResults>(reader.GetString(11), jsonOptions),
                                LinkFaculty = reader.GetString(12),
                                LinkFile = reader.GetString(13),
                                Components = new List<ProgramComponent>(),
                                Jobs = new List<Job>()
                            };
                            programs.Add(program);
                        }
                        catch (JsonException ex)
                        {
                            Console.WriteLine($"Error deserializing program with ID {reader.GetInt32(0)}: {ex.Message}");
                        }
                    }
                }

                foreach (var program in programs)
                {
                    using (var cmd = new NpgsqlCommand(
                        "SELECT Id, ComponentType, ComponentName, ComponentCredits, ComponentHours, ControlForm " +
                        "FROM ProgramComponents " +
                        "WHERE ProgramId = @programId", connection))
                    {
                        cmd.Parameters.AddWithValue("programId", program.Id);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                program.Components.Add(new ProgramComponent
                                {
                                    Id = reader.GetInt32(0),
                                    ProgramId = program.Id,
                                    ComponentType = reader.GetString(1),
                                    ComponentName = reader.GetString(2),
                                    ComponentCredits = reader.GetInt32(3),
                                    ComponentHours = reader.GetInt32(4),
                                    ControlForm = reader.GetString(5)
                                });
                            }
                        }
                    }

                    using (var cmd = new NpgsqlCommand(
                        "SELECT j.Id, j.Code, j.Title " +
                        "FROM Jobs j " +
                        "JOIN ProgramJobs pj ON j.Id = pj.JobId " +
                        "WHERE pj.ProgramId = @programId", connection))
                    {
                        cmd.Parameters.AddWithValue("programId", program.Id);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                program.Jobs.Add(new Job
                                {
                                    Id = reader.GetInt32(0),
                                    Code = reader.GetString(1),
                                    Title = reader.GetString(2)
                                });
                            }
                        }
                    }
                }
            }
            return programs;
        }

        public ProgramView GetProgramById(int id)
        {
            ProgramView program = null;
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand(
                    "SELECT Id, Name, FieldOfStudy, Speciality, Form, Years, Credits, Sum, Costs, " +
                    "ProgramCharacteristics, ProgramCompetence, ProgramResults, LinkFaculty, LinkFile " +
                    "FROM Programs WHERE Id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            try
                            {
                                program = new ProgramView
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    FieldOfStudy = reader.IsDBNull(2) ? null : JsonSerializer.Deserialize<FieldOfStudy>(reader.GetString(2), jsonOptions),
                                    Speciality = reader.IsDBNull(3) ? null : JsonSerializer.Deserialize<Speciality>(reader.GetString(3), jsonOptions),
                                    Form = reader.IsDBNull(4) ? null : JsonSerializer.Deserialize<List<string>>(reader.GetString(4), jsonOptions),
                                    Years = reader.GetInt32(5),
                                    Credits = reader.GetInt32(6),
                                    Sum = reader.GetDecimal(7),
                                    Costs = reader.IsDBNull(8) ? null : JsonSerializer.Deserialize<List<decimal>>(reader.GetString(8), jsonOptions),
                                    ProgramCharacteristics = reader.IsDBNull(9) ? null : JsonSerializer.Deserialize<ProgramCharacteristics>(reader.GetString(9), jsonOptions),
                                    ProgramCompetence = reader.IsDBNull(10) ? null : JsonSerializer.Deserialize<ProgramCompetence>(reader.GetString(10), jsonOptions),
                                    ProgramResults = reader.IsDBNull(11) ? null : JsonSerializer.Deserialize<ProgramResults>(reader.GetString(11), jsonOptions),
                                    LinkFaculty = reader.GetString(12),
                                    LinkFile = reader.GetString(13),
                                    Components = new List<ProgramComponent>(),
                                    Jobs = new List<Job>()
                                };
                            }
                            catch (JsonException ex)
                            {
                                throw new Exception($"Error deserializing program with ID {id}: {ex.Message}");
                            }
                        }
                    }
                }

                if (program != null)
                {
                    using (var cmd = new NpgsqlCommand(
                        "SELECT Id, ComponentType, ComponentName, ComponentCredits, ComponentHours, ControlForm " +
                        "FROM ProgramComponents " +
                        "WHERE ProgramId = @programId", connection))
                    {
                        cmd.Parameters.AddWithValue("programId", program.Id);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                program.Components.Add(new ProgramComponent
                                {
                                    Id = reader.GetInt32(0),
                                    ProgramId = program.Id,
                                    ComponentType = reader.GetString(1),
                                    ComponentName = reader.GetString(2),
                                    ComponentCredits = reader.GetInt32(3),
                                    ComponentHours = reader.GetInt32(4),
                                    ControlForm = reader.GetString(5)
                                });
                            }
                        }
                    }

                    using (var cmd = new NpgsqlCommand(
                        "SELECT j.Id, j.Code, j.Title " +
                        "FROM Jobs j " +
                        "JOIN ProgramJobs pj ON j.Id = pj.JobId " +
                        "WHERE pj.ProgramId = @programId", connection))
                    {
                        cmd.Parameters.AddWithValue("programId", program.Id);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                program.Jobs.Add(new Job
                                {
                                    Id = reader.GetInt32(0),
                                    Code = reader.GetString(1),
                                    Title = reader.GetString(2)
                                });
                            }
                        }
                    }
                }
            }
            return program;
        }

        public void AddProgram(ProgramView program)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                int programId;
                using (var cmd = new NpgsqlCommand(
                    "INSERT INTO Programs (Name, FieldOfStudy, Speciality, Form, Years, Credits, Sum, Costs, " +
                    "ProgramCharacteristics, ProgramCompetence, ProgramResults, LinkFaculty, LinkFile) " +
                    "VALUES (@name, @fieldOfStudy, @speciality, @form, @years, @credits, @sum, @costs, " +
                    "@programCharacteristics, @programCompetence, @programResults, @linkFaculty, @linkFile) " +
                    "RETURNING Id", connection))
                {
                    cmd.Parameters.AddWithValue("name", program.Name);
                    cmd.Parameters.AddWithValue("fieldOfStudy", JsonSerializer.Serialize(program.FieldOfStudy, jsonOptions));
                    cmd.Parameters.AddWithValue("speciality", JsonSerializer.Serialize(program.Speciality, jsonOptions));
                    cmd.Parameters.AddWithValue("form", JsonSerializer.Serialize(program.Form, jsonOptions));
                    cmd.Parameters.AddWithValue("years", program.Years);
                    cmd.Parameters.AddWithValue("credits", program.Credits);
                    cmd.Parameters.AddWithValue("sum", program.Sum);
                    cmd.Parameters.AddWithValue("costs", JsonSerializer.Serialize(program.Costs, jsonOptions));
                    cmd.Parameters.AddWithValue("programCharacteristics", JsonSerializer.Serialize(program.ProgramCharacteristics, jsonOptions));
                    cmd.Parameters.AddWithValue("programCompetence", JsonSerializer.Serialize(program.ProgramCompetence, jsonOptions));
                    cmd.Parameters.AddWithValue("programResults", JsonSerializer.Serialize(program.ProgramResults, jsonOptions));
                    cmd.Parameters.AddWithValue("linkFaculty", program.LinkFaculty);
                    cmd.Parameters.AddWithValue("linkFile", program.LinkFile);
                    programId = (int)cmd.ExecuteScalar();
                }

                if (program.Components != null)
                {
                    foreach (var component in program.Components)
                    {
                        using (var cmd = new NpgsqlCommand(
                            "INSERT INTO ProgramComponents (ProgramId, ComponentType, ComponentName, ComponentCredits, ComponentHours, ControlForm) " +
                            "VALUES (@programId, @componentType, @componentName, @componentCredits, @componentHours, @controlForm)", connection))
                        {
                            cmd.Parameters.AddWithValue("programId", programId);
                            cmd.Parameters.AddWithValue("componentType", component.ComponentType);
                            cmd.Parameters.AddWithValue("componentName", component.ComponentName);
                            cmd.Parameters.AddWithValue("componentCredits", component.ComponentCredits);
                            cmd.Parameters.AddWithValue("componentHours", component.ComponentHours);
                            cmd.Parameters.AddWithValue("controlForm", JsonSerializer.Serialize(component.ControlForm, jsonOptions));
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                if (program.Jobs != null)
                {
                    foreach (var job in program.Jobs)
                    {
                        int jobId;
                        using (var cmd = new NpgsqlCommand(
                            "SELECT Id FROM Jobs WHERE Code = @code AND Title = @title", connection))
                        {
                            cmd.Parameters.AddWithValue("code", job.Code);
                            cmd.Parameters.AddWithValue("title", job.Title);
                            var result = cmd.ExecuteScalar();
                            if (result != null)
                            {
                                jobId = (int)result;
                            }
                            else
                            {
                                using (var insertCmd = new NpgsqlCommand(
                                    "INSERT INTO Jobs (Code, Title) VALUES (@code, @title) RETURNING Id", connection))
                                {
                                    insertCmd.Parameters.AddWithValue("code", job.Code);
                                    insertCmd.Parameters.AddWithValue("title", job.Title);
                                    jobId = (int)insertCmd.ExecuteScalar();
                                }
                            }
                        }

                        using (var cmd = new NpgsqlCommand(
                            "INSERT INTO ProgramJobs (ProgramId, JobId) VALUES (@programId, @jobId)", connection))
                        {
                            cmd.Parameters.AddWithValue("programId", programId);
                            cmd.Parameters.AddWithValue("jobId", jobId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }
}