using OntuPhdApi.Models.Programs.Components;
using System.ComponentModel.DataAnnotations;

namespace OntuPhdApi.Models.Programs.Dto
{
    public class ProgramCreateUpdateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Degree { get; set; }
        public string? NameCode { get; set; }
        public FieldOfStudyDto? FieldOfStudy { get; set; }
        public SpecialityDto? Speciality { get; set; }
        public List<string>? Form { get; set; }
        public string? Objects { get; set; }
        public List<string>? Directions { get; set; }
        public string? Descriptions { get; set; }
        public string? Purpose { get; set; }
        public string? Institute { get; set; }
        public int? Years { get; set; }
        public int? Credits { get; set; }
        public List<string>? Results { get; set; }
        public List<LinkFacultyDto>? LinkFaculties { get; set; }
        public ProgramCharacteristicsCreateDto? ProgramCharacteristics { get; set; }
        public ProgramCompetenceCreateDto? ProgramCompetence { get; set; }
        public List<ProgramComponentCreateDto>? ProgramComponents { get; set; }
        public List<JobCreateDto>? Jobs { get; set; }
        public bool Accredited { get; set; }
    }


    public class JobCreateDto
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string Title { get; set; }
    }


    public class ProgramCharacteristicsCreateDto
    {
        public string Focus { get; set; }
        public string Features { get; set; }
        public AreaCreateDto Area { get; set; }
    }

    public class AreaCreateDto
    {
        public string Object { get; set; }
        public string Aim { get; set; }
        public string Theory { get; set; }
        public string Methods { get; set; }
        public string Instruments { get; set; }
    }

    public class ProgramCompetenceCreateDto
    {
        public string IntegralCompetence { get; set; }
        public List<OverallCompetenceCreateDto> OverallCompetences { get; set; }
        public List<SpecialCompetenceCreateDto> SpecialCompetences { get; set; }
    }

    public class OverallCompetenceCreateDto
    {
        [Required]
        public string Description { get; set; }
    }

    public class SpecialCompetenceCreateDto
    {
        [Required]
        public string Description { get; set; }
    }

    public class ProgramComponentCreateDto
    {
        public string ComponentType { get; set; }
        public string ComponentName { get; set; }
        public int? ComponentCredits { get; set; }
        public int? ComponentHours { get; set; }
        public List<ControlFormCreateDto> ControlForms { get; set; }
    }

    public class ControlFormCreateDto
    {
        [Required]
        public string Type { get; set; }
    }



}
