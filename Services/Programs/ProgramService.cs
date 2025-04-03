using Microsoft.Extensions.Configuration;
using Npgsql;
using OntuPhdApi.Models;
using OntuPhdApi.Models.Programs;
using System;
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
        public List<ProgramModel> GetPrograms()
        {
            var programs = new List<ProgramModel>();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(
                "SELECT Id, Degree, Name, Name_Code, Field_Of_Study, Speciality, Form, Purpose, Years, Credits, " +
                "Program_Characteristics, Program_Competence, Results, Link_Faculty, Link_File, Accredited " +
                "FROM Program", connection))

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        try
                        {
                            var program = new ProgramModel
                            {
                                Id = reader.GetInt32(0),
                                Degree = reader.GetString(1),
                                Name = reader.GetString(2),
                                NameCode = reader.IsDBNull(3) ? null : reader.GetString(3),
                                FieldOfStudy = reader.IsDBNull(4) ? null : JsonSerializer.Deserialize<FieldOfStudy>(reader.GetString(4), jsonOptions),
                                Speciality = reader.IsDBNull(5) ? null : JsonSerializer.Deserialize<Speciality>(reader.GetString(5), jsonOptions),
                                Form = reader.IsDBNull(6) ? null : JsonSerializer.Deserialize<List<string>>(reader.GetString(6), jsonOptions),
                                Purpose = reader.IsDBNull(7) ? null : reader.GetString(7),
                                Years = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8),
                                Credits = reader.IsDBNull(9) ? (int?)null : reader.GetInt32(9),
                                ProgramCharacteristics = reader.IsDBNull(10) ? null : JsonSerializer.Deserialize<ProgramCharacteristics>(reader.GetString(10), jsonOptions),
                                ProgramCompetence = reader.IsDBNull(11) ? null : JsonSerializer.Deserialize<ProgramCompetence>(reader.GetString(11), jsonOptions),
                                Results = reader.IsDBNull(12) ? null : JsonSerializer.Deserialize<List<string>>(reader.GetString(12), jsonOptions),
                                LinkFaculty = reader.IsDBNull(13) ? null : reader.GetString(13),
                                LinkFile = reader.IsDBNull(14) ? null : reader.GetString(14),
                                Accredited = reader.GetBoolean(15),
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
                        "FROM programcomponents " +
                        "WHERE program_id = @programId", connection))
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
                                ControlForm = reader.IsDBNull(5) ? null : JsonSerializer.Deserialize<List<string>>(reader.GetString(5), jsonOptions) ?? throw new Exception($"Failed to deserialize ControlForm for component ID {reader.GetInt32(0)}")
                            });
                        }
                    }

                    using (var cmd = new NpgsqlCommand(
                        "SELECT Id, Code, Title " +
                        "FROM Job " +
                        "WHERE program_id = @programId", connection))
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

        public Object GetProgramById(int id)
        {
            ProgramModel program = null;
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand(
                    "SELECT Id, Degree, Name, Name_Code, Field_Of_Study, Speciality, Form, Purpose, Years, Credits, " +
                    "Program_Characteristics, Program_Competence, Results, Link_Faculty, Link_File, Accredited, Objects, Directions  " +
                    "FROM Program WHERE Id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        try
                        {
                            program = new ProgramModel
                            {
                                Id = reader.GetInt32(0),
                                Degree = reader.GetString(1),
                                Name = reader.GetString(2),
                                NameCode = reader.IsDBNull(3) ? null : reader.GetString(3),
                                FieldOfStudy = reader.IsDBNull(4) ? null : JsonSerializer.Deserialize<FieldOfStudy>(reader.GetString(4), jsonOptions),
                                Speciality = reader.IsDBNull(5) ? null : JsonSerializer.Deserialize<Speciality>(reader.GetString(5), jsonOptions),
                                Form = reader.IsDBNull(6) ? null : JsonSerializer.Deserialize<List<string>>(reader.GetString(6), jsonOptions),
                                Purpose = reader.IsDBNull(7) ? null : reader.GetString(7),
                                Years = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8),
                                Credits = reader.IsDBNull(9) ? (int?)null : reader.GetInt32(9),
                                ProgramCharacteristics = reader.IsDBNull(10) ? null : JsonSerializer.Deserialize<ProgramCharacteristics>(reader.GetString(10), jsonOptions),
                                ProgramCompetence = reader.IsDBNull(11) ? null : JsonSerializer.Deserialize<ProgramCompetence>(reader.GetString(11), jsonOptions),
                                Results = reader.IsDBNull(12) ? null : JsonSerializer.Deserialize<List<string>>(reader.GetString(12), jsonOptions),
                                LinkFaculty = reader.IsDBNull(13) ? null : reader.GetString(13),
                                LinkFile = reader.IsDBNull(14) ? null : reader.GetString(14),
                                Accredited = reader.GetBoolean(15),
                                Components = new List<ProgramComponent>(),
                                Jobs = new List<Job>(),
                                Object = reader.IsDBNull(16) ? null : reader.GetString(16),
                                Directions = reader.IsDBNull(17) ? null : JsonSerializer.Deserialize<List<string>>(reader.GetString(17), jsonOptions)

                            };
                        }
                        catch (JsonException ex)
                        {
                            Console.WriteLine($"Error deserializing program with ID {reader.GetInt32(0)}: {ex.Message}");
                        }
                    }

                }

                using (var cmd = new NpgsqlCommand(
                        "SELECT Id, ComponentType, ComponentName, ComponentCredits, ComponentHours, ControlForm " +
                        "FROM programcomponents " +
                        "WHERE program_id = @programId", connection))
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
                            ControlForm = reader.IsDBNull(5) ? null : JsonSerializer.Deserialize<List<string>>(reader.GetString(5), jsonOptions) ?? throw new Exception($"Failed to deserialize ControlForm for component ID {reader.GetInt32(0)}")
                        });
                    }
                }

                using (var cmd = new NpgsqlCommand(
                    "SELECT Id, Code, Title " +
                    "FROM Job " +
                    "WHERE program_id = @programId", connection))
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
                var Speciality = program.Speciality;
                ShortSpeciality shortSpeciality = new ShortSpeciality
                {
                    Code = Speciality.Code,
                    Name = Speciality.Name
                };
                switch (program.Degree.ToLower())
                {
                                            
                    case "phd":
                        return new ProgramModelPhd
                        {
                            Id = program.Id,
                            Degree = program.Degree,
                            Name = program.Name,
                            FieldOfStudy = program.FieldOfStudy,
                            Speciality = shortSpeciality,
                            Form = program.Form,
                            Purpose = program.Purpose,
                            Years = program.Years,
                            Credits = program.Credits,
                            ProgramCharacteristics = program.ProgramCharacteristics,
                            LinkFaculty = program.LinkFaculty,
                            LinkFile = program.LinkFile
                        };
                    case "doc":

                        return new ProgramModelDoc
                        {
                            Id = program.Id,
                            Degree = program.Degree,
                            Name = program.Name,
                            NameCode = program.NameCode,
                            Accredited = program.Accredited,
                            FieldOfStudy = program.FieldOfStudy,
                            Speciality = shortSpeciality,
                            Form = program.Form,
                            Description = program.Purpose,
                            Objects = program.Object,
                            Directions = program.Directions,
                            LinkFaculty = program.LinkFaculty,
                            LinkFile = program.LinkFile,
                        };
                    default:
                        throw new ArgumentException("Unsupported degree type");
                }

            }
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

                var query = "SELECT Id, Degree, Name, Field_Of_Study, Speciality  FROM Program";
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
                                Name = reader.GetString(2),
                                FieldOfStudy = reader.IsDBNull(3) ? null : JsonSerializer.Deserialize<FieldOfStudy>(reader.GetString(3), jsonOptions),
                                Speciality = reader.IsDBNull(4) ? null : JsonSerializer.Deserialize<ShortSpeciality>(reader.GetString(4), jsonOptions)
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

                using (var cmd = new NpgsqlCommand("SELECT Id, Degree, Field_Of_Study FROM Program", connection))
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


        public void AddProgram(ProgramModel program)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            int programId;
            using (var cmd = new NpgsqlCommand(
                "INSERT INTO Programs (Degree, Name, Name_Eng, FieldOfStudy, Speciality, Form, Years, Credits, " +
                "ProgramCharacteristics, ProgramCompetence, ProgramResults, LinkFaculty, LinkFile) " +
                "VALUES (@degree, @name, @nameEng, @fieldOfStudy, @speciality, @form, @years, @credits, @sum, @costs, " +
                "@programCharacteristics, @programCompetence, @programResults, @linkFaculty, @linkFile) " +
                "RETURNING Id", connection))
            {
                cmd.Parameters.AddWithValue("degree", program.Degree);
                cmd.Parameters.AddWithValue("name", program.Name);
                cmd.Parameters.AddWithValue("nameEng", (object)program.NameCode ?? DBNull.Value);
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
