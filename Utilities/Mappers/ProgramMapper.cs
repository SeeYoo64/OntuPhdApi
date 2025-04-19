using OntuPhdApi.Models.Programs.Components;
using OntuPhdApi.Models.Programs;
using System.Xml.Linq;

namespace OntuPhdApi.Utilities.Mappers
{
    public class ProgramMapper : IProgramMapper
    {
        public ProgramResponseDto ToProgramResponseDto(ProgramModel model)
        {
            if (model == null) return null;

            return new ProgramResponseDto
            {
                Id = model.Id,
                Degree = model.Degree,
                Name = model.Name,
                NameCode = model.NameCode,
                FieldOfStudy = MapFieldOfStudy(model.FieldOfStudy),
                Speciality = MapSpeciality(model.Speciality),
                Form = model.Form ?? new List<string>(),
                Objects = model.Objects,
                Directions = model.Directions ?? new List<string>(),
                Descriptions = model.Descriptions,
                Purpose = model.Purpose,
                Institute = model.Institute.Name,
                Years = model.Years,
                Credits = model.Credits,
                Results = model.Results ?? new List<string>(),
                ProgramDocumentId = model.ProgramDocumentId,
                ProgramDocument = model.ProgramDocument,
                LinkFaculties = model.LinkFaculties?.Select(MapLinkFaculty).ToList() ?? new List<LinkFacultyDto>(),
                ProgramCharacteristics = MapCharacteristics(model.ProgramCharacteristics),
                ProgramCompetence = MapCompetence(model.ProgramCompetence),
                ProgramComponents = model.ProgramComponents?.Select(MapProgramComponent).ToList() ?? new List<ProgramComponentDto>(),
                Jobs = model.Jobs?.Select(MapJob).ToList() ?? new List<JobDto>(),
                Accredited = model.Accredited
            };
        }

        private JobDto MapJob(Job? j)
        {
            return j == null ? null : new JobDto
            {
                Code = j.Code,
                Title = j.Title
            };
        }

        private ProgramComponentDto MapProgramComponent(ProgramComponent? pc)
        {
            if (pc == null) return new ProgramComponentDto();
            return new ProgramComponentDto
            {

                ComponentName = pc.ComponentName,
                ComponentType = pc.ComponentType,
                ComponentCredits = pc.ComponentCredits,
                ComponentHours = pc.ComponentHours,

                ControlForms = pc.ControlForms?.Select(o => new ControlFormDto
                {
                    Type = o.Type
                }).ToList() ?? new List<ControlFormDto>()

            };
        }

        private FieldOfStudyDto MapFieldOfStudy(FieldOfStudy? fos)
        {
            return fos == null ? null : new FieldOfStudyDto
            {
                Code = fos.Code,
                Name = fos.Name
            };
        }

        private SpecialityDto MapSpeciality(Speciality? spec)
        {
            return spec == null ? null : new SpecialityDto
            {
                Code = spec.Code,
                Name = spec.Name
            };
        }

        private LinkFacultyDto MapLinkFaculty(LinkFaculty lf)
        {
            return new LinkFacultyDto
            {
                Name = lf.Name,
                Link = lf.Link
            };
        }

        private ProgramCharacteristicsDto MapCharacteristics(ProgramCharacteristics? pc)
        {
            if (pc == null) return new ProgramCharacteristicsDto();
            return new ProgramCharacteristicsDto
            {
                Focus = pc.Focus,
                Features = pc.Features,
                Area = new AreaDto
                {
                    Aim = pc.Area.Aim,
                    Object = pc.Area.Object,
                    Instruments = pc.Area.Instruments,
                    Methods = pc.Area.Methods,
                    Theory = pc.Area.Theory
                }                
                
            };
        }

        private ProgramCompetenceDto MapCompetence(ProgramCompetence? comp)
        {
            return comp == null ? new ProgramCompetenceDto()
            : new ProgramCompetenceDto
            {
                IntegralCompetence = comp.IntegralCompetence,
                OverallCompetences = comp.OverallCompetences?.Select(o => new OverallCompetenceDto
                {
                    Description = o.Description
                }).ToList() ?? new List<OverallCompetenceDto>(),

                SpecialCompetence = comp.SpecialCompetences?.Select(s => new SpecialCompetenceDto
                {
                    Description = s.Description
                }).ToList() ?? new List<SpecialCompetenceDto>()
            };
        }

        public List<ProgramResponseDto> ToProgramResponseDtos(IEnumerable<ProgramModel> models)
        {
            return models.Select(ToProgramResponseDto).ToList();
        }

        public ProgramDegreeDto ToProgramDegree(ProgramModel program)
        {
            return new ProgramDegreeDto
            {
                Id = program.Id,
                Degree = program.Degree,
                Name = program.Name,
                FieldOfStudy = MapFieldOfStudy(program.FieldOfStudy),
                Speciality = MapSpeciality(program.Speciality),
            };
        }

        public List<ProgramDegreeDto> ToProgramDegrees(IEnumerable<ProgramModel> models)
        {
            return models.Select(ToProgramDegree).ToList();
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
