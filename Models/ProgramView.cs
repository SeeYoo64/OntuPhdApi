namespace OntuPhdApi.Models
{
    public class ProgramView
    {
        public int Id { get; set; }
        public string Name { get; set; } // Две строки названия
        public string FieldOfStudy { get; set; } // fieldCode + fieldName
        public string Speciality { get; set; } // specialityCode + specialityName
        public List<string> Form { get; set; } // Массив форм обучения
        public int Years { get; set; }
        public int Credits { get; set; }
        public decimal Sum { get; set; }
        public List<decimal> Costs { get; set; } // Массив стоимости по годам
        public ProgramCharacteristics ProgramCharacteristics { get; set; }
        public ProgramCompetence ProgramCompetence { get; set; }
        public ProgramResults ProgramResults { get; set; }
        public string LinkFaculty { get; set; }
        public string LinkFile { get; set; }
        public List<ProgramComponent> UniversalComponents { get; set; }
        public List<ProgramComponent> PracticalComponents { get; set; }
        public List<ProgramComponent> OptionalComponents { get; set; }


    }
}
