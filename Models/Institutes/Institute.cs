using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using OntuPhdApi.Models.Programs;

namespace OntuPhdApi.Models.Institutes
{
    public class Institute
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [JsonIgnore]
        public List<ProgramModel>? Programs { get; set; }
    }
}
