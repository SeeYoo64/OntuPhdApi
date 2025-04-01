using Npgsql;
using Npgsql.Internal.Postgres;
using OntuPhdApi.Models;
using OntuPhdApi.Models.News;
using System.Reflection.Metadata;
using System.Text.Json;

namespace OntuPhdApi.Services
{
    public class DatabaseService(IConfiguration configuration)
    {
        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection");


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


    }
}