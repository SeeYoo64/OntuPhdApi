using System.ComponentModel.DataAnnotations;

namespace OntuPhdApi.Models.Programs.Components
{
    public class Institute
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class InstituteDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }

}