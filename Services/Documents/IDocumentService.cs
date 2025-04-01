using OntuPhdApi.Models;

namespace OntuPhdApi.Services.Documents
{
    public interface IDocumentService
    {
        List<DocumentsModel> GetDocuments();
        DocumentsModel GetDocumentById(int id);
        List<DocumentsModel> GetDocumentsByType(string type);
        void AddDocument(DocumentsModel document);
    }
}
