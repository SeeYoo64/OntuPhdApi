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