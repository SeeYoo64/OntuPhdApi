namespace OntuPhdApi.Models.Programs.Components
{
    public class Job
    {
        public int Id { get; set; }
        public int ProgramId { get; set; }

        public string Code { get; set; }
        public string Title { get; set; }

        public ProgramModel ProgramModel { get; set; }
    }
}
