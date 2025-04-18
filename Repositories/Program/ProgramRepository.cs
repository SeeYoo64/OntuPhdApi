using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;
using OntuPhdApi.Controllers;
using OntuPhdApi.Data;
using OntuPhdApi.Models.Institutes;
using OntuPhdApi.Models.Programs;
using OntuPhdApi.Repositories.Institutes;
using OntuPhdApi.Utilities;

namespace OntuPhdApi.Repositories.Program
{
    public class ProgramRepository : IProgramRepository
    {
        private readonly AppDbContext _context;
        private readonly IInstituteRepository _instituteRepository;
        private readonly ILogger<ProgramRepository> _logger;

        public ProgramRepository(
            AppDbContext context,
            IInstituteRepository instituteRepository,
            ILogger<ProgramRepository> logger)
        {
            _context = context;
            _instituteRepository = instituteRepository;
            _logger = logger;
        }

        public async Task<Institute> GetOrCreateInstituteAsync(string instituteName)
        {
            _logger.LogInformation("Fetching or creating institute with name {InstituteName}.", instituteName);
            var institute = await _context.Institutes
                .FirstOrDefaultAsync(i => i.Name == instituteName);

            if (institute == null)
            {
                institute = new Institute { Name = instituteName };
                institute.Id = await _instituteRepository.InsertInstituteAsync(institute);
                _logger.LogInformation("Created new institute with name {InstituteName} and ID {InstituteId}.", instituteName, institute.Id);
            }

            return institute;
        }

        public async Task<IEnumerable<ProgramModel>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all programs from database.");
            return await _context.Programs
                .Include(p => p.ProgramDocument)
                .Include(p => p.Institute)
                .Include(p => p.LinkFaculties)
                .Include(p => p.ProgramCharacteristics)
                    .ThenInclude(c => c.Area)
                .Include(p => p.ProgramCompetence)
                .Include(p => p.ProgramComponents)
                .Include(p => p.Jobs)
                .OrderBy(p => p.Degree == "phd" ? 0 : 1)
                .ThenBy(p => p.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProgramModel>> GetByDegreeAsync(string degree)
        {
            _logger.LogInformation("Fetching programs for degree {Degree}.", degree?.ToString() ?? "all");
            return await _context.Programs
                .Include(p => p.Institute)
                .Include(p => p.FieldOfStudy)
                .Include(p => p.Speciality)
                .Where(p => p.Degree == degree)
                .ToListAsync();
        }

        public async Task<ProgramModel> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching program with ID {ProgramId}.", id);
            return await _context.Programs
                .Include(p => p.ProgramDocument)
                .Include(p => p.Institute)
                .Include(p => p.LinkFaculties)
                .Include(p => p.ProgramCharacteristics)
                    .ThenInclude(c => c.Area)
                .Include(p => p.ProgramCompetence)
                .Include(p => p.ProgramComponents)
                .Include(p => p.Jobs)
                .FirstOrDefaultAsync(p => p.Id == id);

        }

        public async Task AddAsync(ProgramModel program)
        {
            await _context.Programs.AddAsync(program);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProgramModel program)
        {
            _context.Programs.Update(program);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var program = await _context.Programs.FindAsync(id);
            if (program != null)
            {
                _context.Programs.Remove(program);
                await _context.SaveChangesAsync();
            }
        }





    }
}