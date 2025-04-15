namespace OntuPhdApi.Models.Documents
{
    public class DocumentModel
    {
        public int Id { get; set; }
        public int? ProgramId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Link { get; set; }
    }
    public class DocumentDto
    {
        public int Id { get; set; }
        public int? ProgramId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Link { get; set; }
    }

    public class DocumentCreateUpdateDto
    {
        public int? ProgramId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Link { get; set; }
    }
}