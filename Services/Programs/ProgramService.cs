using Microsoft.Extensions.Configuration;
using Npgsql;
using OntuPhdApi.Models;
using OntuPhdApi.Models.Programs;
using System.Text.Json;

namespace OntuPhdApi.Services.Programs
{
    public class ProgramService : IProgramService
    {

        private readonly string _connectionString;

        public ProgramService(IConfiguration configuration)
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
                    "SELECT Id, Degree, Name, Name_Eng, FieldOfStudy, Speciality, Form, Purpose, Years, Credits, Sum, Costs, " +
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
                                Degree = reader.GetString(1),
                                Name = reader.GetString(2),
                                NameEng = reader.IsDBNull(3) ? null : reader.GetString(3),
                                FieldOfStudy = reader.IsDBNull(4) ? null : JsonSerializer.Deserialize<FieldOfStudy>(reader.GetString(4), jsonOptions),
                                Speciality = reader.IsDBNull(5) ? null : JsonSerializer.Deserialize<Speciality>(reader.GetString(5), jsonOptions),
                                Form = reader.IsDBNull(6) ? null : JsonSerializer.Deserialize<List<string>>(reader.GetString(6), jsonOptions),
                                Purpose = reader.GetString(7),
                                Years = reader.GetInt32(8),
                                Credits = reader.GetInt32(9),
                                Sum = reader.GetDecimal(10),
                                Costs = reader.IsDBNull(11) ? null : JsonSerializer.Deserialize<List<decimal>>(reader.GetString(11), jsonOptions),
                                ProgramCharacteristics = reader.IsDBNull(12) ? null : JsonSerializer.Deserialize<ProgramCharacteristics>(reader.GetString(12), jsonOptions),
                                ProgramCompetence = reader.IsDBNull(13) ? null : JsonSerializer.Deserialize<ProgramCompetence>(reader.GetString(13), jsonOptions),
                                Results = reader.IsDBNull(14) ? null : JsonSerializer.Deserialize<List<string>>(reader.GetString(14), jsonOptions),
                                LinkFaculty = reader.GetString(15),
                                LinkFile = reader.GetString(16),
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
                        using var reader = cmd.ExecuteReader();
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
                                ControlForm = reader.IsDBNull(5) ? null : JsonSerializer.Deserialize<List<string>>(reader.GetString(5), jsonOptions)
                            });
                        }
                    }

                    using (var cmd = new NpgsqlCommand(
                        "SELECT j.Id, j.Code, j.Title " +
                        "FROM Jobs j " +
                        "JOIN ProgramJobs pj ON j.Id = pj.JobId " +
                        "WHERE pj.ProgramId = @programId", connection))
                    {
                        cmd.Parameters.AddWithValue("programId", program.Id);
                        using var reader = cmd.ExecuteReader();
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
                    "SELECT Id, Degree, Name, Name_Eng, FieldOfStudy, Speciality, Form, Purpose, Years, Credits, Sum, Costs, " +
                    "ProgramCharacteristics, ProgramCompetence, ProgramResults, LinkFaculty, LinkFile " +
                    "FROM Programs WHERE Id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        try
                        {
                            program = new ProgramView
                            {
                                Id = reader.GetInt32(0),
                                Degree = reader.GetString(1),
                                Name = reader.GetString(2),
                                NameEng = reader.IsDBNull(3) ? null : reader.GetString(3),
                                FieldOfStudy = reader.IsDBNull(4) ? null : JsonSerializer.Deserialize<FieldOfStudy>(reader.GetString(4), jsonOptions),
                                Speciality = reader.IsDBNull(5) ? null : JsonSerializer.Deserialize<Speciality>(reader.GetString(5), jsonOptions),
                                Form = reader.IsDBNull(6) ? null : JsonSerializer.Deserialize<List<string>>(reader.GetString(6), jsonOptions),
                                Purpose = reader.GetString(7),
                                Years = reader.GetInt32(8),
                                Credits = reader.GetInt32(9),
                                Sum = reader.GetDecimal(10),
                                Costs = reader.IsDBNull(11) ? null : JsonSerializer.Deserialize<List<decimal>>(reader.GetString(11), jsonOptions),
                                ProgramCharacteristics = reader.IsDBNull(12) ? null : JsonSerializer.Deserialize<ProgramCharacteristics>(reader.GetString(12), jsonOptions),
                                ProgramCompetence = reader.IsDBNull(13) ? null : JsonSerializer.Deserialize<ProgramCompetence>(reader.GetString(13), jsonOptions),
                                Results = reader.IsDBNull(14) ? null : JsonSerializer.Deserialize<List<string>>(reader.GetString(14), jsonOptions),
                                LinkFaculty = reader.GetString(15),
                                LinkFile = reader.GetString(16),
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

                if (program != null)
                {
                    using (var cmd = new NpgsqlCommand(
                        "SELECT Id, ComponentType, ComponentName, ComponentCredits, ComponentHours, ControlForm " +
                        "FROM ProgramComponents " +
                        "WHERE ProgramId = @programId", connection))
                    {
                        cmd.Parameters.AddWithValue("programId", program.Id);
                        using var reader = cmd.ExecuteReader();
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
                                ControlForm = reader.IsDBNull(5) ? null : JsonSerializer.Deserialize<List<string>>(reader.GetString(5), jsonOptions)
                            });
                        }
                    }

                    using (var cmd = new NpgsqlCommand(
                        "SELECT j.Id, j.Code, j.Title " +
                        "FROM Jobs j " +
                        "JOIN ProgramJobs pj ON j.Id = pj.JobId " +
                        "WHERE pj.ProgramId = @programId", connection))
                    {
                        cmd.Parameters.AddWithValue("programId", program.Id);
                        using var reader = cmd.ExecuteReader();
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
            return program;
        }


        public List<ProgramsDegreeDto> GetProgramsDegrees(string degree = null)
        {
            var programs = new List<ProgramsDegreeDto>();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                var query = "SELECT Id, Degree, FieldOfStudy, Speciality, Name FROM Programs";
                if (!string.IsNullOrEmpty(degree))
                {
                    query += " WHERE Degree = @degree";
                }

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(degree))
                    {
                        cmd.Parameters.AddWithValue("degree", degree);
                    }

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            programs.Add(new ProgramsDegreeDto
                            {
                                Id = reader.GetInt32(0),
                                Degree = reader.GetString(1),
                                FieldOfStudy = reader.IsDBNull(2) ? null : JsonSerializer.Deserialize<FieldOfStudy>(reader.GetString(2), jsonOptions),
                                Speciality = reader.IsDBNull(3) ? null : JsonSerializer.Deserialize<Speciality>(reader.GetString(3), jsonOptions),
                                Name = reader.GetString(4)
                            });
                        }
                    }
                }
            }

            return programs;
        }

        public List<ProgramsFieldDto> GetProgramsFields()
        {
            var programs = new List<ProgramsFieldDto>();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("SELECT Id, Degree, FieldOfStudy FROM Programs", connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        programs.Add(new ProgramsFieldDto
                        {
                            Id = reader.GetInt32(0),
                            Degree = reader.GetString(1),
                            FieldOfStudy = reader.IsDBNull(2) ? null : JsonSerializer.Deserialize<FieldOfStudy>(reader.GetString(2), jsonOptions),
                        });
                    }
                }
            }

            return programs;
        }


        public void AddProgram(ProgramView program)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            int programId;
            using (var cmd = new NpgsqlCommand(
                "INSERT INTO Programs (Degree, Name, Name_Eng, FieldOfStudy, Speciality, Form, Years, Credits, Sum, Costs, " +
                "ProgramCharacteristics, ProgramCompetence, ProgramResults, LinkFaculty, LinkFile) " +
                "VALUES (@degree, @name, @nameEng, @fieldOfStudy, @speciality, @form, @years, @credits, @sum, @costs, " +
                "@programCharacteristics, @programCompetence, @programResults, @linkFaculty, @linkFile) " +
                "RETURNING Id", connection))
            {
                cmd.Parameters.AddWithValue("degree", program.Degree);
                cmd.Parameters.AddWithValue("name", program.Name);
                cmd.Parameters.AddWithValue("nameEng", (object)program.NameEng ?? DBNull.Value);
                cmd.Parameters.Add(new NpgsqlParameter("fieldOfStudy", NpgsqlTypes.NpgsqlDbType.Jsonb)
                {
                    Value = JsonSerializer.Serialize(program.FieldOfStudy, jsonOptions)
                });
                cmd.Parameters.Add(new NpgsqlParameter("speciality", NpgsqlTypes.NpgsqlDbType.Jsonb)
                {
                    Value = JsonSerializer.Serialize(program.Speciality, jsonOptions)
                });
                cmd.Parameters.Add(new NpgsqlParameter("form", NpgsqlTypes.NpgsqlDbType.Jsonb) { Value = JsonSerializer.Serialize(program.Form, jsonOptions) });
                cmd.Parameters.AddWithValue("years", program.Years);
                cmd.Parameters.AddWithValue("credits", program.Credits);
                cmd.Parameters.AddWithValue("sum", program.Sum);
                cmd.Parameters.Add(new NpgsqlParameter("costs", NpgsqlTypes.NpgsqlDbType.Jsonb) { Value = JsonSerializer.Serialize(program.Costs, jsonOptions) });
                cmd.Parameters.Add(new NpgsqlParameter("programCharacteristics", NpgsqlTypes.NpgsqlDbType.Jsonb) { Value = JsonSerializer.Serialize(program.ProgramCharacteristics, jsonOptions) });
                cmd.Parameters.Add(new NpgsqlParameter("programCompetence", NpgsqlTypes.NpgsqlDbType.Jsonb) { Value = JsonSerializer.Serialize(program.ProgramCompetence, jsonOptions) });
                cmd.Parameters.Add(new NpgsqlParameter("programResults", NpgsqlTypes.NpgsqlDbType.Jsonb) { Value = JsonSerializer.Serialize(program.Results, jsonOptions) });
                cmd.Parameters.AddWithValue("linkFaculty", program.LinkFaculty);
                cmd.Parameters.AddWithValue("linkFile", program.LinkFile);
                programId = (int)cmd.ExecuteScalar();
            }

            if (program.Components != null)
            {
                foreach (var component in program.Components)
                {
                    using var cmd = new NpgsqlCommand(
                        "INSERT INTO ProgramComponents (ProgramId, ComponentType, ComponentName, ComponentCredits, ComponentHours, ControlForm) " +
                        "VALUES (@programId, @componentType, @componentName, @componentCredits, @componentHours, @controlForm)", connection);
                    cmd.Parameters.AddWithValue("programId", programId);
                    cmd.Parameters.AddWithValue("componentType", component.ComponentType);
                    cmd.Parameters.AddWithValue("componentName", component.ComponentName);
                    cmd.Parameters.AddWithValue("componentCredits", component.ComponentCredits);
                    cmd.Parameters.AddWithValue("componentHours", component.ComponentHours);
                    cmd.Parameters.Add(new NpgsqlParameter("controlForm", NpgsqlTypes.NpgsqlDbType.Jsonb) { Value = JsonSerializer.Serialize(component.ControlForm, jsonOptions) });

                    cmd.Parameters.AddWithValue("controlForm", JsonSerializer.Serialize(component.ControlForm, jsonOptions));
                    cmd.ExecuteNonQuery();
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
                            using var insertCmd = new NpgsqlCommand(
                                "INSERT INTO Jobs (Code, Title) VALUES (@code, @title) RETURNING Id", connection);
                            insertCmd.Parameters.AddWithValue("code", job.Code);
                            insertCmd.Parameters.AddWithValue("title", job.Title);
                            jobId = (int)insertCmd.ExecuteScalar();
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
