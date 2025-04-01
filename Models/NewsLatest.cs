using System.Text.Json.Serialization;

namespace OntuPhdApi.Models
{
    public class NewsLatest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string MainTag { get; set; }

        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime Date { get; set; }
        public string Thumbnail { get; set; }
    }
}
