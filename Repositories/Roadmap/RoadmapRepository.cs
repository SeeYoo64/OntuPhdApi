using Microsoft.EntityFrameworkCore;
using OntuPhdApi.Data;
using OntuPhdApi.Models.Roadmap;

namespace OntuPhdApi.Repositories.Roadmap
{
    public class RoadmapRepository : IRoadmapRepository
    {
        private readonly AppDbContext _context;

        public RoadmapRepository(AppDbContext context)
        {
            _context = context;
        }


        public Task<List<RoadmapModel>> GetAllAsync() =>
        _context.Roadmaps.ToListAsync();

        public Task<List<RoadmapModel>> GetByTypeAsync(string type) =>
            _context.Roadmaps.Where(r => r.Type == type).ToListAsync();

        public Task<RoadmapModel?> GetByIdAsync(int id) =>
            _context.Roadmaps.FindAsync(id).AsTask();

        public async Task AddAsync(RoadmapModel roadmap)
        {
            await _context.Roadmaps.AddAsync(roadmap);
        }

        public Task SaveChangesAsync() =>
            _context.SaveChangesAsync();


    }
}
