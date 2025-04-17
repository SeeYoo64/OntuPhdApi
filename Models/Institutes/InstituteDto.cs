using System.ComponentModel.DataAnnotations;

namespace OntuPhdApi.Models.Institutes
{
    public class InstituteDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
