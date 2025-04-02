using Npgsql;
using OntuPhdApi.Models;
using System.Text.Json;

namespace OntuPhdApi.Services.ApplyDocuments
{
    public class ApplyDocumentsService : IApplyDocumentsService
    {
        private readonly string _connectionString;

        public ApplyDocumentsService(IConfiguration configuration)
        {

            _connectionString = configuration.GetConnectionString("DefaultConnection");

        }
        public List<ApplyDocumentsModel> GetApplyDocuments()
        {
            var applyDocuments = new List<ApplyDocumentsModel>();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using var cmd = new NpgsqlCommand("SELECT Id, Name, Description, " +
                    "Requirements, OriginalsRequired FROM ApplyDocuments", connection);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        applyDocuments.Add(new ApplyDocumentsModel
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


        public ApplyDocumentsModel GetApplyDocumentById(int id)
        {
            ApplyDocumentsModel applyDocument = null;
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
                        applyDocument = new ApplyDocumentsModel
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


        public List<ApplyDocumentsModel> GetApplyDocumentsByName(string name)
        {
            var applyDocuments = new List<ApplyDocumentsModel>();
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
                        applyDocuments.Add(new ApplyDocumentsModel
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


        public void AddApplyDocument(ApplyDocumentsModel applyDocument)
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
    }
}
