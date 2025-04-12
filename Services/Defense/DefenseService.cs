using Npgsql;
using OntuPhdApi.Models.Defense;
using OntuPhdApi.Models.Programs;
using OntuPhdApi.Repositories.Defense;
using OntuPhdApi.Utilities;
using System.Text.Json;

namespace OntuPhdApi.Services.Defense
{
    public class DefenseService : IDefenseService
    {
        private readonly IDefenseRepository _defenseRepository;
        private readonly ILogger<DefenseService> _logger;

        public DefenseService(IDefenseRepository defenseRepository, ILogger<DefenseService> logger)
        {
            _defenseRepository = defenseRepository;
            _logger = logger;
        }

        public async Task<List<DefenseDto>> GetDefensesAsync()
        {
            _logger.LogInformation("Fetching all defenses.");
            try
            {
                var defenses = await _defenseRepository.GetAllDefensesAsync();
                return DefenseMapper.ToDtoList(defenses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch defenses.");
                throw;
            }
        }


        public async Task<DefenseDto> GetDefenseByIdAsync(int id)
        {
            _logger.LogInformation("Fetching defense with ID {DefenseId}.", id);
            try
            {
                var defense = await _defenseRepository.GetDefenseByIdAsync(id);
                if (defense == null)
                {
                    _logger.LogWarning("Defense with ID {DefenseId} not found.", id);
                    return null;
                }
                return DefenseMapper.ToDto(defense);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch defense with ID {DefenseId}.", id);
                throw;
            }
        }

        public async Task<List<DefenseDto>> GetDefensesByDegreeAsync(string degree)
        {
            _logger.LogInformation("Fetching defenses for degree {Degree}.", degree);
            try
            {
                var defenses = await _defenseRepository.GetDefensesByDegreeAsync(degree);
                return DefenseMapper.ToDtoList(defenses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch defenses for degree {Degree}.", degree);
                throw;
            }
        }



    }
}
