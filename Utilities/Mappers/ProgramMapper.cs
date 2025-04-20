using OntuPhdApi.Models.Programs.Components;
using OntuPhdApi.Models.Programs;
using System.Xml.Linq;
using OntuPhdApi.Models.Programs.Dto;
using Microsoft.EntityFrameworkCore;
using OntuPhdApi.Data;

namespace OntuPhdApi.Utilities.Mappers
{
    public class ProgramMapper : IProgramMapper
    {

        private readonly AppDbContext context;

        public ProgramMapper(AppDbContext appDbContext)
        {
            context = appDbContext;
        }

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
                Form = model.Form?.Any() == true ? model.Form : null,
                Objects = model.Objects,
                Directions = model.Directions?.Any() == true ? model.Directions : null,
                Descriptions = model.Descriptions,
                Purpose = model.Purpose,
                Institute = model.Institute?.Name,
                Years = model.Years,
                Credits = model.Credits,
                Results = model.Results?.Any() == true ? model.Results : null,
                ProgramDocumentId = model.ProgramDocumentId,
                ProgramDocument = model.ProgramDocument,
                LinkFaculties = model.LinkFaculties?.Any() == true
            ? model.LinkFaculties.Select(MapLinkFaculty).ToList()
            : null,
                ProgramCharacteristics = CleanCharacteristics(MapCharacteristics(model.ProgramCharacteristics)),
                ProgramCompetence = CleanCompetence(MapCompetence(model.ProgramCompetence)),
                ProgramComponents = model.ProgramComponents?.Any() == true
            ? model.ProgramComponents.Select(MapProgramComponent).ToList()
            : null,
                Jobs = model.Jobs?.Any() == true
            ? model.Jobs.Select(MapJob).ToList()
            : null,
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
                }).ToList() ?? null

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
            return comp == null ? null
            : new ProgramCompetenceDto
            {
                IntegralCompetence = comp.IntegralCompetence,
                OverallCompetences = comp.OverallCompetences?.Select(o => new OverallCompetenceDto
                {
                    Description = o.Description
                }).ToList() ?? null,

                SpecialCompetence = comp.SpecialCompetences?.Select(s => new SpecialCompetenceDto
                {
                    Description = s.Description
                }).ToList() ?? null
            };
        }

        private ProgramCharacteristicsDto? CleanCharacteristics(ProgramCharacteristicsDto? dto)
        {
            if (dto == null) return null;

            bool isEmpty =
                string.IsNullOrWhiteSpace(dto.Focus) &&
                string.IsNullOrWhiteSpace(dto.Features) &&
                (dto.Area == null || (
                    string.IsNullOrWhiteSpace(dto.Area.Aim) &&
                    string.IsNullOrWhiteSpace(dto.Area.Object) &&
                    string.IsNullOrWhiteSpace(dto.Area.Instruments) &&
                    string.IsNullOrWhiteSpace(dto.Area.Methods) &&
                    string.IsNullOrWhiteSpace(dto.Area.Theory)
                ));

            return isEmpty ? null : dto;
        }

        private ProgramCompetenceDto? CleanCompetence(ProgramCompetenceDto? dto)
        {
            if (dto == null) return null;

            bool isEmpty =
                string.IsNullOrWhiteSpace(dto.IntegralCompetence) &&
                (dto.OverallCompetences == null || !dto.OverallCompetences.Any(o => !string.IsNullOrWhiteSpace(o.Description))) &&
                (dto.SpecialCompetence == null || !dto.SpecialCompetence.Any(s => !string.IsNullOrWhiteSpace(s.Description)));

            return isEmpty ? null : dto;
        }




        public List<ProgramResponseDto> ToProgramResponseDtos(IEnumerable<ProgramModel> models)
        {
            return models.Select(ToProgramResponseDto).ToList();
        }

        public IEnumerable<ProgramResponseDto> ToProgramResponseDto1s(IEnumerable<ProgramModel> models)
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


        public ProgramModel ToProgramModel(ProgramCreateUpdateDto programDto)
        {
            var program = new ProgramModel
            {
                Degree = programDto.Degree,
                Name = programDto.Name,
                NameCode = programDto.NameCode,
                Form = programDto.Form,
                Objects = programDto.Objects,
                Directions = programDto.Directions,
                Descriptions = programDto.Descriptions,
                Purpose = programDto.Purpose,
                Years = programDto.Years,
                Credits = programDto.Credits,
                Results = programDto.Results,
                Accredited = programDto.Accredited
            };

            if (programDto.FieldOfStudy != null && programDto.Speciality != null)
            {
                program.FieldOfStudy = new FieldOfStudy
                {
                    Code = programDto.FieldOfStudy.Code,
                    Name = programDto.FieldOfStudy.Name,
                    Degree = programDto.Degree
                };

                program.Speciality = new Speciality
                {
                    Code = programDto.Speciality.Code,
                    Name = programDto.Speciality.Name,
                    FieldCode = programDto.FieldOfStudy.Code
                };

            }


            if (programDto.Institute != null)
            {
                program.Institute = new Models.Programs.Components.Institute
                {
                    Name = programDto.Institute.Name
                };
            }

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

        public void UpdateProgramModel(ProgramModel program, ProgramCreateUpdateDto dto)
        {
            program.Name = dto.Name;
            program.Degree = dto.Degree;
            program.NameCode = dto.NameCode;
            program.Form = dto.Form;
            program.Objects = dto.Objects;
            program.Directions = dto.Directions;
            program.Descriptions = dto.Descriptions;
            program.Purpose = dto.Purpose;
            program.Years = dto.Years;
            program.Credits = dto.Credits;
            program.Results = dto.Results;
            program.Accredited = dto.Accredited;





            if (dto.Institute != null)
            {
                if (program.Institute == null)
                    program.Institute = new Institute();

                program.Institute = dto.Institute;
            }
            else
            {
                program.Institute = null;
            }

            if (dto.FieldOfStudy != null && dto.Speciality != null)
            {
                program.FieldOfStudy.Name = dto.FieldOfStudy.Name;
                program.FieldOfStudy.Code = dto.FieldOfStudy.Code;
                program.FieldOfStudy.Degree = program.Degree;


                program.Speciality.Name = dto.Speciality.Name;
                program.Speciality.Code = dto.Speciality.Code;
                program.Speciality.FieldCode = dto.FieldOfStudy.Code;

            }


            // --- LinkFaculties ---
            context.LinkFaculties.RemoveRange(context.LinkFaculties.Where(x => x.ProgramId == program.Id));
            if (dto.LinkFaculties != null)
            {
                program.LinkFaculties = dto.LinkFaculties.Select(x => new LinkFaculty
                {
                    Name = x.Name,
                    Link = x.Link,
                    ProgramId = program.Id
                }).ToList();
            }

            // --- ProgramCharacteristics & Area ---
            var oldCharacteristics = context.ProgramCharacteristics.FirstOrDefault(x => x.ProgramId == program.Id);
            if (oldCharacteristics != null)
            {
                var area = context.Areas.FirstOrDefault(a => a.ProgramCharacteristicsId == oldCharacteristics.Id);
                if (area != null) context.Areas.Remove(area);
                context.ProgramCharacteristics.Remove(oldCharacteristics);
            }

            if (dto.ProgramCharacteristics != null)
            {
                var newChar = new ProgramCharacteristics
                {
                    Focus = dto.ProgramCharacteristics.Focus,
                    Features = dto.ProgramCharacteristics.Features,
                    ProgramId = program.Id,
                };

                if (dto.ProgramCharacteristics.Area != null)
                {
                    newChar.Area = new Area
                    {
                        Aim = dto.ProgramCharacteristics.Area.Aim,
                        Object = dto.ProgramCharacteristics.Area.Object,
                        Theory = dto.ProgramCharacteristics.Area.Theory,
                        Methods = dto.ProgramCharacteristics.Area.Methods,
                        Instruments = dto.ProgramCharacteristics.Area.Instruments,
                    };
                }

                program.ProgramCharacteristics = newChar;
            }

            // ========== ProgramCompetence + SpecialCompetences + OverallCompetences ==========
            var oldCompetence = context.ProgramCompetences.FirstOrDefault(x => x.ProgramId == program.Id);
            if (oldCompetence != null)
            {
                var spec = context.SpecialCompetences.Where(x => x.ProgramCompetenceId == oldCompetence.Id);
                var over = context.OverallCompetences.Where(x => x.ProgramCompetenceId == oldCompetence.Id);
                context.SpecialCompetences.RemoveRange(spec);
                context.OverallCompetences.RemoveRange(over);
                context.ProgramCompetences.Remove(oldCompetence);
            }

            if (dto.ProgramCompetence != null)
            {
                var newComp = new ProgramCompetence
                {
                    IntegralCompetence = dto.ProgramCompetence.IntegralCompetence,
                    ProgramId = program.Id,
                    SpecialCompetences = dto.ProgramCompetence.SpecialCompetences?.Select(x => new SpecialCompetence
                    {
                        Description = x.Description
                    }).ToList(),
                    OverallCompetences = dto.ProgramCompetence.OverallCompetences?.Select(x => new OverallCompetence
                    {
                        Description = x.Description
                    }).ToList()
                };

                program.ProgramCompetence = newComp;
            }

            // --- ProgramComponents + ControlForms ---
            var oldComponents = context.ProgramComponents.Where(x => x.ProgramId == program.Id).ToList();
            context.ProgramComponents.RemoveRange(oldComponents);

            if (dto.ProgramComponents != null)
            {
                program.ProgramComponents = dto.ProgramComponents.Select(c => new ProgramComponent
                {
                    ComponentName = c.ComponentName,
                    ComponentType = c.ComponentType,
                    ComponentHours = c.ComponentHours,
                    ComponentCredits = c.ComponentCredits,
                    ProgramId = program.Id,
                    ControlForms = c.ControlForms?.Select(cf => new ControlForm
                    {
                        Type = cf.Type,
                    }).ToList()
                }).ToList();
            }

            // ========== Jobs ==========
            context.Jobs.RemoveRange(context.Jobs.Where(x => x.ProgramId == program.Id));
            if (dto.Jobs != null)
            {
                program.Jobs = dto.Jobs.Select(j => new Job
                {
                    Title = j.Title,
                    Code = j.Code,
                    ProgramId = program.Id
                }).ToList();
            }


        }




    }
    
}
