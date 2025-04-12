namespace OntuPhdApi.Models
{
    public class ApplyDocumentsModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Requirements> Requirements { get; set; } 
        public List<Requirements> OriginalsRequired { get; set; } 
    }

    public class Requirements
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }

}