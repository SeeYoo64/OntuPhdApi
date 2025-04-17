using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace OntuPhdApi.Models.Programs
{


    public class ProgramCreateUpdateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Degree { get; set; }
        public string? NameCode { get; set; }
        public FieldOfStudy? FieldOfStudy { get; set; }
        public Speciality? Speciality { get; set; }
        public List<string>? Form { get; set; }
        public string? Objects { get; set; }
        public List<string>? Directions { get; set; }
        public string? Descriptions { get; set; }
        public string? Purpose { get; set; }
        public int? Years { get; set; }
        public int? Credits { get; set; }
        public ProgramCharacteristics? ProgramCharacteristics { get; set; }
        public ProgramCompetence? ProgramCompetence { get; set; }
        public List<string>? Results { get; set; }
        public List<IFormFile>? Files { get; set; }
        public List<LinksFaculties>? LinkFaculty { get; set; }
        public List<ProgramComponent>? Components { get; set; }
        public List<Job>? Jobs { get; set; }
        public bool Accredited { get; set; }
    }
}
