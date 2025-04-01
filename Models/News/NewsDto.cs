namespace OntuPhdApi.Models.News
{
    public class NewsDto
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public string MainTag { get; set; }
        public List<string> OtherTags { get; set; }
        public DateTime Date { get; set; }
        public IFormFile Thumbnail { get; set; } 
        public List<IFormFile> Photos { get; set; } 
        public string Body { get; set; }
    }
}