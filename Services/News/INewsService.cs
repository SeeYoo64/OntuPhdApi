using OntuPhdApi.Models.News;

namespace OntuPhdApi.Services.News
{
    public interface INewsService
    {
        List<NewsModel> GetNews();
        NewsView GetNewsById(int id);
        void UpdateNews(NewsModel news);
        List<NewsLatest> GetLatestNews(int count = 4);
        void AddNews(NewsModel news);
    }
}
