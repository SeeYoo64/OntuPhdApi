using OntuPhdApi.Data;
using OntuPhdApi.Models.Defense;
using OntuPhdApi.Models.Programs;
using OntuPhdApi.Models.Programs.Components;

namespace OntuPhdApi.Utilities.Mappers
{
    public class DefenseMapper
    {
        public static DefenseDto ToDto(DefenseModel entity, AppDbContext context)
        {
            if (entity == null) return null;

            var fieldOfStudy = context.FieldOfStudies
                .FirstOrDefault(f => f.Id == entity.Program.FieldOfStudyId);
            var speciality = context.Specialities
                .FirstOrDefault(s => s.Id == entity.Program.SpecialityId);

            return new DefenseDto
            {
                Id = entity.Id,
                CandidateNameSurname = entity.CandidateNameSurname,
                DefenseTitle = entity.DefenseTitle,
                ScienceTeachers = entity.ScienceTeachers,
                CandidateDegree = entity.Program.Degree,
                DefenseDate = entity.DefenseDate.ToString("yyyy-MM-ddTHH:mm"),
                Address = entity.Address,
                Message = entity.Message,
                Placeholder = entity.Placeholder,
                Members = entity.Members?.Select(m => new CompositionOfRadaDto
                {
                    Position = m.Position,
                    Members = m.Members?.Select(member => new MemberOfRadaDto
                    {
                        NameSurname = member.NameSurname,
                        ToolTip = member.ToolTip,
                        Title = member.Title
                    }).ToList()
                }).ToList(),
                Files = entity.Files?.Select(f => new DefenseFileDto
                {
                    Name = f.Name,
                    Link = f.Link,
                    Type = f.Type
                }).ToList(),
                PublicationDate = entity.PublicationDate.ToString("yyyy-MM-ddTHH:mm"),
                Program = entity.Program != null ? new ProgramDefenseDto
                {
                    Id = entity.Program.Id,
                    Name = entity.Program.Name,
                    FieldOfStudy = entity.Program.FieldOfStudy != null ? new FieldOfStudyDto
                    {
                        Code = entity.Program.FieldOfStudy.Code,
                        Name = entity.Program.FieldOfStudy.Name
                    } : null,
                    Speciality = entity.Program.Speciality != null ? new SpecialityDto
                    {
                        Code = entity.Program.Speciality.Code,
                        Name = entity.Program.Speciality.Name
                    } : null
                } : null
            };
        }

        public static List<DefenseDto> ToDtoList(List<DefenseModel> entities, AppDbContext context)
        {
            return entities?.Select(entity => ToDto(entity, context))
                   .Where(dto => dto != null)
                   .ToList() ?? new List<DefenseDto>();
        }

        public static DefenseModel ToEntity(DefenseCreateDto dto)
        {
            if (dto == null) return null;

            return new DefenseModel
            {
                CandidateNameSurname = dto.CandidateNameSurname,
                DefenseTitle = dto.DefenseTitle,
                ScienceTeachers = dto.ScienceTeachers,
                DefenseDate = dto.DefenseDate,
                Address = dto.Address,
                Message = dto.Message,
                Placeholder = dto.Placeholder,
                Members = dto.Members?.Select(m => new CompositionOfRada
                {
                    Position = m.Position,
                    Members = m.Members?.Select(member => new MemberOfRada
                    {
                        NameSurname = member.NameSurname,
                        ToolTip = member.ToolTip,
                        Title = member.Title
                    }).ToList()
                }).ToList(),
                Files = dto.Files?.Select(f => new DefenseFile
                {
                    Name = f.Name,
                    Link = f.Link,
                    Type = f.Type
                }).ToList(),
                PublicationDate = dto.PublicationDate,
                ProgramId = dto.ProgramId
            };
        }
    }
}
