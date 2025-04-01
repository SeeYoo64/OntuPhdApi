using System.Text.Json.Serialization;

namespace OntuPhdApi.Models.News
{
    public class NewsModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string MainTag { get; set; }
        public List<string> OtherTags { get; set; }

        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime Date { get; set; }

        public string Thumbnail { get; set; }
        public List<string> Photos { get; set; }
        public List<string> Body { get; set; }
    }
}