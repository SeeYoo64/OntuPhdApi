using OntuPhdApi.Models.Roadmap;
using OntuPhdApi.Repositories.Roadmap;

namespace OntuPhdApi.Services.Roadmap
{
    public class RoadmapService : IRoadmapService
    {
        private readonly IRoadmapRepository _repo;

        public RoadmapService(IRoadmapRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<RoadmapModelDto>> GetAllAsync(string? type = null)
        {
            var roadmaps = string.IsNullOrEmpty(type)
                ? await _repo.GetAllAsync()
                : await _repo.GetByTypeAsync(type);

            return roadmaps
                .Select(MapToDto)
                .OrderBy(r => r.Status switch
                {
                    RoadmapStatus.Completed => 1,
                    RoadmapStatus.Ontime => 2,
                    RoadmapStatus.NotStarted => 3,
                    _ => 4
                })
                .ThenBy(r => r.DataStart)
                .ToList();
        }

        public async Task<RoadmapModelDto?> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity is null ? null : MapToDto(entity);
        }

        public async Task<RoadmapModelDto> AddAsync(RoadmapModelDto dto)
        {
            var entity = new RoadmapModel
            {
                Type = dto.Type,
                DataStart = dto.DataStart,
                DataEnd = dto.DataEnd,
                AdditionalTime = dto.AdditionalTime,
                Description = dto.Description
            };

            entity.DataStart = DateTime.SpecifyKind(entity.DataStart, DateTimeKind.Utc);

            if (entity.DataEnd.HasValue)
                entity.DataEnd = DateTime.SpecifyKind(entity.DataEnd.Value, DateTimeKind.Utc);


            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();

            return MapToDto(entity);
        }

        private static RoadmapModelDto MapToDto(RoadmapModel roadmap) =>
            new()
            {
                Id = roadmap.Id,
                Type = roadmap.Type,
                DataStart = roadmap.DataStart,
                DataEnd = roadmap.DataEnd,
                AdditionalTime = roadmap.AdditionalTime,
                Description = roadmap.Description,
                Status = roadmap.Status
            };


    }
}
