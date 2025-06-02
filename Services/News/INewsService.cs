using OntuPhdApi.Models.News;

namespace OntuPhdApi.Services.News
{
    public interface INewsService
    {
        Task<List<NewsDto>> GetNewsAsync();
        Task<NewsViewDto> GetNewsByIdAsync(int id);
        Task<NewsDto> GetFullNewsByIdAsync(int id);
        Task<List<NewsLatestDto>> GetLatestNewsAsync(int count);
        Task AddNewsAsync(NewsCreateUpdateDto newsDto, List<IFormFile> photoFiles);
        Task UpdateNewsAsync(int id, NewsCreateUpdateDto newsDto, List<IFormFile> photoFiles);
        Task DeleteNewsAsync(int id);
    }
}
