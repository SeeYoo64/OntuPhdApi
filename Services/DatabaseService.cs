using Npgsql;
using Npgsql.Internal.Postgres;
using OntuPhdApi.Models;
using System.Reflection.Metadata;
using System.Text.Json;

namespace OntuPhdApi.Services
{
    public class DatabaseService(IConfiguration configuration)
    {
        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection");

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
                    "SELECT Id, Degree, Name, Name_Eng, FieldOfStudy, Speciality, Form, Years, Credits, Sum, Costs, " +
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
                                Years = reader.GetInt32(7),
                                Credits = reader.GetInt32(8),
                                Sum = reader.GetDecimal(9),
                                Costs = reader.IsDBNull(10) ? null : JsonSerializer.Deserialize<List<decimal>>(reader.GetString(10), jsonOptions),
                                ProgramCharacteristics = reader.IsDBNull(11) ? null : JsonSerializer.Deserialize<ProgramCharacteristics>(reader.GetString(11), jsonOptions),
                                ProgramCompetence = reader.IsDBNull(12) ? null : JsonSerializer.Deserialize<ProgramCompetence>(reader.GetString(12), jsonOptions),
                                Results = reader.IsDBNull(13) ? null : JsonSerializer.Deserialize<List<string>>(reader.GetString(13), jsonOptions),
                                LinkFaculty = reader.GetString(14),
                                LinkFile = reader.GetString(15),
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
                    "SELECT Id, Degree, Name, Name_Eng, FieldOfStudy, Speciality, Form, Years, Credits, Sum, Costs, " +
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
                                Years = reader.GetInt32(7),
                                Credits = reader.GetInt32(8),
                                Sum = reader.GetDecimal(9),
                                Costs = reader.IsDBNull(10) ? null : JsonSerializer.Deserialize<List<decimal>>(reader.GetString(10), jsonOptions),
                                ProgramCharacteristics = reader.IsDBNull(11) ? null : JsonSerializer.Deserialize<ProgramCharacteristics>(reader.GetString(11), jsonOptions),
                                ProgramCompetence = reader.IsDBNull(12) ? null : JsonSerializer.Deserialize<ProgramCompetence>(reader.GetString(12), jsonOptions),
                                Results = reader.IsDBNull(13) ? null : JsonSerializer.Deserialize<List<string>>(reader.GetString(13), jsonOptions),
                                LinkFaculty = reader.GetString(14),
                                LinkFile = reader.GetString(15),
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


        public List<Documents> GetDocuments()
        {
            var documents = new List<Documents>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using var cmd = new NpgsqlCommand("SELECT Id, ProgramId, Name, Type, Link FROM Documents", connection);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    documents.Add(new Documents
                    {
                        Id = reader.GetInt32(0),
                        ProgramId = reader.IsDBNull(1) ? null : reader.GetInt32(1),
                        Name = reader.GetString(2),
                        Type = reader.GetString(3),
                        Link = reader.GetString(4)
                    });
                }
            }

            return documents;
        }


        public Documents GetDocumentById(int id)
        {
            Documents document = null;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using var cmd = new NpgsqlCommand("SELECT Id, ProgramId, Name, Type, Link FROM Documents WHERE Id = @id", connection);
                cmd.Parameters.AddWithValue("id", id);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    document = new Documents
                    {
                        Id = reader.GetInt32(0),
                        ProgramId = reader.IsDBNull(1) ? null : reader.GetInt32(1),
                        Name = reader.GetString(2),
                        Type = reader.GetString(3),
                        Link = reader.GetString(4)
                    };
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

                using var cmd = new NpgsqlCommand("SELECT Id, ProgramId, Name, Type, Link FROM Documents WHERE Type = @type", connection);
                cmd.Parameters.AddWithValue("type", type);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    documents.Add(new Documents
                    {
                        Id = reader.GetInt32(0),
                        ProgramId = reader.IsDBNull(1) ? null : reader.GetInt32(1),
                        Name = reader.GetString(2),
                        Type = reader.GetString(3),
                        Link = reader.GetString(4)
                    });
                }
            }

            return documents;
        }

        public void AddDocument(Documents document)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(
                "INSERT INTO Documents (ProgramId, Name, Type, Link) " +
                "VALUES (@programId, @name, @type, @link) RETURNING Id", connection);
            cmd.Parameters.AddWithValue("programId", (object)document.ProgramId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("name", document.Name);
            cmd.Parameters.AddWithValue("type", document.Type);
            cmd.Parameters.AddWithValue("link", document.Link);
            document.Id = (int)cmd.ExecuteScalar();
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

                using var cmd = new NpgsqlCommand("SELECT Id, Name, Description, Requirements, OriginalsRequired FROM ApplyDocuments", connection);
                using var reader = cmd.ExecuteReader();
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

                using var cmd = new NpgsqlCommand("SELECT Id, Name, Description, Requirements, OriginalsRequired FROM ApplyDocuments WHERE Id = @id", connection);
                cmd.Parameters.AddWithValue("id", id);
                using var reader = cmd.ExecuteReader();
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

                using var cmd = new NpgsqlCommand("SELECT Id, Name, Description, Requirements, OriginalsRequired FROM ApplyDocuments WHERE Name ILIKE @name", connection);
                cmd.Parameters.AddWithValue("name", $"%{name}%");
                using var reader = cmd.ExecuteReader();
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

            return applyDocuments;
        }


        public void AddApplyDocument(ApplyDocuments applyDocument)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(
                "INSERT INTO ApplyDocuments (Name, Description, Requirements, OriginalsRequired) " +
                "VALUES (@name, @description, @requirements, @originalsRequired) RETURNING Id", connection);
            cmd.Parameters.AddWithValue("name", applyDocument.Name);
            cmd.Parameters.AddWithValue("description", applyDocument.Description);
            cmd.Parameters.Add(new NpgsqlParameter("requirements", NpgsqlTypes.NpgsqlDbType.Jsonb) { Value = JsonSerializer.Serialize(applyDocument.Requirements, jsonOptions) });
            cmd.Parameters.Add(new NpgsqlParameter("originalsRequired", NpgsqlTypes.NpgsqlDbType.Jsonb) { Value = JsonSerializer.Serialize(applyDocument.OriginalsRequired, jsonOptions) });
            applyDocument.Id = (int)cmd.ExecuteScalar();
        }


        public List<Roadmap> GetRoadmaps()
        {
            var roadmaps = new List<Roadmap>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("SELECT Id, Type, DataStart, DataEnd, AdditionalTime, Description FROM Roadmaps", connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        roadmaps.Add(new Roadmap
                        {
                            Id = reader.GetInt32(0),
                            Type = reader.GetString(1),
                            DataStart = reader.GetDateTime(2),
                            DataEnd = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                            AdditionalTime = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Description = reader.GetString(5)
                        });
                    }
                }
            }

            return roadmaps;
        }


        public Roadmap GetRoadmapById(int id)
        {
            Roadmap roadmap = null;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("SELECT Id, Type, DataStart, DataEnd, AdditionalTime, Description FROM Roadmaps WHERE Id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            roadmap = new Roadmap
                            {
                                Id = reader.GetInt32(0),
                                Type = reader.GetString(1),
                                DataStart = reader.GetDateTime(2),
                                DataEnd = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                                AdditionalTime = reader.IsDBNull(4) ? null : reader.GetString(4),
                                Description = reader.GetString(5)
                            };
                        }
                    }
                }
            }

            return roadmap;
        }

        public List<Roadmap> GetRoadmapsByType(string type)
        {
            var roadmaps = new List<Roadmap>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("SELECT Id, Type, DataStart, DataEnd, AdditionalTime, Description FROM Roadmaps WHERE Type = @type", connection))
                {
                    cmd.Parameters.AddWithValue("type", type);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            roadmaps.Add(new Roadmap
                            {
                                Id = reader.GetInt32(0),
                                Type = reader.GetString(1),
                                DataStart = reader.GetDateTime(2),
                                DataEnd = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                                AdditionalTime = reader.IsDBNull(4) ? null : reader.GetString(4),
                                Description = reader.GetString(5)
                            });
                        }
                    }
                }
            }

            return roadmaps;
        }


        public void AddRoadmap(Roadmap roadmap)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand(
                    "INSERT INTO Roadmaps (Type, DataStart, DataEnd, AdditionalTime, Description) " +
                    "VALUES (@type, @dataStart, @dataEnd, @additionalTime, @description) RETURNING Id", connection))
                {
                    cmd.Parameters.AddWithValue("type", roadmap.Type);
                    cmd.Parameters.AddWithValue("dataStart", roadmap.DataStart);
                    cmd.Parameters.AddWithValue("dataEnd", (object)roadmap.DataEnd ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("additionalTime", (object)roadmap.AdditionalTime ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("description", roadmap.Description);
                    roadmap.Id = (int)cmd.ExecuteScalar();
                }
            }
        }


        public List<News> GetNews()
        {
            var newsList = new List<News>();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("SELECT Id, Title, Summary, MainTag, OtherTags, Date, Thumbnail, Photos, Body FROM News", connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        try
                        {
                            newsList.Add(new News
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Summary = reader.GetString(2),
                                MainTag = reader.GetString(3),
                                OtherTags = JsonSerializer.Deserialize<List<string>>(reader.GetString(4), jsonOptions),
                                Date = reader.GetDateTime(5),
                                Thumbnail = reader.GetString(6),
                                Photos = JsonSerializer.Deserialize<List<string>>(reader.GetString(7), jsonOptions),
                                Body = JsonSerializer.Deserialize<List<string>>(reader.GetString(8), jsonOptions)
                            });
                        }
                        catch (JsonException ex)
                        {
                            Console.WriteLine($"Error deserializing News with ID {reader.GetInt32(0)}: {ex.Message}");
                        }
                    }
                }
            }

            return newsList;
        }

        public News GetNewsById(int id)
        {
            News news = null;
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("SELECT Id, Title, Summary, MainTag, OtherTags, Date, Thumbnail, Photos, Body FROM News WHERE Id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            try
                            {
                                news = new News
                                {
                                    Id = reader.GetInt32(0),
                                    Title = reader.GetString(1),
                                    Summary = reader.GetString(2),
                                    MainTag = reader.GetString(3),
                                    OtherTags = JsonSerializer.Deserialize<List<string>>(reader.GetString(4), jsonOptions),
                                    Date = reader.GetDateTime(5),
                                    Thumbnail = reader.GetString(6),
                                    Photos = JsonSerializer.Deserialize<List<string>>(reader.GetString(7), jsonOptions),
                                    Body = JsonSerializer.Deserialize<List<string>>(reader.GetString(8), jsonOptions)
                                };
                            }
                            catch (JsonException ex)
                            {
                                throw new Exception($"Error deserializing News with ID {id}: {ex.Message}");
                            }
                        }
                    }
                }
            }

            return news;
        }

        public void UpdateNews(News news)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand(
                    "UPDATE News SET Title = @title, Summary = @summary, MainTag = @mainTag, OtherTags = @otherTags, " +
                    "Date = @date, Thumbnail = @thumbnail, Photos = @photos, Body = @body WHERE Id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("id", news.Id);
                    cmd.Parameters.AddWithValue("title", news.Title);
                    cmd.Parameters.AddWithValue("summary", news.Summary);
                    cmd.Parameters.AddWithValue("mainTag", news.MainTag);
                    cmd.Parameters.AddWithValue("otherTags", JsonSerializer.Serialize(news.OtherTags, jsonOptions));
                    cmd.Parameters.AddWithValue("date", news.Date);
                    cmd.Parameters.AddWithValue("thumbnail", news.Thumbnail);
                    cmd.Parameters.AddWithValue("photos", JsonSerializer.Serialize(news.Photos, jsonOptions));
                    cmd.Parameters.AddWithValue("body", JsonSerializer.Serialize(news.Body, jsonOptions));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<News> GetLatestNews(int count = 4)
        {
            var newsList = new List<News>();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand(
                    "SELECT Id, Title, Summary, MainTag, OtherTags, Date, Thumbnail, Photos, Body " +
                    "FROM News " +
                    "ORDER BY Date DESC " +
                    "LIMIT @count", connection))
                {
                    cmd.Parameters.AddWithValue("count", count);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                newsList.Add(new News
                                {
                                    Id = reader.GetInt32(0),
                                    Title = reader.GetString(1),
                                    Summary = reader.GetString(2),
                                    MainTag = reader.GetString(3),
                                    OtherTags = JsonSerializer.Deserialize<List<string>>(reader.GetString(4), jsonOptions),
                                    Date = reader.GetDateTime(5),
                                    Thumbnail = reader.GetString(6),
                                    Photos = JsonSerializer.Deserialize<List<string>>(reader.GetString(7), jsonOptions),
                                    Body = JsonSerializer.Deserialize<List<string>>(reader.GetString(8), jsonOptions)
                                });
                            }
                            catch (JsonException ex)
                            {
                                Console.WriteLine($"Error deserializing News with ID {reader.GetInt32(0)}: {ex.Message}");
                            }
                        }
                    }
                }
            }

            return newsList;
        }

        public void AddNews(News news)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand(
                    "INSERT INTO News (Title, Summary, MainTag, OtherTags, Date, Thumbnail, Photos, Body) " +
                    "VALUES (@title, @summary, @mainTag, @otherTags, @date, @thumbnail, @photos, @body) RETURNING Id", connection))
                {
                    cmd.Parameters.AddWithValue("title", news.Title);
                    cmd.Parameters.AddWithValue("summary", news.Summary);
                    cmd.Parameters.AddWithValue("mainTag", news.MainTag);
                    cmd.Parameters.Add(new NpgsqlParameter("otherTags", NpgsqlTypes.NpgsqlDbType.Jsonb) { Value = JsonSerializer.Serialize(news.OtherTags, jsonOptions) });
                    cmd.Parameters.AddWithValue("date", news.Date);
                    cmd.Parameters.AddWithValue("thumbnail", news.Thumbnail);
                    cmd.Parameters.Add(new NpgsqlParameter("photos", NpgsqlTypes.NpgsqlDbType.Jsonb) { Value = JsonSerializer.Serialize(news.Photos, jsonOptions) });
                    cmd.Parameters.Add(new NpgsqlParameter("body", NpgsqlTypes.NpgsqlDbType.Jsonb) { Value = JsonSerializer.Serialize(news.Body, jsonOptions) });
                    news.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

    }
}