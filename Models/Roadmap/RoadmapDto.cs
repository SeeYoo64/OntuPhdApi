using System.Text.Json.Serialization;

namespace OntuPhdApi.Models.Roadmap
{
    public class RoadmapModelDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime DataStart { get; set; }
        [JsonConverter(typeof(NullableDateOnlyConverter))]
        public DateTime? DataEnd { get; set; }
        public string? AdditionalTime { get; set; }
        public string Description { get; set; }
        public RoadmapStatus Status { get; set; }
    }
}
