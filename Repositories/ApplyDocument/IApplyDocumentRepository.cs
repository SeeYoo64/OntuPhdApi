using OntuPhdApi.Models.ApplyDocuments;

namespace OntuPhdApi.Repositories.ApplyDocument
{
    public interface IApplyDocumentRepository
    {
        Task<List<ApplyDocumentModel>> GetAllApplyDocumentsAsync();
        Task<ApplyDocumentModel> GetApplyDocumentByIdAsync(int id);
        Task<ApplyDocumentModel> GetApplyDocumentByNameAsync(string name);
        Task AddApplyDocumentAsync(ApplyDocumentModel applyDocument);
        Task UpdateApplyDocumentAsync(ApplyDocumentModel applyDocument); 
        Task DeleteApplyDocumentAsync(int id);
    }
}
