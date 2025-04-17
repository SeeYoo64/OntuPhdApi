using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OntuPhdApi.Models.Programs
{
    public class ProgramsDegreeDto
    {
        [Key]
        public int Id { get; set; }
        public string Degree { get; set; }
        public string Name { get; set; }
        public FieldOfStudy FieldOfStudy { get; set; }
        public ShortSpeciality Speciality { get; set; }
    }
}
