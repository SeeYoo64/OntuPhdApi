using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using OntuPhdApi.Models.Programs.Components;

namespace OntuPhdApi.Models.Programs.Dto
{
    public class ProgramDegreeDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Degree { get; set; }

        [Required]
        public string Name { get; set; }
        public FieldOfStudyDto FieldOfStudy { get; set; }

        public SpecialityDto Speciality { get; set; }
    }
}
