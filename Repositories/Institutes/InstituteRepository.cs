using Microsoft.EntityFrameworkCore;
using OntuPhdApi.Data;
using OntuPhdApi.Models.Institutes;

namespace OntuPhdApi.Repositories.Institutes
{
    public class InstituteRepository : IInstituteRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<InstituteRepository> _logger;

        public InstituteRepository(AppDbContext context, ILogger<InstituteRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Institute>> GetAllInstitutesAsync()
        {
            _logger.LogInformation("Fetching all institutes from database.");
            return await _context.Institutes
                .OrderBy(i => i.Id)
                .ToListAsync();
        }

        public async Task<Institute> GetInstituteByIdAsync(int id)
        {
            _logger.LogInformation("Fetching institute with ID {InstituteId}.", id);
            var institute = await _context.Institutes
                .FirstOrDefaultAsync(i => i.Id == id);
            if (institute == null)
            {
                _logger.LogWarning("Institute with ID {InstituteId} not found.", id);
            }
            return institute;
        }

        public async Task<int> InsertInstituteAsync(Institute institute)
        {
            _logger.LogInformation("Inserting institute {InstituteName}.", institute.Name);
            _context.Institutes.Add(institute);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Institute {InstituteName} inserted with ID {InstituteId}.", institute.Name, institute.Id);
            return institute.Id;
        }

        public async Task UpdateInstituteAsync(Institute institute)
        {
            _logger.LogInformation("Updating institute {InstituteName} with ID {InstituteId}.", institute.Name, institute.Id);
            var existingInstitute = await _context.Institutes.FindAsync(institute.Id);
            if (existingInstitute == null)
            {
                _logger.LogWarning("Institute with ID {InstituteId} not found for update.", institute.Id);
                throw new Exception($"Institute with ID {institute.Id} not found.");
            }

            _context.Entry(existingInstitute).CurrentValues.SetValues(institute);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Institute {InstituteName} with ID {InstituteId} updated.", institute.Name, institute.Id);
        }

        public async Task DeleteInstituteAsync(int id)
        {
            _logger.LogInformation("Deleting institute with ID {InstituteId}.", id);
            var institute = await _context.Institutes.FindAsync(id);
            if (institute == null)
            {
                _logger.LogWarning("Institute with ID {InstituteId} not found for deletion.", id);
                throw new Exception($"Institute with ID {id} not found.");
            }

            _context.Institutes.Remove(institute);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Institute with ID {InstituteId} deleted.", id);
        }
    }
}
