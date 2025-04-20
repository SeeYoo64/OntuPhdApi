using OntuPhdApi.Models.Roadmap;

namespace OntuPhdApi.Services.Roadmap
{
    public interface IRoadmapService
    {
        Task<List<RoadmapModelDto>> GetAllAsync(string? type = null);
        Task<RoadmapModelDto?> GetByIdAsync(int id);
        Task<RoadmapModelDto> AddAsync(RoadmapModelDto dto);
    }
}
