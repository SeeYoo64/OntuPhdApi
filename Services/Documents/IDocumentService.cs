using OntuPhdApi.Models.Documents;

namespace OntuPhdApi.Services.Documents
{
    public interface IDocumentService
    {
        Task<List<DocumentDto>> GetDocumentsAsync();
        Task<DocumentDto> GetDocumentByIdAsync(int id);
        Task<List<DocumentDto>> GetDocumentsByTypeAsync(string type);
        Task<int> AddDocumentAsync(DocumentCreateUpdateDto documentDto);
        Task UpdateDocumentAsync(int id, DocumentCreateUpdateDto documentDto); 
        Task DeleteDocumentAsync(int id); 
    }
}
