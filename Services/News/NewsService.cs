using Microsoft.AspNetCore.Http.HttpResults;
using Npgsql;
using OntuPhdApi.Models.News;
using OntuPhdApi.Repositories.News;
using OntuPhdApi.Services.Files;
using OntuPhdApi.Utilities.Mappers;
using System.Text.Json;

namespace OntuPhdApi.Services.News
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;
        private readonly IProgramFileService _fileService; 
        private readonly ILogger<NewsService> _logger;
        private readonly string _newsUploadsPath = Path.Combine("wwwroot", "Files", "Uploads", "News");

        public NewsService(
            INewsRepository newsRepository,
            IProgramFileService fileService,
            ILogger<NewsService> logger)
        {
            _newsRepository = newsRepository;
            _fileService = fileService;
            _logger = logger;

            // Создаем директорию для загрузок, если она не существует
            if (!Directory.Exists(_newsUploadsPath))
            {
                Directory.CreateDirectory(_newsUploadsPath);
            }
        }

        public async Task<List<NewsDto>> GetNewsAsync()
        {
            _logger.LogInformation("Fetching all news.");
            try
            {
                var news = await _newsRepository.GetAllNewsAsync();
                return NewsMapper.ToDtoList(news);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch news.");
                throw;
            }
        }

        public async Task<NewsViewDto> GetNewsByIdAsync(int id)
        {
            _logger.LogInformation("Fetching news with ID {NewsId}.", id);
            try
            {
                var news = await _newsRepository.GetNewsByIdAsync(id);
                if (news == null)
                {
                    _logger.LogWarning("News with ID {NewsId} not found.", id);
                    return null;
                }
                return NewsMapper.ToViewDto(news);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch news with ID {NewsId}.", id);
                throw;
            }
        }

        public async Task<List<NewsLatestDto>> GetLatestNewsAsync(int count)
        {
            _logger.LogInformation("Fetching {Count} latest news.", count);
            try
            {
                if (count <= 0)
                {
                    _logger.LogWarning("Invalid count parameter: {Count}.", count);
                    throw new ArgumentException("Count must be greater than zero.");
                }

                var news = await _newsRepository.GetLatestNewsAsync(count);
                return NewsMapper.ToLatestDtoList(news);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch {Count} latest news.", count);
                throw;
            }
        }

        public async Task AddNewsAsync(NewsCreateUpdateDto newsDto)
        {
            if (newsDto == null || string.IsNullOrEmpty(newsDto.Title) || string.IsNullOrEmpty(newsDto.Summary) || string.IsNullOrEmpty(newsDto.MainTag))
            {
                _logger.LogWarning("Invalid news data provided for creation.");
                throw new ArgumentException("Title, Summary, and MainTag are required.");
            }

            _logger.LogInformation("Adding new news with title {NewsTitle}.", newsDto.Title);
            try
            {
                var news = NewsMapper.ToEntity(newsDto);

                // Добавляем новость, чтобы получить ID
                await _newsRepository.AddNewsAsync(news);

                // Создаем директорию для файлов новости
                var newsDir = Path.Combine(_newsUploadsPath, news.Id.ToString());
                if (!Directory.Exists(newsDir))
                {
                    Directory.CreateDirectory(newsDir);
                }

                // Сохраняем миниатюру
                if (newsDto.Thumbnail != null && newsDto.Thumbnail.Length > 0)
                {
                    var thumbnailFileName = $"thumb{news.Id}{Path.GetExtension(newsDto.Thumbnail.FileName)}";
                    var thumbnailPath = Path.Combine(newsDir, thumbnailFileName);
                    using (var stream = new FileStream(thumbnailPath, FileMode.Create))
                    {
                        await newsDto.Thumbnail.CopyToAsync(stream);
                    }
                    news.ThumbnailPath = $"/Files/Uploads/News/{news.Id}/{thumbnailFileName}";
                }

                // Сохраняем фотографии
                if (newsDto.Photos != null && newsDto.Photos.Any())
                {
                    news.PhotoPaths = new List<string>();
                    foreach (var photo in newsDto.Photos)
                    {
                        if (photo.Length > 0)
                        {
                            var uniqueId = Guid.NewGuid().ToString();
                            var photoFileName = $"photo-{uniqueId}{Path.GetExtension(photo.FileName)}";
                            var photoPath = Path.Combine(newsDir, photoFileName);
                            using (var stream = new FileStream(photoPath, FileMode.Create))
                            {
                                await photo.CopyToAsync(stream);
                            }
                            news.PhotoPaths.Add($"/Files/Uploads/News/{news.Id}/{photoFileName}");
                        }
                    }
                }

                // Обновляем новость с путями к файлам
                await _newsRepository.UpdateNewsAsync(news);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add news with title {NewsTitle}.", newsDto.Title);
                throw;
            }
        }

        public async Task UpdateNewsAsync(int id, NewsCreateUpdateDto newsDto)
        {
            if (newsDto == null || string.IsNullOrEmpty(newsDto.Title) || string.IsNullOrEmpty(newsDto.Summary) || string.IsNullOrEmpty(newsDto.MainTag))
            {
                _logger.LogWarning("Invalid news data provided for update.");
                throw new ArgumentException("Title, Summary, and MainTag are required.");
            }

            _logger.LogInformation("Updating news with ID {NewsId}.", id);
            try
            {
                var news = await _newsRepository.GetNewsByIdAsync(id);
                if (news == null)
                {
                    _logger.LogWarning("News with ID {NewsId} not found.", id);
                    throw new KeyNotFoundException("News not found.");
                }

                NewsMapper.UpdateEntity(news, newsDto);

                // Создаем директорию для файлов новости, если её нет
                var newsDir = Path.Combine(_newsUploadsPath, news.Id.ToString());
                if (!Directory.Exists(newsDir))
                {
                    Directory.CreateDirectory(newsDir);
                }

                // Обновляем миниатюру, если она загружена
                if (newsDto.Thumbnail != null && newsDto.Thumbnail.Length > 0)
                {
                    // Удаляем старую миниатюру, если она есть
                    if (!string.IsNullOrEmpty(news.ThumbnailPath))
                    {
                        var oldThumbnailPath = Path.Combine("wwwroot", news.ThumbnailPath.TrimStart('/'));
                        if (File.Exists(oldThumbnailPath))
                        {
                            File.Delete(oldThumbnailPath);
                        }
                    }

                    var thumbnailFileName = $"thumb{news.Id}{Path.GetExtension(newsDto.Thumbnail.FileName)}";
                    var thumbnailPath = Path.Combine(newsDir, thumbnailFileName);
                    using (var stream = new FileStream(thumbnailPath, FileMode.Create))
                    {
                        await newsDto.Thumbnail.CopyToAsync(stream);
                    }
                    news.ThumbnailPath = $"/Files/Uploads/News/{news.Id}/{thumbnailFileName}";
                }

                // Обновляем фотографии, если они загружены
                if (newsDto.Photos != null && newsDto.Photos.Any())
                {
                    // Удаляем старые фотографии, если они есть
                    if (news.PhotoPaths != null && news.PhotoPaths.Any())
                    {
                        foreach (var oldPhotoPath in news.PhotoPaths)
                        {
                            var fullPath = Path.Combine("wwwroot", oldPhotoPath.TrimStart('/'));
                            if (File.Exists(fullPath))
                            {
                                File.Delete(fullPath);
                            }
                        }
                    }

                    // Сохраняем новые фотографии
                    news.PhotoPaths = new List<string>();
                    foreach (var photo in newsDto.Photos)
                    {
                        if (photo.Length > 0)
                        {
                            var uniqueId = Guid.NewGuid().ToString();
                            var photoFileName = $"photo-{uniqueId}{Path.GetExtension(photo.FileName)}";
                            var photoPath = Path.Combine(newsDir, photoFileName);
                            using (var stream = new FileStream(photoPath, FileMode.Create))
                            {
                                await photo.CopyToAsync(stream);
                            }
                            news.PhotoPaths.Add($"/Files/Uploads/News/{news.Id}/{photoFileName}");
                        }
                    }
                }

                await _newsRepository.UpdateNewsAsync(news);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update news with ID {NewsId}.", id);
                throw;
            }
        }


    }
}
