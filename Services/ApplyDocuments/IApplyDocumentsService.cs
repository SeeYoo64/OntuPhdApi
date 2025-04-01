using OntuPhdApi.Models;

namespace OntuPhdApi.Services.ApplyDocuments

{
    public interface IApplyDocumentsService
    {
        List<ApplyDocumentsModel> GetApplyDocuments();
        ApplyDocumentsModel GetApplyDocumentById(int id);
        List<ApplyDocumentsModel> GetApplyDocumentsByName(string name);
        void AddApplyDocument(ApplyDocumentsModel applyDocument);
    }
}
