using Npgsql;
using OntuPhdApi.Models.News;
using System.Text.Json;

namespace OntuPhdApi.Services.News
{
    public class NewsService : INewsService
    {

        private readonly string _connectionString;

        public NewsService(IConfiguration configuration)
        {

            _connectionString = configuration.GetConnectionString("DefaultConnection");

        }

        public List<NewsModel> GetNews()
        {
            var newsList = new List<NewsModel>();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("SELECT Id, Title, Summary, MainTag, " +
                    "Othertags, Date, Thumbnail, Photos, Body FROM News", connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        try
                        {
                            newsList.Add(new NewsModel
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Summary = reader.GetString(2),
                                MainTag = reader.GetString(3),
                                OtherTags = JsonSerializer.Deserialize<List<string>>(reader.GetString(4), jsonOptions),
                                Date = reader.GetDateTime(5),
                                Thumbnail = reader.GetString(6),
                                Photos = JsonSerializer.Deserialize<List<string>>(reader.GetString(7), jsonOptions),
                                Body = reader.GetString(8)
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

        public NewsView GetNewsById(int id)
        {
            NewsView news = null;
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("SELECT Id, Title, " +
                    "MainTag, OtherTags, Date, Photos, Body FROM News WHERE Id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            try
                            {
                                news = new NewsView
                                {
                                    Id = reader.GetInt32(0),
                                    Title = reader.GetString(1),
                                    MainTag = reader.GetString(2),
                                    OtherTags = JsonSerializer.Deserialize<List<string>>(reader.GetString(3), jsonOptions),
                                    Date = reader.GetDateTime(4),
                                    Photos = JsonSerializer.Deserialize<List<string>>(reader.GetString(5), jsonOptions),
                                    Body = reader.GetString(6)
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

        public void UpdateNews(NewsModel news)
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

        public void AddNews(NewsModel news)
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
