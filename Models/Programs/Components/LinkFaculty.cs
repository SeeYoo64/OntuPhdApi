namespace OntuPhdApi.Models.Programs.Components
{
    public class LinkFacultyDto
    {
        public string Name { get; set; }
        public string Link { get; set; }
    }
    public class LinkFaculty
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Link { get; set; }

        public int ProgramId { get; set; }
        public ProgramModel ProgramModel { get; set; }
    }

}
