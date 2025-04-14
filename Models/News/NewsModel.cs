using System.Text.Json.Serialization;

namespace OntuPhdApi.Models.News
{
    public class NewsModel
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
}