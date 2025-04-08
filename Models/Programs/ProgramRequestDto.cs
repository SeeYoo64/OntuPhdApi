using Microsoft.AspNetCore.Mvc;

namespace OntuPhdApi.Models.Programs
{
    public class ProgramRequestDto
    {
        public string? Degree { get; set; }
        public string? Name { get; set; }
        public string? NameCode { get; set; }
        public FieldOfStudy? FieldOfStudy { get; set; }
        public Speciality? Speciality { get; set; }
        [FromForm(Name = "Form")]
        public List<string>? Form { get; set; }
        public string? Objects { get; set; }
        [FromForm(Name = "Directions")]
        public List<string>? Directions { get; set; }
        public string? Descriptions { get; set; }
        public string? Purpose { get; set; }
        public int? Years { get; set; }
        public int? Credits { get; set; }
        public ProgramCharacteristics? ProgramCharacteristics { get; set; }
        public ProgramCompetence? ProgramCompetence { get; set; }
        [FromForm(Name = "Results")]
        public List<string>? Results { get; set; }
        public string? LinkFaculty { get; set; }
        public List<ProgramComponent>? Components { get; set; }
        public List<Job>? Jobs { get; set; }
        public bool Accredited { get; set; }
        public IFormFile? File { get; set; }
    }
}
