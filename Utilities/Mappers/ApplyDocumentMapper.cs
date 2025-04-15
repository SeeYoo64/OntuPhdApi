using OntuPhdApi.Models.ApplyDocuments;

namespace OntuPhdApi.Utilities.Mappers
{
    public class ApplyDocumentMapper
    {
        public static ApplyDocumentDto ToDto(ApplyDocumentModel entity)
        {
            if (entity == null) return null;

            return new ApplyDocumentDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Requirements = entity.Requirements?.Select(r => new RequirementDto
                {
                    Title = r.Title,
                    Description = r.Description
                }).ToList(),
                OriginalsRequired = entity.OriginalsRequired?.Select(r => new RequirementDto
                {
                    Title = r.Title,
                    Description = r.Description
                }).ToList()
            };
        }

        public static List<ApplyDocumentDto> ToDtoList(List<ApplyDocumentModel> entities)
        {
            return entities?.Select(ToDto).Where(dto => dto != null).ToList() ?? new List<ApplyDocumentDto>();
        }

        public static ApplyDocumentModel ToEntity(ApplyDocumentCreateUpdateDto dto)
        {
            if (dto == null) return null;

            return new ApplyDocumentModel
            {
                Name = dto.Name,
                Description = dto.Description,
                Requirements = dto.Requirements?.Select(r => new Requirements
                {
                    Title = r.Title,
                    Description = r.Description
                }).ToList(),
                OriginalsRequired = dto.OriginalsRequired?.Select(r => new Requirements
                {
                    Title = r.Title,
                    Description = r.Description
                }).ToList()
            };
        }

        public static void UpdateEntity(ApplyDocumentModel entity, ApplyDocumentCreateUpdateDto dto)
        {
            if (entity == null || dto == null) return;

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.Requirements = dto.Requirements?.Select(r => new Requirements
            {
                Title = r.Title,
                Description = r.Description
            }).ToList();
            entity.OriginalsRequired = dto.OriginalsRequired?.Select(r => new Requirements
            {
                Title = r.Title,
                Description = r.Description
            }).ToList();
        }

    }
}
