using OntuPhdApi.Models.Documents;
using Microsoft.EntityFrameworkCore;

namespace OntuPhdApi.Utilities.Mappers
{
    public class DocumentMapper
    {
        public static DocumentDto ToDto(DocumentModel entity)
        {
            if (entity == null) return null;

            return new DocumentDto
            {
                Id = entity.Id,
                ProgramId = entity.ProgramId,
                Name = entity.Name,
                Type = entity.Type,
                Link = entity.Link
            };
        }

        public static List<DocumentDto> ToDtoList(List<DocumentModel> entities)
        {
            return entities?.Select(ToDto).Where(dto => dto != null).ToList() ?? new List<DocumentDto>();
        }

        public static DocumentModel ToEntity(DocumentCreateUpdateDto dto)
        {
            if (dto == null) return null;

            return new DocumentModel
            {
                ProgramId = dto.ProgramId,
                Name = dto.Name,
                Type = dto.Type,
                Link = dto.Link
            };
        }

        public static void UpdateEntity(DocumentModel entity, DocumentCreateUpdateDto dto)
        {
            if (entity == null || dto == null) return;

            entity.ProgramId = dto.ProgramId;
            entity.Name = dto.Name;
            entity.Type = dto.Type;
            entity.Link = dto.Link;
        }

    }
}
