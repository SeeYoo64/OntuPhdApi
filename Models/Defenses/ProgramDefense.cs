using OntuPhdApi.Models.Programs;
using OntuPhdApi.Models.Programs.Components;

namespace OntuPhdApi.Models.Defense
{
    public class ProgramDefense
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Degree { get; set; }
        public FieldOfStudy FieldOfStudy { get; set; }
        public SpecialityDto Speciality { get; set; }
    }
}
