namespace OntuPhdApi.Models
{
    public class Program
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public FieldOfStudy FieldOfStudy { get; set; }
        public Speciality Speciality { get; set; }
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
        public List<ProgramComponent> Components { get; set; }
        public List<Job> Jobs { get; set; }  // Список профессий
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