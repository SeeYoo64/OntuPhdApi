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


        public List<NewsView> GetNews()
        {
            var newsList = new List<NewsView>();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("SELECT Id, Title, MainTag, Date, Thumbnail FROM News", connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        try
                        {
                            newsList.Add(new NewsView
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                MainTag = reader.GetString(2),
                                Date = reader.GetDateTime(3),
                                Thumbnail = reader.GetString(4)
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

        public List<NewsLatest> GetLatestNews(int count = 4)
        {
            var newsList = new List<NewsLatest>();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand(
                    "SELECT Id, Title, Summary, MainTag, Date, Thumbnail " +
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
                                newsList.Add(new NewsLatest
                                {
                                    Id = reader.GetInt32(0),
                                    Title = reader.GetString(1),
                                    Summary = reader.GetString(2),
                                    MainTag = reader.GetString(3),
                                    Date = reader.GetDateTime(4),
                                    Thumbnail = reader.GetString(5),
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