namespace OntuPhdApi.Models.Programs
{
    public class ProgramsDegreeDto
    {
        public int Id { get; set; }
        public string Degree { get; set; }
        public string Name { get; set; }
        public string NameEng { get; set; }
        public FieldOfStudy FieldOfStudy { get; set; }
        public Speciality Speciality { get; set; }
    }
}
