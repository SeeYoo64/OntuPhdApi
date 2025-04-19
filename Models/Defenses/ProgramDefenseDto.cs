using OntuPhdApi.Models.Programs;

namespace OntuPhdApi.Models.Defense
{
    public class ProgramDefenseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Degree { get; set; }
        public FieldOfStudy? FieldOfStudy { get; set; }
        public SpecialityDto? Speciality { get; set; }
    }
}
