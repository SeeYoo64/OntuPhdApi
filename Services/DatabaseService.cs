using Npgsql;
using OntuPhdApi.Models;
using System.Reflection.Metadata;
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
                    "SELECT Id, Name, Name_Eng, FieldOfStudy, Speciality, Form, Years, Credits, Sum, Costs, " +
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
                                NameEng = reader.IsDBNull(2) ? null : reader.GetString(2), // Читаем name_eng
                                FieldOfStudy = reader.IsDBNull(3) ? null : JsonSerializer.Deserialize<FieldOfStudy>(reader.GetString(3), jsonOptions),
                                Speciality = reader.IsDBNull(4) ? null : JsonSerializer.Deserialize<Speciality>(reader.GetString(4), jsonOptions),
                                Form = reader.IsDBNull(5) ? null : JsonSerializer.Deserialize<List<string>>(reader.GetString(5), jsonOptions),
                                Years = reader.GetInt32(6),
                                Credits = reader.GetInt32(7),
                                Sum = reader.GetDecimal(8),
                                Costs = reader.IsDBNull(9) ? null : JsonSerializer.Deserialize<List<decimal>>(reader.GetString(9), jsonOptions),
                                ProgramCharacteristics = reader.IsDBNull(10) ? null : JsonSerializer.Deserialize<ProgramCharacteristics>(reader.GetString(10), jsonOptions),
                                ProgramCompetence = reader.IsDBNull(11) ? null : JsonSerializer.Deserialize<ProgramCompetence>(reader.GetString(11), jsonOptions),
                                ProgramResults = reader.IsDBNull(12) ? null : JsonSerializer.Deserialize<ProgramResults>(reader.GetString(12), jsonOptions),
                                LinkFaculty = reader.GetString(13),
                                LinkFile = reader.GetString(14),
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
                                    ControlForm = reader.IsDBNull(5) ? null : JsonSerializer.Deserialize<List<string>>(reader.GetString(5), jsonOptions)
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
                    "SELECT Id, Name, Name_Eng, FieldOfStudy, Speciality, Form, Years, Credits, Sum, Costs, " +
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
                                    NameEng = reader.IsDBNull(2) ? null : reader.GetString(2), // Читаем name_eng
                                    FieldOfStudy = reader.IsDBNull(3) ? null : JsonSerializer.Deserialize<FieldOfStudy>(reader.GetString(3), jsonOptions),
                                    Speciality = reader.IsDBNull(4) ? null : JsonSerializer.Deserialize<Speciality>(reader.GetString(4), jsonOptions),
                                    Form = reader.IsDBNull(5) ? null : JsonSerializer.Deserialize<List<string>>(reader.GetString(5), jsonOptions),
                                    Years = reader.GetInt32(6),
                                    Credits = reader.GetInt32(7),
                                    Sum = reader.GetDecimal(8),
                                    Costs = reader.IsDBNull(9) ? null : JsonSerializer.Deserialize<List<decimal>>(reader.GetString(9), jsonOptions),
                                    ProgramCharacteristics = reader.IsDBNull(10) ? null : JsonSerializer.Deserialize<ProgramCharacteristics>(reader.GetString(10), jsonOptions),
                                    ProgramCompetence = reader.IsDBNull(11) ? null : JsonSerializer.Deserialize<ProgramCompetence>(reader.GetString(11), jsonOptions),
                                    ProgramResults = reader.IsDBNull(12) ? null : JsonSerializer.Deserialize<ProgramResults>(reader.GetString(12), jsonOptions),
                                    LinkFaculty = reader.GetString(13),
                                    LinkFile = reader.GetString(14),
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
                                    ControlForm = reader.IsDBNull(5) ? null : JsonSerializer.Deserialize<List<string>>(reader.GetString(5), jsonOptions)
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
                    "INSERT INTO Programs (Name, Name_Eng, FieldOfStudy, Speciality, Form, Years, Credits, Sum, Costs, " +
                    "ProgramCharacteristics, ProgramCompetence, ProgramResults, LinkFaculty, LinkFile) " +
                    "VALUES (@name, @nameEng, @fieldOfStudy, @speciality, @form, @years, @credits, @sum, @costs, " +
                    "@programCharacteristics, @programCompetence, @programResults, @linkFaculty, @linkFile) " +
                    "RETURNING Id", connection))
                {
                    cmd.Parameters.AddWithValue("name", program.Name);
                    cmd.Parameters.AddWithValue("nameEng", (object)program.NameEng ?? DBNull.Value); // Учитываем, что nameEng может быть null
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
    
        
        public List<Documents> GetDocuments()
        {
            var documents = new List<Documents>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("Select Id, ProgramId, Name, Type, Link From Documents", connection))

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        documents.Add(new Documents
                        {
                            Id = reader.GetInt32(0),
                            ProgramId = reader.GetInt32(1),
                            Name = reader.GetString(2),
                            Type = reader.GetString(3),
                            Link = reader.GetString(4)
                        });
                    }
                }
            }

            return documents;

        }


        public Documents GetDocumentsById(int id)
        {
            Documents document = null;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("Select Id, ProgramId, Name, Type, Link From Documents WHERE Id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            document = new Documents
                            {
                                Id = reader.GetInt32(0),
                                ProgramId = reader.GetInt32(1),
                                Name = reader.GetString(2),
                                Type = reader.GetString(3),
                                Link = reader.GetString(4)
                            };
                        }
                    }
                }
            }
            return document;
        }

        public List<Documents> GetDocumentsByType(string type)
        {
            var documents = new List<Documents>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("SELECT Id, ProgramId, Name, Type, Link FROM Documents WHERE Type = @type", connection))
                {
                    cmd.Parameters.AddWithValue("type", type);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            documents.Add(new Documents
                            {
                                Id = reader.GetInt32(0),
                                ProgramId = reader.GetInt32(1),
                                Name = reader.GetString(2),
                                Type = reader.GetString(3),
                                Link = reader.GetString(4)
                            });
                        }
                    }
                }
            }

            return documents;
        }

        public void AddDocument(Documents document)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand(
                    "INSERT INTO Documents (ProgramId, Name, Type, Link) " +
                    "VALUES (@programId, @name, @type, @link) RETURNING Id", connection))
                {
                    cmd.Parameters.AddWithValue("programId", document.ProgramId);
                    cmd.Parameters.AddWithValue("name", document.Name);
                    cmd.Parameters.AddWithValue("type", document.Type);
                    cmd.Parameters.AddWithValue("link", document.Link);
                    document.Id = (int)cmd.ExecuteScalar();
                }
            }
        }


        public List<ApplyDocuments> GetApplyDocuments()
        {
            var applyDocuments = new List<ApplyDocuments>();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("SELECT Id, Name, Description, Requirements, OriginalsRequired FROM ApplyDocuments", connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        try
                        {
                            applyDocuments.Add(new ApplyDocuments
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.GetString(2),
                                Requirements = JsonSerializer.Deserialize<List<Requirements>>(reader.GetString(3), jsonOptions),
                                OriginalsRequired = JsonSerializer.Deserialize<List<Requirements>>(reader.GetString(4), jsonOptions)
                            });
                        }
                        catch (JsonException ex)
                        {
                            Console.WriteLine($"Error deserializing ApplyDocuments with ID {reader.GetInt32(0)}: {ex.Message}");
                        }
                    }
                }
            }

            return applyDocuments;
        }


        public ApplyDocuments GetApplyDocumentById(int id)
        {
            ApplyDocuments applyDocument = null;
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("SELECT Id, Name, Description, Requirements, OriginalsRequired FROM ApplyDocuments WHERE Id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            try
                            {
                                applyDocument = new ApplyDocuments
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Description = reader.GetString(2),
                                    Requirements = JsonSerializer.Deserialize<List<Requirements>>(reader.GetString(3), jsonOptions),
                                    OriginalsRequired = JsonSerializer.Deserialize<List<Requirements>>(reader.GetString(4), jsonOptions)
                                };
                            }
                            catch (JsonException ex)
                            {
                                throw new Exception($"Error deserializing ApplyDocuments with ID {id}: {ex.Message}");
                            }
                        }
                    }
                }
            }

            return applyDocument;
        }


        public List<ApplyDocuments> GetApplyDocumentsByName(string name)
        {
            var applyDocuments = new List<ApplyDocuments>();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("SELECT Id, Name, Description, Requirements, OriginalsRequired FROM ApplyDocuments WHERE Name ILIKE @name", connection))
                {
                    cmd.Parameters.AddWithValue("name", $"%{name}%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                applyDocuments.Add(new ApplyDocuments
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Description = reader.GetString(2),
                                    Requirements = JsonSerializer.Deserialize<List<Requirements>>(reader.GetString(3), jsonOptions),
                                    OriginalsRequired = JsonSerializer.Deserialize<List<Requirements>>(reader.GetString(4), jsonOptions)
                                });
                            }
                            catch (JsonException ex)
                            {
                                Console.WriteLine($"Error deserializing ApplyDocuments with ID {reader.GetInt32(0)}: {ex.Message}");
                            }
                        }
                    }
                }
            }

            return applyDocuments;
        }


        public void AddApplyDocument(ApplyDocuments applyDocument)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand(
                    "INSERT INTO ApplyDocuments (Name, Description, Requirements, OriginalsRequired) " +
                    "VALUES (@name, @description, @requirements, @originalsRequired) RETURNING Id", connection))
                {
                    cmd.Parameters.AddWithValue("name", applyDocument.Name);
                    cmd.Parameters.AddWithValue("description", applyDocument.Description);
                    cmd.Parameters.AddWithValue("requirements", JsonSerializer.Serialize(applyDocument.Requirements, jsonOptions));
                    cmd.Parameters.AddWithValue("originalsRequired", JsonSerializer.Serialize(applyDocument.Requirements, jsonOptions));
                    applyDocument.Id = (int)cmd.ExecuteScalar();
                }
            }
        }



    }
}