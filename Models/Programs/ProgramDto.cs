using System.ComponentModel.DataAnnotations;
using OntuPhdApi.Models.Institutes;
using OntuPhdApi.Models.Programs.Components;

namespace OntuPhdApi.Models.Programs
{
    public class ProgramResponseDto
    {
        public int Id { get; set; }
        public string Degree { get; set; }
        public string Name { get; set; }
        public string NameCode { get; set; }
        public FieldOfStudy FieldOfStudy { get; set; }
        public Speciality Speciality { get; set; }
        public List<string> Form { get; set; }
        public string Objects { get; set; }
        public List<string> Directions { get; set; }
        public string Descriptions { get; set; }
        public string Purpose { get; set; }
        public int? InstituteId { get; set; }
        public int? Years { get; set; }
        public int? Credits { get; set; }
        public List<string> Results { get; set; }
        public List<LinkFacultyDto> LinkFaculties { get; set; }
        public ProgramDocumentDto ProgramDocument { get; set; }
        public bool Accredited { get; set; }
    }

    public class ProgramCreateDto
    {
        [Required]
        public string Degree { get; set; }

        [Required]
        public string Name { get; set; }

        public string NameCode { get; set; }
        public FieldOfStudy FieldOfStudy { get; set; }
        public Speciality Speciality { get; set; }
        public List<string> Form { get; set; }
        public string Objects { get; set; }
        public List<string> Directions { get; set; }
        public string Descriptions { get; set; }
        public string Purpose { get; set; }
        public int? InstituteId { get; set; }
        public int? Years { get; set; }
        public int? Credits { get; set; }
        public List<string> Results { get; set; }
        public List<LinkFacultyDto> LinkFaculties { get; set; }
        public bool Accredited { get; set; }
    }

    public class ProgramUpdateDto
    {
        [Required]
        public string Degree { get; set; }

        [Required]
        public string Name { get; set; }

        public string NameCode { get; set; }
        public FieldOfStudy FieldOfStudy { get; set; }
        public Speciality Speciality { get; set; }
        public List<string> Form { get; set; }
        public string Objects { get; set; }
        public List<string> Directions { get; set; }
        public string Descriptions { get; set; }
        public string Purpose { get; set; }
        public int? InstituteId { get; set; }
        public int? Years { get; set; }
        public int? Credits { get; set; }
        public List<string> Results { get; set; }
        public List<LinkFacultyDto> LinkFaculties { get; set; }
        public bool Accredited { get; set; }
    }
}
