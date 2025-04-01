using OntuPhdApi.Models.News;

namespace OntuPhdApi.Services.News
{
    public interface INewsService
    {
        List<NewsView> GetNews();
        NewsModel GetNewsById(int id);
        void UpdateNews(NewsModel news);
        List<NewsLatest> GetLatestNews(int count = 4);
        void AddNews(NewsModel news);
    }
}
