namespace OntuPhdApi.Models.Programs.Components
{
    public class ProgramDocument
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadDate { get; set; }
        public long? FileSize { get; set; }
        public string ContentType { get; set; }
        public ProgramModel? Program { get; set; }
    }

    public class ProgramDocumentDto
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
    }

}
