using OntuPhdApi.Models.Programs.Components;
using OntuPhdApi.Models.Programs;

namespace OntuPhdApi.Utilities.Mappers
{
    public class ProgramMapper : IProgramMapper
    {
        public ProgramResponseDto ToProgramResponseDto(ProgramModel program)
        {
            if (program == null) return null;

            return new ProgramResponseDto
            {
                Id = program.Id,
                Degree = program.Degree,
                Name = program.Name,
                NameCode = program.NameCode,
                FieldOfStudy = program.FieldOfStudy,
                Speciality = program.Speciality,
                Form = program.Form,
                Objects = program.Objects,
                Directions = program.Directions,
                Descriptions = program.Descriptions,
                Purpose = program.Purpose,
                InstituteId = program.InstituteId,
                Years = program.Years,
                Credits = program.Credits,
                Results = program.Results,
                LinkFaculties = program.LinkFaculties?.Select(lf => new LinkFacultyDto
                {
                    Name = lf.Name,
                    Link = lf.Link
                }).ToList(),
                ProgramDocument = program.ProgramDocument != null ? new ProgramDocumentDto
                {
                    Id = program.ProgramDocument.Id,
                    FileName = program.ProgramDocument.FileName,
                    ContentType = program.ProgramDocument.ContentType,
                    FileSize = program.ProgramDocument.FileSize ?? 0
                } : null,
                Accredited = program.Accredited
            };
        }

        public IEnumerable<ProgramResponseDto> ToProgramResponseDtos(IEnumerable<ProgramModel> programs)
        {
            return programs.Select(ToProgramResponseDto).Where(dto => dto != null);
        }

        public ProgramModel ToProgramModel(ProgramCreateDto programDto)
        {
            var program = new ProgramModel
            {
                Degree = programDto.Degree,
                Name = programDto.Name,
                NameCode = programDto.NameCode,
                FieldOfStudy = programDto.FieldOfStudy,
                Speciality = programDto.Speciality,
                Form = programDto.Form,
                Objects = programDto.Objects,
                Directions = programDto.Directions,
                Descriptions = programDto.Descriptions,
                Purpose = programDto.Purpose,
                InstituteId = programDto.InstituteId,
                Years = programDto.Years,
                Credits = programDto.Credits,
                Results = programDto.Results,
                Accredited = programDto.Accredited
            };

            if (programDto.LinkFaculties != null)
            {
                program.LinkFaculties = programDto.LinkFaculties.Select(lf => new LinkFaculty
                {
                    Name = lf.Name,
                    Link = lf.Link
                }).ToList();
            }

            if (programDto.ProgramCharacteristics != null)
            {
                program.ProgramCharacteristics = new ProgramCharacteristics
                {
                    Focus = programDto.ProgramCharacteristics.Focus,
                    Features = programDto.ProgramCharacteristics.Features
                };

                if (programDto.ProgramCharacteristics.Area != null)
                {
                    program.ProgramCharacteristics.Area = new Area
                    {
                        Object = programDto.ProgramCharacteristics.Area.Object,
                        Aim = programDto.ProgramCharacteristics.Area.Aim,
                        Theory = programDto.ProgramCharacteristics.Area.Theory,
                        Methods = programDto.ProgramCharacteristics.Area.Methods,
                        Instruments = programDto.ProgramCharacteristics.Area.Instruments
                    };
                }
            }

            if (programDto.ProgramCompetence != null)
            {
                program.ProgramCompetence = new ProgramCompetence
                {
                    IntegralCompetence = programDto.ProgramCompetence.IntegralCompetence,
                    OverallCompetences = programDto.ProgramCompetence.OverallCompetences?.Select(oc => new OverallCompetence
                    {
                        Description = oc.Description
                    }).ToList() ?? new List<OverallCompetence>(),
                    SpecialCompetences = programDto.ProgramCompetence.SpecialCompetences?.Select(sc => new SpecialCompetence
                    {
                        Description = sc.Description
                    }).ToList() ?? new List<SpecialCompetence>()
                };
            }

            if (programDto.ProgramComponents != null)
            {
                program.ProgramComponents = programDto.ProgramComponents.Select(pc => new ProgramComponent
                {
                    ComponentType = pc.ComponentType,
                    ComponentName = pc.ComponentName,
                    ComponentCredits = pc.ComponentCredits,
                    ComponentHours = pc.ComponentHours,
                    ControlForms = pc.ControlForms?.Select(cf => new ControlForm
                    {
                        Type = cf.Type
                    }).ToList() ?? new List<ControlForm>()
                }).ToList();
            }

            if (programDto.Jobs != null)
            {
                program.Jobs = programDto.Jobs.Select(j => new Job
                {
                    Code = j.Code,
                    Title = j.Title
                }).ToList();
            }

            return program;
        }

        public void UpdateProgramModel(ProgramModel program, ProgramUpdateDto programDto)
        {
            program.Degree = programDto.Degree;
            program.Name = programDto.Name;
            program.NameCode = programDto.NameCode;
            program.FieldOfStudy = programDto.FieldOfStudy;
            program.Speciality = programDto.Speciality;
            program.Form = programDto.Form;
            program.Objects = programDto.Objects;
            program.Directions = programDto.Directions;
            program.Descriptions = programDto.Descriptions;
            program.Purpose = programDto.Purpose;
            program.InstituteId = programDto.InstituteId;
            program.Years = programDto.Years;
            program.Credits = programDto.Credits;
            program.Results = programDto.Results;
            program.Accredited = programDto.Accredited;

            if (programDto.LinkFaculties != null)
            {
                program.LinkFaculties.Clear();
                program.LinkFaculties.AddRange(programDto.LinkFaculties.Select(lf => new LinkFaculty
                {
                    Name = lf.Name,
                    Link = lf.Link,
                    ProgramId = program.Id
                }));
            }
        }
    }
    
}
