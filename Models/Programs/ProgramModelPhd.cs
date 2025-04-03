namespace OntuPhdApi.Models.Programs
{
    public class ProgramModelPhd
    {
        public int Id { get; set; }
        public string Degree { get; set; }
        public string Name { get; set; }
        public bool Accredited { get; set; }
        public FieldOfStudy FieldOfStudy { get; set; }
        public ShortSpeciality Speciality { get; set; }
        public List<string> Form { get; set; }
        public string Purpose { get; set; }
        public int? Years { get; set; }
        public int? Credits { get; set; }
        public ProgramCharacteristics ProgramCharacteristics { get; set; }
        public string LinkFaculty { get; set; }
        public string LinkFile { get; set; }

    }
}
