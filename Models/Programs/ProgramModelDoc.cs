using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OntuPhdApi.Models.Programs
{
    public class ProgramModelDoc
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

        [JsonPropertyName("descriptions")]
        public string? Descriptions { get; set; }

        [Column(TypeName = "jsonb")]
        [JsonPropertyName("directions")]
        public List<string>? Directions { get; set; }

        [Column(TypeName = "jsonb")]
        [JsonPropertyName("linkFaculty")]
        public string? LinkFaculty { get; set; }

        [JsonPropertyName("accredited")]
        public bool Accredited { get; set; }
        public int ProgramDocumentId { get; set; }

    }
}
