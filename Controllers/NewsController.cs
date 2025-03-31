using Microsoft.AspNetCore.Mvc;
using OntuPhdApi.Models;
using OntuPhdApi.Services;

namespace OntuPhdApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly DatabaseService _dbService;
        private readonly IWebHostEnvironment _environment;

        public NewsController(DatabaseService dbService, IWebHostEnvironment environment)
        {
            _dbService = dbService;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult GetNews()
        {
            try
            {
                var news = _dbService.GetNews();
                return Ok(news);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetNews(int id)
        {
            try
            {
                var news = _dbService.GetNewsById(id);
                if (news == null)
                {
                    return NotFound($"News with ID {id} not found.");
                }
                return Ok(news);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddNews([FromForm] NewsDto newsDto)
        {
            if (newsDto == null ||
                string.IsNullOrEmpty(newsDto.Title) ||
                string.IsNullOrEmpty(newsDto.Summary) ||
                string.IsNullOrEmpty(newsDto.MainTag) ||
                newsDto.Thumbnail == null ||
                newsDto.OtherTags == null ||
                newsDto.Photos == null ||
                newsDto.Body == null)
            {
                return BadRequest("Invalid news data. Title, Summary, MainTag, Thumbnail, OtherTags, Photos, and Body are required.");
            }

            try
            {
                // Создаём объект News
                var news = new News
                {
                    Title = newsDto.Title,
                    Summary = newsDto.Summary,
                    MainTag = newsDto.MainTag,
                    OtherTags = newsDto.OtherTags,
                    Date = newsDto.Date,
                    Body = newsDto.Body
                };

                // Сохраняем новость в базе, чтобы получить Id
                _dbService.AddNews(news);

                // Создаём директорию для файлов новости
                var newsDir = Path.Combine(_environment.ContentRootPath, "Files", "Uploads", "News", news.Id.ToString());
                Directory.CreateDirectory(newsDir);

                // Сохраняем Thumbnail
                var thumbnailExtension = Path.GetExtension(newsDto.Thumbnail.FileName);
                var thumbnailPath = Path.Combine(newsDir, $"thumbnail{thumbnailExtension}");
                using (var stream = new FileStream(thumbnailPath, FileMode.Create))
                {
                    await newsDto.Thumbnail.CopyToAsync(stream);
                }
                news.Thumbnail = Path.Combine("Files", "Uploads", "News", news.Id.ToString(), $"thumbnail{thumbnailExtension}").Replace("\\", "/");

                // Сохраняем Photos
                news.Photos = new List<string>();
                for (int i = 0; i < newsDto.Photos.Count; i++)
                {
                    var photo = newsDto.Photos[i];
                    var photoExtension = Path.GetExtension(photo.FileName);
                    var photoPath = Path.Combine(newsDir, $"photo{i + 1}{photoExtension}");
                    using (var stream = new FileStream(photoPath, FileMode.Create))
                    {
                        await photo.CopyToAsync(stream);
                    }
                    news.Photos.Add(Path.Combine("Files", "Uploads", "News", news.Id.ToString(), $"photo{i + 1}{photoExtension}").Replace("\\", "/"));
                }

                // Обновляем запись в базе с путями к файлам
                _dbService.UpdateNews(news);

                return CreatedAtAction(nameof(GetNews), new { id = news.Id }, news);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("download")]
        public IActionResult DownloadFile([FromQuery] string filePath)
        {
            try
            {
                // Проверяем, что путь начинается с "Files/Uploads/News"
                if (string.IsNullOrEmpty(filePath) || !filePath.StartsWith("Files/Uploads/News"))
                {
                    return BadRequest("Invalid file path.");
                }

                var fullPath = Path.Combine(_environment.ContentRootPath, filePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
                if (!System.IO.File.Exists(fullPath))
                {
                    return NotFound($"File {filePath} not found.");
                }

                var fileBytes = System.IO.File.ReadAllBytes(fullPath);
                var fileName = Path.GetFileName(fullPath);
                return File(fileBytes, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("latest")]
        public IActionResult GetLatestNews()
        {
            try
            {
                var latestNews = _dbService.GetLatestNews(4);
                return Ok(latestNews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



    }
}