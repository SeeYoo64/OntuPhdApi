using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OntuPhdApi.Models.Programs
{
    public class ProgramsDegreeDto
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
        public ShortSpeciality Speciality { get; set; }
    }
}
