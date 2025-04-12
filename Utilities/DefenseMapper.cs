using OntuPhdApi.Models.Defense;
using OntuPhdApi.Models.Programs;

namespace OntuPhdApi.Utilities
{
    public class DefenseMapper
    {
        public static DefenseDto ToDto(DefenseModel entity)
        {
            if (entity == null) return null;

            return new DefenseDto
            {
                Id = entity.Id,
                CandidateNameSurname = entity.CandidateNameSurname,
                DefenseTitle = entity.DefenseTitle,
                ScienceTeachers = entity.ScienceTeachers,
                DefenseDate = entity.DefenseDate,
                Address = entity.Address,
                Message = entity.Message,
                Placeholder = entity.Placeholder,
                Members = entity.Members?.Select(m => new CompositionOfRadaDto
                {
                    Position = m.Position,
                    Members = m.Members?.Select(mr => new MemberOfRadaDto
                    {
                        NameSurname = mr.NameSurname,
                        ToolTip = mr.ToolTip
                    }).ToList()
                }).ToList(),
                Files = entity.Files?.Select(f => new DefenseFileDto
                {
                    Name = f.Name,
                    Link = f.Link,
                    Type = f.Type
                }).ToList(),
                PublicationDate = entity.PublicationDate,
                ProgramInfo = new ProgramDefenseDto
                {
                    Id = entity.Program.Id,
                    Name = entity.Program.Name,
                    Degree = entity.Program.Degree,
                    FieldOfStudy = entity.Program.FieldOfStudy,
                    Speciality = new ShortSpeciality {
                        Code = entity.Program.Speciality.Code,
                        Name = entity.Program.Speciality.Name
                    }
                }
            };
        }

        public static List<DefenseDto> ToDtoList(List<DefenseModel> entities)
        {
            return entities?.Select(ToDto).Where(dto => dto != null).ToList() ?? new List<DefenseDto>();
        }
    }
}
