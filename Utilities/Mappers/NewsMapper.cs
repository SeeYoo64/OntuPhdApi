using OntuPhdApi.Models.News;

namespace OntuPhdApi.Utilities.Mappers
{
    public class NewsMapper
    {
        public static NewsDto ToDto(NewsModel entity)
        {
            if (entity == null) return null;

            return new NewsDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Summary = entity.Summary,
                MainTag = entity.MainTag,
                OtherTags = entity.OtherTags,
                PublicationDate = entity.PublicationDate,
                ThumbnailPath = entity.ThumbnailPath,
                PhotoPaths = entity.PhotoPaths,
                Body = entity.Body
            };
        }

        public static NewsViewDto ToViewDto(NewsModel entity)
        {
            if (entity == null) return null;

            return new NewsViewDto
            {
                Id = entity.Id,
                Title = entity.Title,
                MainTag = entity.MainTag,
                OtherTags = entity.OtherTags,
                PublicationDate = entity.PublicationDate,
                PhotoPaths = entity.PhotoPaths,
                Body = entity.Body
            };
        }

        public static NewsLatestDto ToLatestDto(NewsModel entity)
        {
            if (entity == null) return null;

            return new NewsLatestDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Summary = entity.Summary,
                MainTag = entity.MainTag,
                PublicationDate = entity.PublicationDate,
                ThumbnailPath = entity.ThumbnailPath
            };
        }

        public static List<NewsDto> ToDtoList(List<NewsModel> entities)
        {
            return entities?.Select(ToDto).Where(dto => dto != null).ToList() ?? new List<NewsDto>();
        }

        public static List<NewsLatestDto> ToLatestDtoList(List<NewsModel> entities)
        {
            return entities?.Select(ToLatestDto).Where(dto => dto != null).ToList() ?? new List<NewsLatestDto>();
        }

        public static NewsModel ToEntity(NewsCreateUpdateDto dto)
        {
            if (dto == null) return null;

            return new NewsModel
            {
                Title = dto.Title,
                Summary = dto.Summary,
                MainTag = dto.MainTag,
                OtherTags = dto.OtherTags,
                PublicationDate = dto.PublicationDate,
                Body = dto.Body
                // ThumbnailPath и PhotoPaths будут установлены в сервисе после сохранения файлов
            };
        }

        public static void UpdateEntity(NewsModel entity, NewsCreateUpdateDto dto)
        {
            if (entity == null || dto == null) return;

            entity.Title = dto.Title;
            entity.Summary = dto.Summary;
            entity.MainTag = dto.MainTag;
            entity.OtherTags = dto.OtherTags;
            entity.PublicationDate = dto.PublicationDate;
            entity.Body = dto.Body;
            // ThumbnailPath и PhotoPaths будут обновлены в сервисе после сохранения файлов
        }

    }
}
