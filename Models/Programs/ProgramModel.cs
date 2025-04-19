using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using OntuPhdApi.Models.Institutes;
using OntuPhdApi.Models.Programs.Components;

namespace OntuPhdApi.Models.Programs
{
    public class ProgramModel
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
        public int? InstituteId { get; set; }
        public Institute? Institute { get; set; }
        public int? Years { get; set; }
        public int? Credits { get; set; }
        public List<string>? Results { get; set; }
        public int? ProgramDocumentId { get; set; }
        public ProgramDocument? ProgramDocument { get; set; }
        public List<LinkFaculty>? LinkFaculties { get; set; }
        public ProgramCharacteristics? ProgramCharacteristics { get; set; }
        public ProgramCompetence? ProgramCompetence { get; set; }
        public List<ProgramComponent>? ProgramComponents { get; set; }
        public List<Job>? Jobs { get; set; }
        public bool Accredited { get; set; }
    }


    public class FieldOfStudyDto
    {
        public string? Code { get; set; }

        public string? Name { get; set; }
    }

    public class FieldOfStudy
    {
        public string? Code { get; set; }

        public string? Name { get; set; }

        public string? Degree { get; set; }
    }

    public class SpecialityDto
    {
        public string? Code { get; set; }

        public string? Name { get; set; }
    }

    public class Speciality
    {
        public string? Code { get; set; }

        public string? Name { get; set; }
        public string? FieldCode { get; set; }
    }

}