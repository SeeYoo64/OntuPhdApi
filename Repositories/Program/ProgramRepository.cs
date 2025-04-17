using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;
using OntuPhdApi.Controllers;
using OntuPhdApi.Data;
using OntuPhdApi.Models.Programs;
using OntuPhdApi.Utilities;

namespace OntuPhdApi.Repositories.Program
{
    public class ProgramRepository : IProgramRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProgramRepository> _logger;

        public ProgramRepository(AppDbContext context, ILogger<ProgramRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IEnumerable<ProgramModel>> GetAllProgramsAsync()
        {
            _logger.LogInformation("Fetching all programs from database.");
            return await _context.Programs
                //.Include(p => p.Components)
                //.Include(p => p.Jobs)
                .OrderBy(p => p.Degree == "phd" ? 0 : 1)
                .ThenBy(p => p.Id)
                .ToListAsync();
        }

        public async Task<ProgramModel> GetProgramByIdAsync(int id)
        {
            _logger.LogInformation("Fetching program with ID {ProgramId}.", id);
            var program = await _context.Programs
                //.Include(p => p.Components)
                //.Include(p => p.Jobs)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (program == null)
            {
                _logger.LogWarning("Program with ID {ProgramId} not found.", id);
            }
            return program;
        }

        public async Task AddProgramAsync(ProgramModel program)
        {
            _logger.LogInformation("Inserting program {ProgramName}.", program.Name);

            _context.Programs.Add(program);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Program {ProgramName} inserted with ID {ProgramId}.", program.Name, program.Id);

        }

        public async Task UpdateProgramAsync(ProgramModel program)
        {
            _logger.LogInformation("Updating program {ProgramName}.", program.Name, program.Id);
            _context.Programs.Update(program);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Program {ProgramName} updated.", program.Name, program.Id);
        }

        public async Task DeleteProgramAsync(int id)
        {
            _logger.LogInformation("Deleting program with ID {ProgramId}.", id);

            var program = await _context.Programs.FindAsync(id);
            if (program != null)
            {
                _context.Programs.Remove(program);
                _logger.LogInformation("Program with ID {ProgramId} deleted.", id);
                await _context.SaveChangesAsync();
            }
            else _logger.LogWarning("Program with ID {ProgramId} not found for deletion.", id);
            
        }

        public async Task<IEnumerable<ProgramModel>> GetProgramsByDegreeAsync(string degreeType)
        {
            _logger.LogInformation("Fetching programs for degree {Degree}.", degreeType);
            var query = _context.Programs.AsQueryable();
            query = query.Where(p => p.Degree == degreeType);

            _logger.LogInformation("Retrieved programs for degree {Degree}.", degreeType);

            return await _context.Programs
                //.Include(p => p.Components)
                //.Include(p => p.Jobs)
                .Where(p => p.Degree.ToLower().Contains(degreeType.ToLower()))
                .ToListAsync();


        }



    }
}