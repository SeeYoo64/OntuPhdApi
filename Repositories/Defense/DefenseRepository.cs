﻿using Microsoft.EntityFrameworkCore;
using OntuPhdApi.Data;
using OntuPhdApi.Models.Defense;

namespace OntuPhdApi.Repositories.Defense
{
    public class DefenseRepository : IDefenseRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DefenseRepository> _logger;

        public DefenseRepository(AppDbContext context, ILogger<DefenseRepository> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<List<DefenseModel>> GetAllDefensesAsync()
        {
            _logger.LogInformation("Fetching all defenses from database.");
            try
            {
                return await _context.Defenses
                    .Include(d => d.Program)
                    .OrderBy(d => d.DefenseDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all defenses.");
                throw;
            }
        }


        public async Task<DefenseModel> GetDefenseByIdAsync(int id)
        {
            _logger.LogInformation("Fetching defense with ID {DefenseId} from database.", id);
            try
            {
                return await _context.Defenses
                    .Include(d => d.Program)
                    .FirstOrDefaultAsync(d => d.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching defense with ID {DefenseId}.", id);
                throw;
            }
        }

        public async Task<List<DefenseModel>> GetDefensesByDegreeAsync(string degree)
        {
            _logger.LogInformation("Fetching defenses for degree {Degree}.", degree);
            try
            {
                return await _context.Defenses
                    .Include(d => d.Program)
                    .Where(d => d.Program.Degree == degree)
                    .OrderBy(d => d.DefenseDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching defenses for degree {Degree}.", degree);
                throw;
            }
        }

        public async Task DeleteDefenseAsync(int id)
        {
            _logger.LogInformation("Deleting defense with ID {DefenseId} from database.", id);
            try
            {
                var defense = await _context.Defenses.FirstOrDefaultAsync(d => d.Id == id);
                if (defense == null)
                {
                    _logger.LogWarning("Defense with ID {DefenseId} not found for deletion.", id);
                    throw new KeyNotFoundException("Defense not found.");
                }

                _context.Defenses.Remove(defense);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting defense with ID {DefenseId}.", id);
                throw;
            }
        }

        public async Task AddDefenseAsync(DefenseModel defense)
        {
            _logger.LogInformation("Adding new defense with title {DefenseTitle}.", defense.DefenseTitle);
            try
            {
                await _context.Defenses.AddAsync(defense);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding defense with title {DefenseTitle}.", defense.DefenseTitle);
                throw;
            }
        }

        public async Task UpdateDefenseAsync(DefenseModel defense)
        {
            _logger.LogInformation("Updating defense with ID {DefenseId} in database.", defense.Id);
            try
            {
                _context.Defenses.Update(defense);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating defense with ID {DefenseId}.", defense.Id);
                throw;
            }
        }


    }
}
