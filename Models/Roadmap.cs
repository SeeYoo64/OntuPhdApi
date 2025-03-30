namespace OntuPhdApi.Models
{
    public enum RoadmapStatus
    {
        Completed,
        Ontime,
        NotStarted
    }

    public class Roadmap
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public DateTime DataStart { get; set; }
        public DateTime? DataEnd { get; set; }
        public string? AdditionalTime { get; set; }
        public string Description { get; set; }

        public RoadmapStatus Status
        {
            get
            {
                var now = DateTime.UtcNow;
                var effectiveEnd = DataEnd ?? DataStart; 

                if (now < DataStart)
                    return RoadmapStatus.NotStarted;
                if (now >= DataStart && now <= effectiveEnd)
                    return RoadmapStatus.Ontime;
                return RoadmapStatus.Completed;
            }
        }
    }
}