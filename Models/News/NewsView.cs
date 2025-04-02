using System.Text.Json.Serialization;

namespace OntuPhdApi.Models.News
{
    public class NewsView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string MainTag { get; set; }
        public List<string> OtherTags { get; set; }

        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime Date { get; set; }
        public List<string> Photos { get; set; }
        public string Body { get; set; }

    }
}
