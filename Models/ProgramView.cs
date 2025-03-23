namespace OntuPhdApi.Models
{
    public class ProgramView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public FieldOfStudy FieldOfStudy { get; set; }  // Объект с code и name
        public Speciality Speciality { get; set; }      // Объект с code и name
        public List<string> Form { get; set; }
        public int Years { get; set; }
        public int Credits { get; set; }
        public decimal Sum { get; set; }
        public List<decimal> Costs { get; set; }
        public ProgramCharacteristics ProgramCharacteristics { get; set; }
        public ProgramCompetence ProgramCompetence { get; set; }
        public ProgramResults ProgramResults { get; set; }
        public string LinkFaculty { get; set; }
        public string LinkFile { get; set; }
    }

    public class FieldOfStudy
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class Speciality
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
