using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OntuPhdApi.Models.Programs
{
    public class ProgramModel
    {
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [Required]
        [JsonPropertyName("degree")]
        public string Degree { get; set; }

        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("nameCode")]
        public string? NameCode { get; set; }

        [Column(TypeName = "jsonb")]
        [JsonPropertyName("fieldOfStudy")]
        public FieldOfStudy FieldOfStudy { get; set; }

        [Column(TypeName = "jsonb")]
        [JsonPropertyName("speciality")]
        public Speciality Speciality { get; set; }

        [Column(TypeName = "jsonb")]
        [JsonPropertyName("form")]
        public List<string> Form { get; set; }

        [JsonPropertyName("objects")]
        public string? Objects { get; set; }

        [Column(TypeName = "jsonb")]
        [JsonPropertyName("directions")]
        public List<string>? Directions { get; set; }

        [JsonPropertyName("descriptions")]
        public string? Descriptions { get; set; }

        [JsonPropertyName("purpose")]
        public string? Purpose { get; set; }

        [JsonPropertyName("years")]
        public int? Years { get; set; }

        [JsonPropertyName("credits")]
        public int? Credits { get; set; }

        [Column(TypeName = "jsonb")]
        [JsonPropertyName("programCharacteristics")]
        public ProgramCharacteristics? ProgramCharacteristics { get; set; }

        [Column(TypeName = "jsonb")]
        [JsonPropertyName("programCompetence")]
        public ProgramCompetence? ProgramCompetence { get; set; }

        [Column(TypeName = "jsonb")]
        [JsonPropertyName("results")]
        public List<string>? Results { get; set; }

        [JsonPropertyName("linkFaculty")]
        public string? LinkFaculty { get; set; }

        [JsonIgnore]
        [JsonPropertyName("programDocumentId")]
        public int? ProgramDocumentId { get; set; }

        [Column(TypeName = "jsonb")]
        [JsonPropertyName("programDocument")]
        public ProgramDocument ProgramDocument { get; set; }

        [JsonIgnore]
        [JsonPropertyName("components")]
        public List<ProgramComponent>? Components { get; set; }

        [JsonIgnore]
        [JsonPropertyName("jobs")]
        public List<Job>? Jobs { get; set; }

        [JsonPropertyName("accredited")]
        public bool Accredited { get; set; }
    }

    public class FieldOfStudy
    {
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }

    public class FieldOfStudyDto
    {
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [Column(TypeName = "jsonb")]
        [JsonPropertyName("specialities")]
        public List<Speciality>? Specialities { get; set; }

        [JsonPropertyName("degree")]
        public string? Degree { get; set; }
    }

    public class ShortSpeciality
    {
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }

    public class Speciality
    {
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("fieldCode")]
        public string? FieldCode { get; set; }
    }

    public class ProgramComponent
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("programId")]
        public int? ProgramId { get; set; }

        [JsonPropertyName("componentType")]
        public string? ComponentType { get; set; }

        [JsonPropertyName("componentName")]
        public string? ComponentName { get; set; }

        [JsonPropertyName("componentCredits")]
        public int? ComponentCredits { get; set; }

        [JsonPropertyName("componentHours")]
        public int? ComponentHours { get; set; }

        [Column(TypeName = "jsonb")]
        [JsonPropertyName("controlForm")]
        public List<string>? ControlForm { get; set; }
    }

    public class ProgramCompetence
    {
        [Column(TypeName = "jsonb")]
        [JsonPropertyName("overallCompetence")]
        public List<string>? OverallCompetence { get; set; }

        [Column(TypeName = "jsonb")]
        [JsonPropertyName("specialCompetence")]
        public List<string>? SpecialCompetence { get; set; }

        [JsonPropertyName("integralCompetence")]
        public string? IntegralCompetence { get; set; }
    }

    public class ProgramCharacteristics
    {
        [Column(TypeName = "jsonb")]
        [JsonPropertyName("area")]
        public Area? Area { get; set; }

        [JsonPropertyName("focus")]
        public string? Focus { get; set; }

        [Column(TypeName = "jsonb")]
        [JsonPropertyName("features")]
        public List<string>? Features { get; set; }
    }

    public class Area
    {
        [JsonPropertyName("object")]
        public string? Object { get; set; }

        [JsonPropertyName("aim")]
        public string? Aim { get; set; }

        [JsonPropertyName("theory")]
        public string? Theory { get; set; }

        [JsonPropertyName("methods")]
        public string? Methods { get; set; }

        [JsonPropertyName("instruments")]
        public string? Instruments { get; set; }
    }

    public class Job
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
    }
}