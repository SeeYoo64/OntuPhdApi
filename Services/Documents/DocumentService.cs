using Npgsql;
using OntuPhdApi.Models;

namespace OntuPhdApi.Services.Documents
{
    public class DocumentService : IDocumentService
    {
        private readonly string _connectionString;

        public DocumentService(IConfiguration configuration)
        {

            _connectionString = configuration.GetConnectionString("DefaultConnection");

        }
        public List<DocumentsModel> GetDocuments()
        {
            var documents = new List<DocumentsModel>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using var cmd = new NpgsqlCommand("SELECT Id, ProgramId, Name, Type, Link FROM Documents", connection);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    documents.Add(new DocumentsModel
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

        public DocumentsModel GetDocumentById(int id)
        {
            DocumentsModel document = null;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using var cmd = new NpgsqlCommand("SELECT Id, ProgramId, Name, Type, Link FROM Documents WHERE Id = @id", connection);
                cmd.Parameters.AddWithValue("id", id);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    document = new DocumentsModel
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

        public List<DocumentsModel> GetDocumentsByType(string type)
        {
            var documents = new List<DocumentsModel>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using var cmd = new NpgsqlCommand("SELECT Id, ProgramId, Name, Type, Link FROM Documents WHERE Type = @type", connection);
                cmd.Parameters.AddWithValue("type", type);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    documents.Add(new DocumentsModel
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

        public void AddDocument(DocumentsModel document)
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
    }
}
