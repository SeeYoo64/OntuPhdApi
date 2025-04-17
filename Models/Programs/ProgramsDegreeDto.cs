using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OntuPhdApi.Models.Programs
{
    public class ProgramsDegreeDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Degree { get; set; }

        [Required]
        public string Name { get; set; }
        [Column(TypeName = "jsonb")]
        public FieldOfStudy FieldOfStudy { get; set; }

        [Column(TypeName = "jsonb")]
        public ShortSpeciality Speciality { get; set; }
    }
}
