using OntuPhdApi.Models.Employees;

namespace OntuPhdApi.Utilities.Mappers
{
    public interface EmployeeMapper
    {
        public static EmployeeModelDto ToDto(EmployeeModel entity)
        {
            if (entity == null) return null;

            return new EmployeeModelDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Position = entity.Position,
                PhotoPath = entity.PhotoPath
            };
        }

        public static List<EmployeeModelDto> ToDtoList(List<EmployeeModel> entities)
        {
            return entities?.Select(ToDto).Where(dto => dto != null).ToList() ?? new List<EmployeeModelDto>();
        }

        public static EmployeeModel ToEntity(EmployeeCreateUpdateDto dto)
        {
            if (dto == null) return null;

            return new EmployeeModel
            {
                Name = dto.Name,
                Position = dto.Position
                // PhotoPath будет установлен в сервисе после сохранения файла
            };
        }

        public static void UpdateEntity(EmployeeModel entity, EmployeeCreateUpdateDto dto)
        {
            if (entity == null || dto == null) return;

            entity.Name = dto.Name;
            entity.Position = dto.Position;
            // PhotoPath будет обновлен в сервисе после сохранения файла
        }
    }
}
