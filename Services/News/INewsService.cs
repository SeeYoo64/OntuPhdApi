using OntuPhdApi.Models.News;

namespace OntuPhdApi.Services.News
{
    public interface INewsService
    {
        Task<List<NewsDto>> GetNewsAsync();
        Task<NewsViewDto> GetNewsByIdAsync(int id);
        Task<List<NewsLatestDto>> GetLatestNewsAsync(int count);
        Task AddNewsAsync(NewsCreateUpdateDto newsDto);
        Task UpdateNewsAsync(int id, NewsCreateUpdateDto newsDto);
        Task DeleteNewsAsync(int id);
    }
}
