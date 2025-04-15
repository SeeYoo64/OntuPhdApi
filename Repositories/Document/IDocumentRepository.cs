using OntuPhdApi.Models.Documents;

namespace OntuPhdApi.Repositories.Document
{
    public interface IDocumentRepository
    {
        Task<List<DocumentModel>> GetAllDocumentsAsync();
        Task<DocumentModel> GetDocumentByIdAsync(int id);
        Task<List<DocumentModel>> GetDocumentsByTypeAsync(string type);
        Task AddDocumentAsync(DocumentModel document);
        Task UpdateDocumentAsync(DocumentModel document); 
        Task DeleteDocumentAsync(int id); 
    }
}
