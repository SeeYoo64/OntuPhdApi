namespace OntuPhdApi.Models.Programs
{
    public class ProgramModelDto
    {
        public int Id { get; set; }
        public string Degree { get; set; }
        public string Name { get; set; }
        public string? NameCode { get; set; }
        public FieldOfStudy? FieldOfStudy { get; set; }
        public Speciality? Speciality { get; set; }
        public List<string>? Form { get; set; }
        public string? Objects { get; set; }
        public List<string>? Directions { get; set; }
        public string? Descriptions { get; set; }
        public string? Purpose { get; set; }
        public int? Years { get; set; }
        public int? Credits { get; set; }
        public ProgramCharacteristics? ProgramCharacteristics { get; set; }
        public ProgramCompetence? ProgramCompetence { get; set; }
        public List<string>? Results { get; set; }
        public string? LinkFaculty { get; set; }
        public int? ProgramDocumentId { get; set; }
        public ProgramDocument? ProgramDocument { get; set; }
        public List<ProgramComponent>? Components { get; set; }
        public List<Job>? Jobs { get; set; }
        public bool Accredited { get; set; }
        public string? Institute { get; set; }
    }
}
