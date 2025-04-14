using OntuPhdApi.Models.News;

namespace OntuPhdApi.Repositories.News
{
    public interface INewsRepository
    {
        Task<List<NewsModel>> GetAllNewsAsync();
        Task<NewsModel> GetNewsByIdAsync(int id);
        Task<List<NewsModel>> GetLatestNewsAsync(int count);
        Task AddNewsAsync(NewsModel news);
        Task UpdateNewsAsync(NewsModel news);
    }
}
