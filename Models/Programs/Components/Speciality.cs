using System.Text.Json.Serialization;

namespace OntuPhdApi.Models.Programs.Components
{
    public class Speciality
    {
        public int Id { get; set; }
        public string? Code { get; set; }

        public string? Name { get; set; }
        public string? FieldCode { get; set; }
        public int FieldOfStudyId { get; set; }
        public FieldOfStudy FieldOfStudy { get; set; }

        [JsonIgnore]
        public ICollection<ProgramModel> Programs { get; set; } = [];

    }

    public class SpecialityDto
    {
        public string? Code { get; set; }

        public string? Name { get; set; }
    }

}
