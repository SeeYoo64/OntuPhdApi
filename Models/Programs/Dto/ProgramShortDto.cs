using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using OntuPhdApi.Models.Programs.Components;

namespace OntuPhdApi.Models.Programs.Dto
{
    public class ProgramShortDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Degree { get; set; }
        public FieldOfStudyDto FieldOfStudy { get; set; }
        public SpecialityDto Speciality { get; set; }
        public string Institute { get; set; }
    }
}
