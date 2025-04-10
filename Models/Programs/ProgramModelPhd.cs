using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OntuPhdApi.Models.Programs
{
    public class ProgramModelPhd
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

        [Column(TypeName = "jsonb")]
        [JsonPropertyName("fieldOfStudy")]
        public FieldOfStudy FieldOfStudy { get; set; }

        [Column(TypeName = "jsonb")]
        [JsonPropertyName("speciality")]
        public Speciality Speciality { get; set; }
        [Column(TypeName = "jsonb")]
        [JsonPropertyName("form")]
        public List<string> Form { get; set; }
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
        [JsonPropertyName("linkFaculty")]
        public string? LinkFaculty { get; set; }
        [JsonPropertyName("accredited")]
        public bool Accredited { get; set; }
        public int ProgramDocumentId { get; set; }

    }
}
