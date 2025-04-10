namespace OntuPhdApi.Models.Programs
{
    public class ProgramDocument
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadDate { get; set; }
        public long? FileSize { get; set; }
        public string ContentType { get; set; }

    }
}
