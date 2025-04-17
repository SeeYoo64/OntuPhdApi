using OntuPhdApi.Models.ApplyDocuments;

namespace OntuPhdApi.Services.ApplyDocuments

{
    public interface IApplyDocumentsService
    {
        Task<List<ApplyDocumentDto>> GetApplyDocumentsAsync();
        Task<ApplyDocumentDto> GetApplyDocumentByIdAsync(int id);
        Task<ApplyDocumentDto> GetApplyDocumentByNameAsync(string name);
        Task<int> AddApplyDocumentAsync(ApplyDocumentCreateUpdateDto applyDocumentDto);
        Task UpdateApplyDocumentAsync(int id, ApplyDocumentCreateUpdateDto applyDocumentDto); 
        Task DeleteApplyDocumentAsync(int id);
    }
}
