using System.Text.Json;
using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;
using OntuPhdApi.Controllers;
using OntuPhdApi.Models.Programs;
using OntuPhdApi.Utilities;

namespace OntuPhdApi.Repositories
{
    public class ProgramRepository : IProgramRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<ProgramRepository> _logger;

        public ProgramRepository(IConfiguration configuration, ILogger<ProgramRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public async Task<List<ProgramModel>> GetAllProgramsAsync()
        {
            var programs = new List<ProgramModel>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    _logger.LogDebug("Database connection opened successfully.");

                    using (var cmd = new NpgsqlCommand(
                        "SELECT Id, Degree, Name, Name_Code, Field_Of_Study, Speciality, Form, Purpose, Years, Credits, " +
                        "Program_Characteristics, Program_Competence, Results, Link_Faculty, programdocumentid, Accredited, Directions, Objects " +
                        "FROM Program", connection))
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            try
                            {
                                var program = new ProgramModel
                                {
                                    Id = reader.GetInt32(0),
                                    Degree = reader.GetString(1),
                                    Name = reader.GetString(2),
                                    NameCode = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    FieldOfStudy = reader.IsDBNull(4) ? null : JsonSerializerHelper.Deserialize<FieldOfStudy>(reader.GetString(4)),
                                    Speciality = reader.IsDBNull(5) ? null : JsonSerializerHelper.Deserialize<Speciality>(reader.GetString(5)),
                                    Form = reader.IsDBNull(6) ? null : JsonSerializerHelper.Deserialize<List<string>>(reader.GetString(6)),
                                    Purpose = reader.IsDBNull(7) ? null : reader.GetString(7),
                                    Years = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8),
                                    Credits = reader.IsDBNull(9) ? (int?)null : reader.GetInt32(9),
                                    ProgramCharacteristics = reader.IsDBNull(10) ? null : JsonSerializerHelper.Deserialize<ProgramCharacteristics>(reader.GetString(10)),
                                    ProgramCompetence = reader.IsDBNull(11) ? null : JsonSerializerHelper.Deserialize<ProgramCompetence>(reader.GetString(11)),
                                    Results = reader.IsDBNull(12) ? null : JsonSerializerHelper.Deserialize<List<string>>(reader.GetString(12)),
                                    LinkFaculty = reader.IsDBNull(13) ? null : reader.GetString(13),
                                    ProgramDocumentId = reader.IsDBNull(14) ? 0 : reader.GetInt32(14),
                                    Accredited = reader.GetBoolean(15),
                                    Directions = reader.IsDBNull(16) ? null : JsonSerializerHelper.Deserialize<List<string>>(reader.GetString(16)),
                                    Objects = reader.IsDBNull(17) ? null : reader.GetString(17)
                                };
                                programs.Add(program);
                            }
                            catch (JsonException ex)
                            {
                                _logger.LogError(ex, "Failed to deserialize program with ID {ProgramId}.", reader.GetInt32(0));
                            }
                        }
                    }
                }
                catch (NpgsqlException ex)
                {
                    _logger.LogError(ex, "Database error occurred while fetching programs.");
                    throw; // Повторно выбрасываем исключение, чтобы клиент мог обработать ошибку
                }
            }

            return programs;
        }

        public async Task<List<ProgramComponent>> GetProgramComponentsAsync(int programId)
        {
            var components = new List<ProgramComponent>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    using (var cmd = new NpgsqlCommand(
                        "SELECT Id, ComponentType, ComponentName, ComponentCredits, ComponentHours, ControlForm " +
                        "FROM programcomponents WHERE program_id = @programId", connection))
                    {
                        cmd.Parameters.AddWithValue("programId", programId);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                components.Add(new ProgramComponent
                                {
                                    Id = reader.GetInt32(0),
                                    ProgramId = programId,
                                    ComponentType = reader.GetString(1),
                                    ComponentName = reader.GetString(2),
                                    ComponentCredits = reader.GetInt32(3),
                                    ComponentHours = reader.GetInt32(4),
                                    ControlForm = reader.IsDBNull(5) ? null : JsonSerializerHelper.Deserialize<List<string>>(reader.GetString(5))
                                });
                            }
                        }
                    }
                }
                catch (NpgsqlException ex)
                {
                    _logger.LogError(ex, "Database error occurred while fetching components for program ID {ProgramId}.", programId);
                    throw;
                }
            }
            return components;
        }

        public async Task<List<Job>> GetProgramJobsAsync(int programId)
        {
            var jobs = new List<Job>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    using (var cmd = new NpgsqlCommand(
                        "SELECT Id, Code, Title FROM Job WHERE program_id = @programId", connection))
                    {
                        cmd.Parameters.AddWithValue("programId", programId);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                jobs.Add(new Job
                                {
                                    Id = reader.GetInt32(0),
                                    Code = reader.GetString(1),
                                    Title = reader.GetString(2)
                                });
                            }
                        }
                    }
                }
                catch (NpgsqlException ex)
                {
                    _logger.LogError(ex, "Database error occurred while fetching jobs for program ID {ProgramId}.", programId);
                    throw;
                }
            }
            return jobs;
        }

        public async Task<ProgramModel> GetProgramByIdAsync(int id)
        {
            ProgramModel program = null;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    _logger.LogDebug("Database connection opened successfully for program ID {ProgramId}.", id);

                    using (var cmd = new NpgsqlCommand(
                        "SELECT p.Id, p.Degree, p.Name, p.Name_Code, p.Field_Of_Study, p.Speciality, p.Form, p.Purpose, p.Years, p.Credits, " +
                        "p.Program_Characteristics, p.Program_Competence, p.Results, p.Link_Faculty, p.programdocumentid, p.Accredited, p.Objects, p.Directions, " +
                        "p.Descriptions, d.FileName, d.FilePath " +
                        "FROM Program p " +
                        "LEFT JOIN Programdocuments d ON p.programdocumentid = d.Id " +
                        "WHERE p.Id = @id", connection))
                    {
                        cmd.Parameters.AddWithValue("id", id);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
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
                                        FieldOfStudy = reader.IsDBNull(4) ? null : JsonSerializerHelper.Deserialize<FieldOfStudy>(reader.GetString(4)),
                                        Speciality = reader.IsDBNull(5) ? null : JsonSerializerHelper.Deserialize<Speciality>(reader.GetString(5)),
                                        Form = reader.IsDBNull(6) ? null : JsonSerializerHelper.Deserialize<List<string>>(reader.GetString(6)),
                                        Purpose = reader.IsDBNull(7) ? null : reader.GetString(7),
                                        Years = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8),
                                        Credits = reader.IsDBNull(9) ? (int?)null : reader.GetInt32(9),
                                        ProgramCharacteristics = reader.IsDBNull(10) ? null : JsonSerializerHelper.Deserialize<ProgramCharacteristics>(reader.GetString(10)),
                                        ProgramCompetence = reader.IsDBNull(11) ? null : JsonSerializerHelper.Deserialize<ProgramCompetence>(reader.GetString(11)),
                                        Results = reader.IsDBNull(12) ? null : JsonSerializerHelper.Deserialize<List<string>>(reader.GetString(12)),
                                        LinkFaculty = reader.IsDBNull(13) ? null : reader.GetString(13),
                                        ProgramDocumentId = reader.IsDBNull(14) ? 0 : reader.GetInt32(14),
                                        Accredited = reader.GetBoolean(15),
                                        Objects = reader.IsDBNull(16) ? null : reader.GetString(16),
                                        Directions = reader.IsDBNull(17) ? null : JsonSerializerHelper.Deserialize<List<string>>(reader.GetString(17)),
                                        Descriptions = reader.IsDBNull(18) ? null : reader.GetString(18),
                                        // Дополнительные поля из Programdocuments
                                        ProgramDocument = reader.IsDBNull(19) ? null : new ProgramDocuments
                                        {
                                            FileName = reader.GetString(19),
                                            FilePath = reader.GetString(20)
                                        }
                                    };
                                }
                                catch (JsonException ex)
                                {
                                    _logger.LogError(ex, "Failed to deserialize program with ID {ProgramId}.", id);
                                    throw;
                                }
                            }
                        }
                    }

                    if (program == null)
                    {
                        _logger.LogWarning("Program with ID {ProgramId} not found.", id);
                    }

                    return program;
                }
                catch (NpgsqlException ex)
                {
                    _logger.LogError(ex, "Database error occurred while fetching program with ID {ProgramId}.", id);
                    throw;
                }
            }
        }

        public async Task<List<ProgramsDegreeDto>> GetProgramsByDegreeAsync(DegreeType? degree)
        {
            var programs = new List<ProgramsDegreeDto>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT Id, Degree, Name, Field_Of_Study, Speciality FROM Program";
                if (degree.HasValue)
                {
                    query += " WHERE Degree = @degree";
                }

                try
                {
                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        if (degree.HasValue)
                        {
                            cmd.Parameters.AddWithValue("degree", degree.Value.ToString());
                        }

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                try
                                {
                                    programs.Add(new ProgramsDegreeDto
                                    {
                                        Id = reader.GetInt32(0),
                                        Degree = reader.GetString(1),
                                        Name = reader.GetString(2),
                                        FieldOfStudy = reader.IsDBNull(3) ? null : JsonSerializerHelper.Deserialize<FieldOfStudy>(reader.GetString(3)),
                                        Speciality = reader.IsDBNull(4) ? null : JsonSerializerHelper.Deserialize<ShortSpeciality>(reader.GetString(4))
                                    });
                                }
                                catch (JsonException ex)
                                {
                                    _logger.LogError(ex, "Failed to deserialize program with ID {ProgramId} for degree {Degree}.", reader.GetInt32(0), degree);
                                }
                            }
                        }
                    }

                    _logger.LogInformation("Retrieved {ProgramCount} programs for degree {Degree}.", programs.Count, degree?.ToString() ?? "all");
                    return programs;
                }
                catch (NpgsqlException ex)
                {
                    _logger.LogError(ex, "Database error occurred while fetching programs for degree {Degree}.", degree);
                    throw;
                }
            }
        }



        public async Task<int> InsertProgramAsync(ProgramModel program)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                return await InsertProgramAsync(program, connection, null);
            }
        }

        public async Task<int> InsertProgramAsync(ProgramModel program, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            var query = @"
                INSERT INTO Program (
                    Degree, Name, Name_Code, Field_Of_Study, Speciality, Form, Objects, Directions, 
                    Descriptions, Purpose, Years, Credits, Program_Characteristics, Program_Competence, 
                    Results, Link_Faculty, ProgramDocumentId, Accredited
                ) 
                VALUES (
                    @Degree, @Name, @NameCode, @FieldOfStudy, @Speciality, @Form, @Objects, @Directions, 
                    @Descriptions, @Purpose, @Years, @Credits, @ProgramCharacteristics, @ProgramCompetence, 
                    @Results, @LinkFaculty, @ProgramDocumentId, @Accredited
                ) 
                RETURNING Id";

            try
            {
                using (var cmd = new NpgsqlCommand(query, connection) { Transaction = transaction })
                {
                    cmd.Parameters.AddWithValue("Degree", program.Degree ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("Name", program.Name ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("NameCode", program.NameCode ?? (object)DBNull.Value);
                    cmd.Parameters.Add(new NpgsqlParameter("FieldOfStudy", NpgsqlDbType.Jsonb)
                    {
                        Value = JsonSerializerHelper.Serialize(program.FieldOfStudy) ?? (object)DBNull.Value
                    });
                    cmd.Parameters.Add(new NpgsqlParameter("Speciality", NpgsqlDbType.Jsonb)
                    {
                        Value = JsonSerializerHelper.Serialize(program.Speciality) ?? (object)DBNull.Value
                    });
                    cmd.Parameters.Add(new NpgsqlParameter("Form", NpgsqlDbType.Jsonb)
                    {
                        Value = JsonSerializerHelper.Serialize(program.Form) ?? (object)DBNull.Value
                    });
                    cmd.Parameters.AddWithValue("Objects", program.Objects ?? (object)DBNull.Value);
                    cmd.Parameters.Add(new NpgsqlParameter("Directions", NpgsqlDbType.Jsonb)
                    {
                        Value = JsonSerializerHelper.Serialize(program.Directions) ?? (object)DBNull.Value
                    });
                    cmd.Parameters.AddWithValue("Descriptions", program.Descriptions ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("Purpose", program.Purpose ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("Years", program.Years.HasValue ? program.Years.Value : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("Credits", program.Credits.HasValue ? program.Credits.Value : (object)DBNull.Value);
                    cmd.Parameters.Add(new NpgsqlParameter("ProgramCharacteristics", NpgsqlDbType.Jsonb)
                    {
                        Value = JsonSerializerHelper.Serialize(program.ProgramCharacteristics) ?? (object)DBNull.Value
                    });
                    cmd.Parameters.Add(new NpgsqlParameter("ProgramCompetence", NpgsqlDbType.Jsonb)
                    {
                        Value = JsonSerializerHelper.Serialize(program.ProgramCompetence) ?? (object)DBNull.Value
                    });
                    cmd.Parameters.Add(new NpgsqlParameter("Results", NpgsqlDbType.Jsonb)
                    {
                        Value = JsonSerializerHelper.Serialize(program.Results) ?? (object)DBNull.Value
                    });
                    cmd.Parameters.AddWithValue("LinkFaculty", program.LinkFaculty ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("ProgramDocumentId", program.ProgramDocumentId.HasValue ? program.ProgramDocumentId.Value : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("Accredited", program.Accredited);

                    var programId = (int)await cmd.ExecuteScalarAsync();
                    _logger.LogInformation("Program {ProgramName} inserted with ID {ProgramId}.", program.Name, programId);
                    return programId;
                }
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(ex, "Failed to insert program {ProgramName}.", program.Name);
                throw;
            }
        }



        public async Task UpdateProgramAsync(ProgramModel program)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                await UpdateProgramAsync(program, connection, null);
            }
        }

        public async Task UpdateProgramAsync(ProgramModel program, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
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
                    Speciality = @Speciality,
                    Program_Characteristics = @ProgramCharacteristics,
                    ProgramDocumentId = @ProgramDocumentId
                WHERE Id = @Id";

            try
            {
                using (var cmd = new NpgsqlCommand(query, connection) { Transaction = transaction })
                {
                    cmd.Parameters.AddWithValue("Id", program.Id);
                    cmd.Parameters.AddWithValue("Degree", program.Degree ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("Name", program.Name ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("NameCode", program.NameCode ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("Purpose", program.Purpose ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("Years", program.Years.HasValue ? program.Years.Value : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("Credits", program.Credits.HasValue ? program.Credits.Value : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("LinkFaculty", program.LinkFaculty ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("Accredited", program.Accredited);
                    cmd.Parameters.AddWithValue("Objects", program.Objects ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("Descriptions", program.Descriptions ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("ProgramDocumentId", program.ProgramDocumentId.HasValue ? program.ProgramDocumentId.Value : (object)DBNull.Value);

                    cmd.Parameters.Add(new NpgsqlParameter("Form", NpgsqlDbType.Jsonb)
                    {
                        Value = JsonSerializerHelper.Serialize(program.Form) ?? (object)DBNull.Value
                    });
                    cmd.Parameters.Add(new NpgsqlParameter("Directions", NpgsqlDbType.Jsonb)
                    {
                        Value = JsonSerializerHelper.Serialize(program.Directions) ?? (object)DBNull.Value
                    });
                    cmd.Parameters.Add(new NpgsqlParameter("Results", NpgsqlDbType.Jsonb)
                    {
                        Value = JsonSerializerHelper.Serialize(program.Results) ?? (object)DBNull.Value
                    });
                    cmd.Parameters.Add(new NpgsqlParameter("FieldOfStudy", NpgsqlDbType.Jsonb)
                    {
                        Value = JsonSerializerHelper.Serialize(program.FieldOfStudy) ?? (object)DBNull.Value
                    });
                    cmd.Parameters.Add(new NpgsqlParameter("Speciality", NpgsqlDbType.Jsonb)
                    {
                        Value = JsonSerializerHelper.Serialize(program.Speciality) ?? (object)DBNull.Value
                    });
                    cmd.Parameters.Add(new NpgsqlParameter("ProgramCharacteristics", NpgsqlDbType.Jsonb)
                    {
                        Value = JsonSerializerHelper.Serialize(program.ProgramCharacteristics) ?? (object)DBNull.Value
                    });

                    var rowsAffected = await cmd.ExecuteNonQueryAsync();
                    if (rowsAffected == 0)
                    {
                        _logger.LogWarning("Program with ID {ProgramId} not found for update.", program.Id);
                        throw new Exception($"Program with ID {program.Id} not found.");
                    }

                    _logger.LogInformation("Program {ProgramName} with ID {ProgramId} updated.", program.Name, program.Id);
                }
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(ex, "Failed to update program {ProgramName} with ID {ProgramId}.", program.Name, program.Id);
                throw;
            }
        }




        public async Task DeleteProgramAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                await DeleteProgramAsync(id, connection, null);
            }
        }

        public async Task DeleteProgramAsync(int id, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            var query = "DELETE FROM Program WHERE Id = @Id";

            try
            {
                using (var cmd = new NpgsqlCommand(query, connection) { Transaction = transaction })
                {
                    cmd.Parameters.AddWithValue("Id", id);
                    var rowsAffected = await cmd.ExecuteNonQueryAsync();

                    if (rowsAffected == 0)
                    {
                        _logger.LogWarning("Program with ID {ProgramId} not found for deletion.", id);
                        throw new Exception($"Program with ID {id} not found.");
                    }

                    _logger.LogInformation("Program with ID {ProgramId} deleted from database.", id);
                }
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(ex, "Failed to delete program with ID {ProgramId} from database.", id);
                throw;
            }
        }









    }
}