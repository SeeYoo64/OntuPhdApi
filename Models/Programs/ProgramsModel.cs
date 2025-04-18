using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using OntuPhdApi.Models.Institutes;
using OntuPhdApi.Models.Programs.Components;

namespace OntuPhdApi.Models.Programs
{
    public class ProgramModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Degree { get; set; }

        [Required]
        public string Name { get; set; }

        public string? NameCode { get; set; }
        [Column(TypeName = "jsonb")]
        public FieldOfStudy? FieldOfStudy { get; set; }

        [Column(TypeName = "jsonb")]
        public Speciality? Speciality { get; set; }

        [Column(TypeName = "jsonb")]
        public List<string>? Form { get; set; }

        public string? Objects { get; set; }

        [Column(TypeName = "jsonb")]
        public List<string>? Directions { get; set; }

        public string? Descriptions { get; set; }

        public string? Purpose { get; set; }

        public int? InstituteId { get; set; }
        public Institute? Institute { get; set; }

        public int? Years { get; set; }

        public int? Credits { get; set; }

        public List<string>? Results { get; set; }

        public List<LinkFaculty> LinkFaculties { get; set; } = new();

        public int? ProgramDocumentId { get; set; }
        public ProgramDocument? ProgramDocument { get; set; }


        public ProgramCharacteristics? ProgramCharacteristics { get; set; }
        public ProgramCompetence? ProgramCompetence { get; set; }
        public List<ProgramComponent>? ProgramComponents { get; set; }
        public List<Job>? Jobs { get; set; }

        public bool Accredited { get; set; }
    }


    public class FieldOfStudy
    {
        public string? Code { get; set; }

        public string? Name { get; set; }
    }

    public class FieldOfStudyDto
    {
        public string? Code { get; set; }

        public string? Name { get; set; }

        [Column(TypeName = "jsonb")]
        public List<Speciality>? Specialities { get; set; }

        public string? Degree { get; set; }
    }

    public class ShortSpeciality
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