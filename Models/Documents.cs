namespace OntuPhdApi.Models
{
    public class Documents
    {
        public int Id { get; set; }
        public int? ProgramId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Link { get; set; }
    }
}