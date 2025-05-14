using System.Text.Json.Serialization;

namespace OntuPhdApi.Models.News
{
    public class NewsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string MainTag { get; set; }
        public List<string>? OtherTags { get; set; }

        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime PublicationDate { get; set; }

        public string ThumbnailPath { get; set; }
        public List<string>? PhotoPaths { get; set; }
        public string Body { get; set; }
    }

    public class NewsViewDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string MainTag { get; set; }
        public List<string>? OtherTags { get; set; }

        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime PublicationDate { get; set; }

        public List<string>? PhotoPaths { get; set; }
        public string Body { get; set; }
    }

    public class NewsLatestDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string MainTag { get; set; }

        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime PublicationDate { get; set; }

        public string ThumbnailPath { get; set; }
    }

    public class NewsCreateUpdateDto
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public string MainTag { get; set; }
        public List<string>? OtherTags { get; set; }

        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime PublicationDate { get; set; }

        public IFormFile? Thumbnail { get; set; }
        public List<IFormFile>? Photos { get; set; } 
        public string Body { get; set; }
    }
}