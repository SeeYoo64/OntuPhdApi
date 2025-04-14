using Microsoft.EntityFrameworkCore;
using OntuPhdApi.Data;
using OntuPhdApi.Models.News;

namespace OntuPhdApi.Repositories.News
{
    public class NewsRepository : INewsRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<NewsRepository> _logger;

        public NewsRepository(AppDbContext context, ILogger<NewsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<NewsModel>> GetAllNewsAsync()
        {
            _logger.LogInformation("Fetching all news from database.");
            try
            {
                return await _context.News
                    .OrderByDescending(n => n.PublicationDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all news.");
                throw;
            }
        }

        public async Task<NewsModel> GetNewsByIdAsync(int id)
        {
            _logger.LogInformation("Fetching news with ID {NewsId} from database.", id);
            try
            {
                return await _context.News
                    .FirstOrDefaultAsync(n => n.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching news with ID {NewsId}.", id);
                throw;
            }
        }


        public async Task<List<NewsModel>> GetLatestNewsAsync(int count)
        {
            _logger.LogInformation("Fetching {Count} latest news from database.", count);
            try
            {
                return await _context.News
                    .OrderByDescending(n => n.PublicationDate)
                    .Take(count)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching {Count} latest news.", count);
                throw;
            }
        }

        public async Task AddNewsAsync(NewsModel news)
        {
            _logger.LogInformation("Adding new news with title {NewsTitle}.", news.Title);
            try
            {
                await _context.News.AddAsync(news);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding news with title {NewsTitle}.", news.Title);
                throw;
            }
        }

        public async Task UpdateNewsAsync(NewsModel news)
        {
            _logger.LogInformation("Updating news with ID {NewsId}.", news.Id);
            try
            {
                _context.News.Update(news);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating news with ID {NewsId}.", news.Id);
                throw;
            }
        }


        public async Task DeleteNewsAsync(int id)
        {
            _logger.LogInformation("Deleting news with ID {NewsId} from database.", id);
            try
            {
                var news = await _context.News.FirstOrDefaultAsync(n => n.Id == id);
                if (news == null)
                {
                    _logger.LogWarning("News with ID {NewsId} not found for deletion.", id);
                    throw new KeyNotFoundException("News not found.");
                }

                _context.News.Remove(news);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting news with ID {NewsId}.", id);
                throw;
            }
        }

    }
}
