using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;
using OntuPhdApi.Controllers;
using OntuPhdApi.Data;
using OntuPhdApi.Models.Programs;
using OntuPhdApi.Utilities;

namespace OntuPhdApi.Repositories
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
        public async Task<List<ProgramModel>> GetAllProgramsAsync()
        {
            _logger.LogInformation("Fetching all programs from database.");
            return await _context.Programs
                .Include(p => p.ProgramDocument)
                .OrderBy(p => p.Degree == "phd" ? 0 : 1)
                .ThenBy(p => p.Id)
                .ToListAsync();
        }

        public Task<List<ProgramComponent>> GetProgramComponentsAsync(int programId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Job>> GetProgramJobsAsync(int programId)
        {
            throw new NotImplementedException();
        }
        public async Task<ProgramModel> GetProgramByIdAsync(int id)
        {
            _logger.LogInformation("Fetching program with ID {ProgramId}.", id);
            var program = await _context.Programs
                .Include(p => p.ProgramDocument)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (program == null)
            {
                _logger.LogWarning("Program with ID {ProgramId} not found.", id);
            }
            return program;
        }

        public async Task<int> InsertProgramAsync(ProgramModel program)
        {
            _logger.LogInformation("Inserting program {ProgramName}.", program.Name);
            _context.Programs.Add(program);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Program {ProgramName} inserted with ID {ProgramId}.", program.Name, program.Id);
            return program.Id;
        }

        public async Task UpdateProgramAsync(ProgramModel program)
        {
            _logger.LogInformation("Updating program {ProgramName} with ID {ProgramId}.", program.Name, program.Id);
            var existingProgram = await _context.Programs.FindAsync(program.Id);
            if (existingProgram == null)
            {
                _logger.LogWarning("Program with ID {ProgramId} not found for update.", program.Id);
                throw new Exception($"Program with ID {program.Id} not found.");
            }

            _context.Entry(existingProgram).CurrentValues.SetValues(program);
            existingProgram.ProgramDocument = program.ProgramDocument; // Обновляем навигационное свойство вручную
            await _context.SaveChangesAsync();
            _logger.LogInformation("Program {ProgramName} with ID {ProgramId} updated.", program.Name, program.Id);
        }

        public async Task DeleteProgramAsync(int id)
        {
            _logger.LogInformation("Deleting program with ID {ProgramId}.", id);
            var program = await _context.Programs.FindAsync(id);
            if (program == null)
            {
                _logger.LogWarning("Program with ID {ProgramId} not found for deletion.", id);
                throw new Exception($"Program with ID {id} not found.");
            }

            _context.Programs.Remove(program);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Program with ID {ProgramId} deleted.", id);
        }

        public async Task<List<ProgramsDegreeDto>> GetProgramsByDegreeAsync(DegreeType? degree)
        {
            _logger.LogInformation("Fetching programs for degree {Degree}.", degree?.ToString() ?? "all");
            var query = _context.Programs.AsQueryable();
            if (degree.HasValue)
            {
                query = query.Where(p => p.Degree == degree.Value.ToString());
            }

            var result = await query
                .Select(p => new ProgramsDegreeDto
                {
                    Id = p.Id,
                    Degree = p.Degree,
                    Name = p.Name,
                    FieldOfStudy = p.FieldOfStudy,
                    Speciality = new ShortSpeciality
                    {
                        Code = p.Speciality.Code,
                        Name = p.Speciality.Name
                    }
                })
                .ToListAsync();

            _logger.LogInformation("Retrieved {ProgramCount} programs for degree {Degree}.", result.Count, degree?.ToString() ?? "all");
            return result;
        }



    }
}