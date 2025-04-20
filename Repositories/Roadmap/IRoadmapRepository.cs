using OntuPhdApi.Models.Roadmap;
namespace OntuPhdApi.Repositories.Roadmap


{
    public interface IRoadmapRepository
    {
        Task<List<RoadmapModel>> GetAllAsync();
        Task<List<RoadmapModel>> GetByTypeAsync(string type);
        Task<RoadmapModel?> GetByIdAsync(int id);
        Task AddAsync(RoadmapModel roadmap);
        Task SaveChangesAsync();
    }
}
