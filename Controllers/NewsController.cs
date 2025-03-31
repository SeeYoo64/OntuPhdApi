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

        public NewsController(DatabaseService dbService)
        {
            _dbService = dbService;
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
        public IActionResult AddNews([FromBody] News news)
        {
            if (news == null ||
                string.IsNullOrEmpty(news.Title) ||
                string.IsNullOrEmpty(news.Summary) ||
                string.IsNullOrEmpty(news.MainTag) ||
                string.IsNullOrEmpty(news.Thumbnail) ||
                news.OtherTags == null ||
                news.Photos == null ||
                news.Body == null)
            {
                return BadRequest("Invalid news data. Title, Summary, MainTag, Thumbnail, OtherTags, Photos, and Body are required.");
            }

            try
            {
                _dbService.AddNews(news);
                return CreatedAtAction(nameof(GetNews), new { id = news.Id }, news);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}