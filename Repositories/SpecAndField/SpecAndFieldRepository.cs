using Microsoft.EntityFrameworkCore;
using OntuPhdApi.Data;
using OntuPhdApi.Models.Programs.Components;

namespace OntuPhdApi.Repositories.SpecAndField
{
    public class SpecAndFieldRepository : ISpecAndFieldRepository
    {
        private readonly AppDbContext _context;

        public SpecAndFieldRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<List<FieldOfStudy>> GetFieldsWithSpecialitiesAsync(string? degree)
        {
            var query = _context.FieldOfStudies.AsQueryable();

            if (!string.IsNullOrEmpty(degree))
            {
                query = query.Where(f => f.Degree == degree);
            }

            return await query.ToListAsync();
        }

        public async Task<List<Speciality>> GetSpecialitiesByCodeAsync(string code)
        {
            return await _context.Specialities
                .Where(s => s.FieldCode == code)
                .ToListAsync();
        }


    }
}
