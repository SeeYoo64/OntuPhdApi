namespace OntuPhdApi.Models
{
    public class ApplyDocuments
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Requirements> Requirements { get; set; } // Теперь список
        public List<Requirements> OriginalsRequired { get; set; } // Теперь список
    }

    public class Requirements
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }

}