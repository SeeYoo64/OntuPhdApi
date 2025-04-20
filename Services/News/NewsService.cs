using Microsoft.AspNetCore.Http.HttpResults;
using Npgsql;
using OntuPhdApi.Models.News;
using OntuPhdApi.Repositories.News;
using OntuPhdApi.Utilities.Mappers;
using System.Text.Json;

namespace OntuPhdApi.Services.News
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;
        private readonly ILogger<NewsService> _logger;
        private readonly string _newsUploadsPath = Path.Combine("wwwroot", "Files", "Uploads", "News");

        public NewsService(
            INewsRepository newsRepository,
            ILogger<NewsService> logger)
        {
            _newsRepository = newsRepository;
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

                // Создаём директорию для файлов новости
                var newsDir = Path.Combine(_newsUploadsPath, news.Id.ToString());
                Directory.CreateDirectory(newsDir);

                // Сохраняем миниатюру, если загружена
                if (newsDto.Thumbnail != null && newsDto.Thumbnail.Length > 0)
                {
                    // Проверка размера
                    if (newsDto.Thumbnail.Length > 5 * 1024 * 1024)
                    {
                        _logger.LogWarning("Thumbnail too large for news ID: {NewsId}, Size: {Size}", news.Id, newsDto.Thumbnail.Length);
                        throw new ArgumentException("Thumbnail size exceeds 5 MB");
                    }

                    // Проверка типа
                    var allowedTypes = new[] { "image/png", "image/jpeg", "image/jpg" };
                    if (!allowedTypes.Contains(newsDto.Thumbnail.ContentType))
                    {
                        _logger.LogWarning("Invalid thumbnail type for news ID: {NewsId}, Type: {Type}", news.Id, newsDto.Thumbnail.ContentType);
                        throw new ArgumentException("Only PNG, JPG, JPEG files are allowed for thumbnail");
                    }

                    var extension = Path.GetExtension(newsDto.Thumbnail.FileName).ToLower();
                    var fileName = $"thumb_{DateTime.UtcNow:yyyyMMddHHmmss}{extension}";
                    var filePath = Path.Combine(newsDir, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await newsDto.Thumbnail.CopyToAsync(stream);
                    }
                    news.ThumbnailPath = fileName;
                    _logger.LogInformation("Uploaded thumbnail for news ID: {NewsId}, Path: {Path}", news.Id, news.ThumbnailPath);
                }
                else
                {
                    news.ThumbnailPath = null;
                    _logger.LogInformation("No thumbnail provided for news ID: {NewsId}", news.Id);
                }

                // Сохраняем фотографии, если загружены
                if (newsDto.Photos != null && newsDto.Photos.Any())
                {
                    news.PhotoPaths = new List<string>();
                    foreach (var photo in newsDto.Photos)
                    {
                        if (photo.Length == 0)
                            continue;

                        // Проверка размера
                        if (photo.Length > 5 * 1024 * 1024)
                        {
                            _logger.LogWarning("Photo too large for news ID: {NewsId}, Size: {Size}", news.Id, photo.Length);
                            throw new ArgumentException("Photo size exceeds 5 MB");
                        }

                        // Проверка типа
                        var allowedTypes = new[] { "image/png", "image/jpeg", "image/jpg" };
                        if (!allowedTypes.Contains(photo.ContentType))
                        {
                            _logger.LogWarning("Invalid photo type for news ID: {NewsId}, Type: {Type}", news.Id, photo.ContentType);
                            throw new ArgumentException("Only PNG, JPG, JPEG files are allowed for photos");
                        }

                        var extension = Path.GetExtension(photo.FileName).ToLower();
                        var fileName = $"photo_{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid().ToString("N")[..8]}{extension}";
                        var filePath = Path.Combine(newsDir, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await photo.CopyToAsync(stream);
                        }
                        news.PhotoPaths.Add(fileName);
                        _logger.LogInformation("Uploaded photo for news ID: {NewsId}, Path: {Path}", news.Id, filePath);
                    }
                }

                // Обновляем новость с путями
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

                // Создаём директорию, если нет
                var newsDir = Path.Combine(_newsUploadsPath, id.ToString());
                Directory.CreateDirectory(newsDir);

                // Обновляем миниатюру, если загружена
                if (newsDto.Thumbnail != null && newsDto.Thumbnail.Length > 0)
                {
                    // Проверка размера
                    if (newsDto.Thumbnail.Length > 5 * 1024 * 1024)
                    {
                        _logger.LogWarning("Thumbnail too large for news ID: {NewsId}, Size: {Size}", id, newsDto.Thumbnail.Length);
                        throw new ArgumentException("Thumbnail size exceeds 5 MB");
                    }

                    // Проверка типа
                    var allowedTypes = new[] { "image/png", "image/jpeg", "image/jpg" };
                    if (!allowedTypes.Contains(newsDto.Thumbnail.ContentType))
                    {
                        _logger.LogWarning("Invalid thumbnail type for news ID: {NewsId}, Type: {Type}", id, newsDto.Thumbnail.ContentType);
                        throw new ArgumentException("Only PNG, JPG, JPEG files are allowed for thumbnail");
                    }

                    // Удаляем старую миниатюру, если есть
                    if (!string.IsNullOrEmpty(news.ThumbnailPath))
                    {
                        var oldThumbnailPath = Path.Combine(_newsUploadsPath, id.ToString(), Path.GetFileName(news.ThumbnailPath));
                        if (System.IO.File.Exists(oldThumbnailPath))
                        {
                            System.IO.File.Delete(oldThumbnailPath);
                            _logger.LogInformation("Deleted old thumbnail for news ID: {NewsId}, Path: {Path}", id, oldThumbnailPath);
                        }
                    }

                    var extension = Path.GetExtension(newsDto.Thumbnail.FileName).ToLower();
                    var fileName = $"thumb_{DateTime.UtcNow:yyyyMMddHHmmss}{extension}";
                    var filePath = Path.Combine(newsDir, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await newsDto.Thumbnail.CopyToAsync(stream);
                    }
                    news.ThumbnailPath = fileName;
                    _logger.LogInformation("Uploaded new thumbnail for news ID: {NewsId}, Path: {Path}", id, news.ThumbnailPath);
                }
                else
                {
                    _logger.LogInformation("No new thumbnail provided for news ID: {NewsId}, keeping existing: {Path}", id, news.ThumbnailPath ?? "none");
                }

                // Обновляем фотографии, если загружены
                if (newsDto.Photos != null && newsDto.Photos.Any())
                {
                    // Удаляем старые фотографии
                    if (news.PhotoPaths != null && news.PhotoPaths.Any())
                    {
                        foreach (var oldPhotoPath in news.PhotoPaths)
                        {
                            var fullPath = Path.Combine(_newsUploadsPath, id.ToString(), Path.GetFileName(oldPhotoPath));
                            if (System.IO.File.Exists(fullPath))
                            {
                                System.IO.File.Delete(fullPath);
                                _logger.LogInformation("Deleted old photo for news ID: {NewsId}, Path: {Path}", id, fullPath);
                            }
                        }
                    }

                    // Сохраняем новые фотографии
                    news.PhotoPaths = new List<string>();
                    foreach (var photo in newsDto.Photos)
                    {
                        if (photo.Length == 0)
                            continue;

                        // Проверка размера
                        if (photo.Length > 5 * 1024 * 1024)
                        {
                            _logger.LogWarning("Photo too large for news ID: {NewsId}, Size: {Size}", id, photo.Length);
                            throw new ArgumentException("Photo size exceeds 5 MB");
                        }

                        // Проверка типа
                        var allowedTypes = new[] { "image/png", "image/jpeg", "image/jpg" };
                        if (!allowedTypes.Contains(photo.ContentType))
                        {
                            _logger.LogWarning("Invalid photo type for news ID: {NewsId}, Type: {Type}", id, photo.ContentType);
                            throw new ArgumentException("Only PNG, JPG, JPEG files are allowed for photos");
                        }

                        var extension = Path.GetExtension(photo.FileName).ToLower();
                        var fileName = $"photo_{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid().ToString("N")[..8]}{extension}";
                        var filePath = Path.Combine(newsDir, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await photo.CopyToAsync(stream);
                        }
                        news.PhotoPaths.Add(fileName);
                        _logger.LogInformation("Uploaded new photo for news ID: {NewsId}, Path: {Path}", id, filePath);
                    }
                }
                else
                {
                    _logger.LogInformation("No new photos provided for news ID: {NewsId}, keeping existing: {Count}", id, news.PhotoPaths?.Count ?? 0);
                }

                await _newsRepository.UpdateNewsAsync(news);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update news with ID {NewsId}.", id);
                throw;
            }
        }

        public async Task DeleteNewsAsync(int id)
        {
            _logger.LogInformation("Deleting news with ID {NewsId}.", id); try
            {
                var news = await _newsRepository.GetNewsByIdAsync(id); 
                if (news == null) 
                { 
                    _logger.LogWarning("News with ID {NewsId} not found for deletion.", id); 
                    throw new KeyNotFoundException("News not found."); 
                }

                // Удаляем файлы
                var newsDir = Path.Combine(_newsUploadsPath, id.ToString());
                if (Directory.Exists(newsDir))
                {
                    foreach (var file in Directory.GetFiles(newsDir))
                    {
                        System.IO.File.Delete(file);
                        _logger.LogInformation("Deleted file for news ID: {NewsId}, File: {File}", id, file);
                    }
                    Directory.Delete(newsDir);
                    _logger.LogInformation("Deleted directory for news ID: {NewsId}, Directory: {Dir}", id, newsDir);
                }

                // Удаляем новость
                await _newsRepository.DeleteNewsAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete news with ID {NewsId}.", id);
                throw;
            }

        }

    }
}
