namespace Paperless.SearchLibrary;

public interface ISearchIndex
{
    void AddDocumentAsync(Document doc);
    Task<IEnumerable<Document>> SearchDocumentAsync(string searchTerm, int? limit);
}


