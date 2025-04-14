using Microsoft.AspNetCore.Mvc;
using OntuPhdApi.Models.News;
using OntuPhdApi.Services;
using OntuPhdApi.Services.News;
using OntuPhdApi.Services.Programs;

namespace OntuPhdApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        private readonly ILogger<NewsController> _logger;

        public NewsController(INewsService newsService, ILogger<NewsController> logger)
        {
            _newsService = newsService;
            _logger = logger;
        }


        [HttpGet]
        public async Task<IActionResult> GetNews()
        {
            _logger.LogInformation("Fetching all news.");
            try
            {
                var news = await _newsService.GetNewsAsync();
                return Ok(news);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch news.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetNewsById(int id)
        {
            _logger.LogInformation("Fetching news with ID {NewsId}.", id);
            try
            {
                var news = await _newsService.GetNewsByIdAsync(id);
                if (news == null)
                {
                    _logger.LogWarning("News with ID {NewsId} not found.", id);
                    return StatusCode(404, $"News with ID {id} was not found");

                }
                return Ok(news);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid request for news with ID {NewsId}: {ErrorMessage}", id, ex.Message);
                return StatusCode(400, $"Bad request error: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch news with ID {NewsId}.", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestNews([FromQuery] int count = 4)
        {
            _logger.LogInformation("Fetching {Count} latest news.", count);
            try
            {
                var news = await _newsService.GetLatestNewsAsync(count);
                return Ok(news);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid count parameter {Count}: {ErrorMessage}", count, ex.Message);
                return StatusCode(400, $"Bad Request error: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch {Count} latest news.", count);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddNews([FromForm] NewsCreateUpdateDto newsDto)
        {
            _logger.LogInformation("Adding new news with title {NewsTitle}.", newsDto.Title);
            try
            {
                await _newsService.AddNewsAsync(newsDto);
                return CreatedAtAction(
                    nameof(GetNewsById),
                    new { id = 0 }, // ID можно получить из сервиса, если нужно
                    newsDto
                );
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid news data: {ErrorMessage}", ex.Message);
                return StatusCode(400, $"Bad Request error: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add news with title {NewsTitle}.", newsDto.Title);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNews(int id, [FromForm] NewsCreateUpdateDto newsDto)
        {
            _logger.LogInformation("Updating news with ID {NewsId}.", id);
            try
            {
                await _newsService.UpdateNewsAsync(id, newsDto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid news data: {ErrorMessage}", ex.Message);
                return StatusCode(400, $"Bad Request error: {ex.Message}");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("News with ID {NewsId} not found.", id);
                return StatusCode(404, $"News with ID {id} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update news with ID {NewsId}.", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



    }
}