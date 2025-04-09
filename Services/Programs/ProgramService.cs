using Npgsql;
using NpgsqlTypes;
using OntuPhdApi.Controllers;
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
                "Program_Characteristics, Program_Competence, Results, Link_Faculty, programdocumentid, Accredited, Directions, Objects " +
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
                                ProgramDocumentId = reader.IsDBNull(14) ? 0 : reader.GetInt32(14),
                                Accredited = reader.GetBoolean(15),
                                Directions = reader.IsDBNull(16) ? null : JsonSerializer.Deserialize<List<string>>(reader.GetString(16), jsonOptions),
                                Objects = reader.IsDBNull(17) ? null : reader.GetString(17),
                        //        Components = new List<ProgramComponent>(),
                        //        Jobs = new List<Job>()
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

        public async Task<ProgramModel> GetProgram(int id)
        {
            ProgramModel program = null;
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var cmd = new NpgsqlCommand(
                    "SELECT p.Id, p.Degree, p.Name, p.Name_Code, p.Field_Of_Study, p.Speciality, p.Form, p.Purpose, p.Years, p.Credits, " +
                    "p.Program_Characteristics, p.Program_Competence, p.Results, p.Link_Faculty, p.programdocumentid, p.Accredited, p.Objects, p.Directions, " +
                    "p.Descriptions, d.FileName, d.FilePath " +
                    "FROM Program p " +
                    "LEFT JOIN Programdocuments d ON p.programdocumentid = d.Id " +
                    "WHERE p.Id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using var reader = await cmd.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
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
                                Descriptions = reader.IsDBNull(18) ? null : reader.GetString(18),
                                Years = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8),
                                Credits = reader.IsDBNull(9) ? (int?)null : reader.GetInt32(9),
                                ProgramCharacteristics = reader.IsDBNull(10) ? null : JsonSerializer.Deserialize<ProgramCharacteristics>(reader.GetString(10), jsonOptions),
                                ProgramCompetence = reader.IsDBNull(11) ? null : JsonSerializer.Deserialize<ProgramCompetence>(reader.GetString(11), jsonOptions),
                                Results = reader.IsDBNull(12) ? null : JsonSerializer.Deserialize<List<string>>(reader.GetString(12), jsonOptions),
                                LinkFaculty = reader.IsDBNull(13) ? null : reader.GetString(13),
                                ProgramDocumentId = reader.IsDBNull(14) ? 0 : reader.GetInt32(14),
                                Accredited = reader.GetBoolean(15),
                                Objects = reader.IsDBNull(16) ? null : reader.GetString(16),
                                Directions = reader.IsDBNull(17) ? null : JsonSerializer.Deserialize<List<string>>(reader.GetString(17), jsonOptions),
                               // Components = new List<ProgramComponent>(),
                               // Jobs = new List<Job>()
                            };
                        }
                        catch (JsonException ex)
                        {
                            Console.WriteLine($"Error deserializing program with ID {id}: {ex.Message}");
                            throw;
                        }
                    }
                    else
                    {
                        return null; 
                    }
                }

                using (var cmd = new NpgsqlCommand(
                    "SELECT Id, ComponentType, ComponentName, ComponentCredits, ComponentHours, ControlForm " +
                    "FROM programcomponents " +
                    "WHERE program_id = @programId", connection))
                {
                    cmd.Parameters.AddWithValue("programId", program.Id);
                    using var reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
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
                    "SELECT Id, Code, Title " +
                    "FROM Job " +
                    "WHERE program_id = @programId", connection))
                {
                    cmd.Parameters.AddWithValue("programId", program.Id);
                    using var reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        program.Jobs.Add(new Job
                        {
                            Id = reader.GetInt32(0),
                            Code = reader.GetString(1),
                            Title = reader.GetString(2)
                        });
                    }
                }

                return program;
            }
        }

        public List<ProgramsDegreeDto> GetProgramsDegrees(DegreeType? degree)
        {
            var programs = new List<ProgramsDegreeDto>();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                var query = "SELECT Id, Degree, Name, Field_Of_Study, Speciality FROM Program";
                if (degree.HasValue) 
                {
                    query += " WHERE Degree = @degree";
                }

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    if (degree.HasValue)
                    {
                        cmd.Parameters.AddWithValue("degree", degree.Value.ToString());
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

        public async Task AddProgram(ProgramModel program, string filePath, string contentType, long fileSize)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Сохранение документа
                var documentQuery = "INSERT INTO Documents (FileName, FilePath, FileSize, ContentType) VALUES (@FileName, @FilePath, @FileSize, @ContentType) RETURNING Id";
                int documentId;
                using (var cmd = new NpgsqlCommand(documentQuery, connection))
                {
                    cmd.Parameters.AddWithValue("FileName", program.Name + Path.GetExtension(filePath)); // Имя файла можно связать с программой
                    cmd.Parameters.AddWithValue("FilePath", filePath);
                    cmd.Parameters.AddWithValue("FileSize", fileSize);
                    cmd.Parameters.AddWithValue("ContentType", contentType);
                    documentId = (int)await cmd.ExecuteScalarAsync();
                }

                // Сохранение программы
                var programQuery = "INSERT INTO Programs (Degree, Name, NameCode, Purpose, Years, Credits, LinkFaculty, ProgramDocumentId, Accredited) VALUES (@Degree, @Name, @NameCode, @Purpose, @Years, @Credits, @LinkFaculty, @ProgramDocumentId, @Accredited) RETURNING Id";
                using (var cmd = new NpgsqlCommand(programQuery, connection))
                {
                    cmd.Parameters.AddWithValue("Degree", program.Degree ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("Name", program.Name);
                    cmd.Parameters.AddWithValue("NameCode", program.NameCode ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("Purpose", program.Purpose ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("Years", program.Years ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("Credits", program.Credits ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("LinkFaculty", program.LinkFaculty ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("ProgramDocumentId", documentId);
                    cmd.Parameters.AddWithValue("Accredited", program.Accredited);
                    program.Id = (int)await cmd.ExecuteScalarAsync();
                }
                program.ProgramDocumentId = documentId;
            }
        }

        public async Task UpdateProgram(ProgramModel program)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"
            UPDATE Program
            SET Degree = @Degree, 
                Name = @Name, 
                Name_Code = @NameCode, 
                Purpose = @Purpose, 
                Years = @Years, 
                Credits = @Credits, 
                Link_Faculty = @LinkFaculty, 
                Accredited = @Accredited,
                Form = @Form,
                Directions = @Directions,
                Results = @Results,
                Objects = @Objects,
                Descriptions = @Descriptions,
                Field_Of_Study = @FieldOfStudy,
                Speciality = @Speciality
            WHERE Id = @Id";
                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("Id", program.Id);
                    cmd.Parameters.AddWithValue("Degree", program.Degree ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("Name", program.Name ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("NameCode", program.NameCode ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("Descriptions", program.Descriptions ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("Purpose", program.Purpose ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("Years", program.Years ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("Credits", program.Credits ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("LinkFaculty", program.LinkFaculty ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("Accredited", program.Accredited);
                    cmd.Parameters.AddWithValue("Objects", program.Objects ?? (object)DBNull.Value);

                    // JSONB поля
                    var formJson = program.Form != null ? JsonSerializer.Serialize(program.Form) : null;
                    cmd.Parameters.Add(new NpgsqlParameter("Form", NpgsqlDbType.Jsonb) { Value = formJson ?? (object)DBNull.Value });

                    var directionsJson = program.Directions != null ? JsonSerializer.Serialize(program.Directions) : null;
                    cmd.Parameters.Add(new NpgsqlParameter("Directions", NpgsqlDbType.Jsonb) { Value = directionsJson ?? (object)DBNull.Value });

                    var resultsJson = program.Results != null ? JsonSerializer.Serialize(program.Results) : null;
                    cmd.Parameters.Add(new NpgsqlParameter("Results", NpgsqlDbType.Jsonb) { Value = resultsJson ?? (object)DBNull.Value });

                    var fieldOfStudyJson = program.FieldOfStudy != null ? JsonSerializer.Serialize(program.FieldOfStudy) : null;
                    cmd.Parameters.Add(new NpgsqlParameter("FieldOfStudy", NpgsqlDbType.Jsonb) { Value = fieldOfStudyJson ?? (object)DBNull.Value });

                    var specialityJson = program.Speciality != null ? JsonSerializer.Serialize(program.Speciality) : null;
                    cmd.Parameters.Add(new NpgsqlParameter("Speciality", NpgsqlDbType.Jsonb) { Value = specialityJson ?? (object)DBNull.Value });

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateProgramWithDocument(ProgramModel program, string filePath, string fileName, string contentType, long fileSize)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Удаление старого документа (если был)
                if (program.ProgramDocumentId != 0)
                {
                    var deleteQuery = "DELETE FROM ProgramDocuments WHERE Id = @Id";
                    using (var cmd = new NpgsqlCommand(deleteQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("Id", program.ProgramDocumentId);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                // Сохранение нового документа
                var documentQuery = "INSERT INTO ProgramDocuments (FileName, FilePath, FileSize, ContentType) " +
                    "VALUES (@FileName, @FilePath, @FileSize, @ContentType) RETURNING Id";
                int documentId;
                using (var cmd = new NpgsqlCommand(documentQuery, connection))
                {
                    cmd.Parameters.AddWithValue("FileName", fileName);
                    cmd.Parameters.AddWithValue("FilePath", filePath);
                    cmd.Parameters.AddWithValue("FileSize", fileSize);
                    cmd.Parameters.AddWithValue("ContentType", contentType);
                    

                    documentId = (int)await cmd.ExecuteScalarAsync();
                    Console.WriteLine(documentId);
                }

                // Обновление программы
                var query = @"
            UPDATE Program
            SET Degree = @Degree, 
                Name = @Name, 
                Name_Code = @NameCode, 
                Purpose = @Purpose, 
                Years = @Years, 
                Credits = @Credits, 
                Link_Faculty = @LinkFaculty, 
                Accredited = @Accredited,
                Form = @Form,
                Directions = @Directions,
                Results = @Results,
                Objects = @Objects,
                Descriptions = @Descriptions,
                Field_Of_Study = @FieldOfStudy,
                ProgramDocumentId = @ProgramDocumentId,
                Speciality = @Speciality
            WHERE Id = @Id";
                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("Id", program.Id);
                    cmd.Parameters.AddWithValue("Degree", program.Degree ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("Name", program.Name ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("NameCode", program.NameCode ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("Descriptions", program.Descriptions ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("Purpose", program.Purpose ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("Years", program.Years ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("Credits", program.Credits ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("LinkFaculty", program.LinkFaculty ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("Accredited", program.Accredited);
                    cmd.Parameters.AddWithValue("Objects", program.Objects ?? (object)DBNull.Value);

                    // JSONB поля
                    var formJson = program.Form != null ? JsonSerializer.Serialize(program.Form) : null;
                    cmd.Parameters.Add(new NpgsqlParameter("Form", NpgsqlDbType.Jsonb) { Value = formJson ?? (object)DBNull.Value });

                    var directionsJson = program.Directions != null ? JsonSerializer.Serialize(program.Directions) : null;
                    cmd.Parameters.Add(new NpgsqlParameter("Directions", NpgsqlDbType.Jsonb) { Value = directionsJson ?? (object)DBNull.Value });

                    var resultsJson = program.Results != null ? JsonSerializer.Serialize(program.Results) : null;
                    cmd.Parameters.Add(new NpgsqlParameter("Results", NpgsqlDbType.Jsonb) { Value = resultsJson ?? (object)DBNull.Value });

                    var fieldOfStudyJson = program.FieldOfStudy != null ? JsonSerializer.Serialize(program.FieldOfStudy) : null;
                    cmd.Parameters.Add(new NpgsqlParameter("FieldOfStudy", NpgsqlDbType.Jsonb) { Value = fieldOfStudyJson ?? (object)DBNull.Value });

                    var specialityJson = program.Speciality != null ? JsonSerializer.Serialize(program.Speciality) : null;
                    cmd.Parameters.Add(new NpgsqlParameter("Speciality", NpgsqlDbType.Jsonb) { Value = specialityJson ?? (object)DBNull.Value });

                    cmd.Parameters.AddWithValue("ProgramDocumentId", documentId);

                    await cmd.ExecuteNonQueryAsync();
                }

            }
        }



        public async Task DeleteProgram(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Получаем информацию о programdocument, чтобы удалить файл с диска
                string filePath = null;
                var selectDocQuery = "SELECT FilePath FROM programdocuments WHERE Id = (SELECT programdocumentid FROM Program WHERE Id = @Id)";
                using (var cmd = new NpgsqlCommand(selectDocQuery, connection))
                {
                    cmd.Parameters.AddWithValue("Id", id);
                    var result = await cmd.ExecuteScalarAsync();
                    filePath = result as string;
                }

                // Удаляем запись из таблицы programdocuments
                var deleteDocQuery = "DELETE FROM programdocuments WHERE Id = (SELECT programdocumentid FROM Program WHERE Id = @Id)";
                using (var cmd = new NpgsqlCommand(deleteDocQuery, connection))
                {
                    cmd.Parameters.AddWithValue("Id", id);
                    await cmd.ExecuteNonQueryAsync();
                }

                // Удаляем программу из таблицы Program
                var deleteProgramQuery = "DELETE FROM Program WHERE Id = @Id";
                using (var cmd = new NpgsqlCommand(deleteProgramQuery, connection))
                {
                    cmd.Parameters.AddWithValue("Id", id);
                    var rowsAffected = await cmd.ExecuteNonQueryAsync();

                    if (rowsAffected == 0)
                        throw new Exception("Program not found.");
                }

                // Удаляем файл с диска, если он существует
                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }


    }
}
