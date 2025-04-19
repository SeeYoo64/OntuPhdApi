using System.Text.Json.Serialization;
using OntuPhdApi.Models.Programs;

namespace OntuPhdApi.Models.Programs.Components
{
    public class Job
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public int ProgramId { get; set; }
        [JsonIgnore]
        public ProgramModel ProgramModel { get; set; }
    }
}