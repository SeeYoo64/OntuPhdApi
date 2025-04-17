using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using OntuPhdApi.Models.Institutes;

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
        [ForeignKey("Institute")]
        public int? InstituteId { get; set; } 

        public Institute? Institute { get; set; }
        public int? Years { get; set; }

        public int? Credits { get; set; }

        [Column(TypeName = "jsonb")]
        public ProgramCharacteristics? ProgramCharacteristics { get; set; }

        [Column(TypeName = "jsonb")]
        public ProgramCompetence? ProgramCompetence { get; set; }

        [Column(TypeName = "jsonb")]
        public List<string>? Results { get; set; }

        public string? LinkFaculty { get; set; }

        [JsonIgnore]
        public int? ProgramDocumentId { get; set; }

        public ProgramDocument? ProgramDocument { get; set; }

        [JsonIgnore]
        public List<ProgramComponent>? Components { get; set; }

        [JsonIgnore]
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

    public class ProgramComponent
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        public int? ProgramId { get; set; }

        public string? ComponentType { get; set; }

        public string? ComponentName { get; set; }

        public int? ComponentCredits { get; set; }

        public int? ComponentHours { get; set; }

        public List<string>? ControlForm { get; set; }
        [JsonIgnore]
        public ProgramModel? ProgramModel { get; set; }
    }

    public class ProgramCompetence
    {
        public List<string>? OverallCompetence { get; set; }

        public List<string>? SpecialCompetence { get; set; }

        public string? IntegralCompetence { get; set; }
    }

    public class ProgramCharacteristics
    {
        public Area? Area { get; set; }

        public string? Focus { get; set; }

        public string? Features { get; set; }
    }

    public class Area
    {
        public string? Object { get; set; }
        public string? Aim { get; set; }
        public string? Theory { get; set; }
        public string? Methods { get; set; }
        public string? Instruments { get; set; }
    }

    public class Job
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        [JsonIgnore]
        public int ProgramId { get; set; }
        [JsonIgnore]
        [Column(TypeName = "jsonb")]
        public ProgramModel ProgramModel { get; set; }
    }
}