namespace OntuPhdApi.Models.Programs
{
    public class ProgramModelDoc
    {
        public int Id { get; set; }
        public string Degree { get; set; }
        public string Name { get; set; }
        public string? NameCode { get; set; }
        public bool Accredited { get; set; }
        public FieldOfStudy FieldOfStudy { get; set; }
        public ShortSpeciality Speciality { get; set; }
        public List<string> Form { get; set; }
        public string Description { get; set; }
        public string Objects { get; set; }
        public List<string> Directions { get; set; }
        public string LinkFaculty { get; set; }
        public string LinkFile { get; set; }

    }
}
