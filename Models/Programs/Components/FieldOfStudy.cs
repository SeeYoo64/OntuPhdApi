using System.Text.Json.Serialization;

namespace OntuPhdApi.Models.Programs.Components
{
    public class FieldOfStudy
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Degree { get; set; }

        [JsonIgnore]
        public ICollection<Speciality> Specialities { get; set; } = [];
        [JsonIgnore]
        public ICollection<ProgramModel> Programs { get; set; } = [];

    }


    public class FieldOfStudyDto
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
    }




}
