using System.Text.Json.Serialization;

namespace OntuPhdApi.Models
{
    public class NewsView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string MainTag { get; set; }

        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime Date { get; set; }
        public string Thumbnail { get; set; }

    }
}
