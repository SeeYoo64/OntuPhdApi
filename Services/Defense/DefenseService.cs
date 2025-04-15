using Microsoft.EntityFrameworkCore;
using Npgsql;
using OntuPhdApi.Data;
using OntuPhdApi.Models.Defense;
using OntuPhdApi.Models.Programs;
using OntuPhdApi.Repositories.Defense;
using OntuPhdApi.Utilities.Mappers;
using System.Text.Json;

namespace OntuPhdApi.Services.Defense
{
    public class DefenseService : IDefenseService
    {
        private readonly IDefenseRepository _defenseRepository;
        private readonly ILogger<DefenseService> _logger;
        private readonly AppDbContext _context;
        public DefenseService(IDefenseRepository defenseRepository, 
            ILogger<DefenseService> logger,
            AppDbContext context

            )
        {
            _defenseRepository = defenseRepository;
            _logger = logger;
            _context = context;
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

        public async Task DeleteDefenseAsync(int id)
        {
            _logger.LogInformation("Deleting defense with ID {DefenseId}.", id);
            try
            {
                // Проверяем существование защиты
                var defense = await _defenseRepository.GetDefenseByIdAsync(id);
                if (defense == null)
                {
                    _logger.LogWarning("Defense with ID {DefenseId} not found for deletion.", id);
                    throw new KeyNotFoundException("Defense not found.");
                }

                // Удаляем защиту из базы
                await _defenseRepository.DeleteDefenseAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete defense with ID {DefenseId}.", id);
                throw;
            }
        }

        public async Task<int> AddDefenseAsync(DefenseCreateDto defenseDto)
        {
            if (defenseDto == null || string.IsNullOrEmpty(defenseDto.CandidateNameSurname) || string.IsNullOrEmpty(defenseDto.DefenseTitle))
            {
                _logger.LogWarning("Invalid defense data provided for creation.");
                throw new ArgumentException("CandidateNameSurname and DefenseTitle are required.");
            }

            _logger.LogInformation("Adding new defense with title {DefenseTitle}.", defenseDto.DefenseTitle);
            try
            {
                // Проверяем, существует ли Program с указанным ProgramId
                var programExists = await _context.Programs.AnyAsync(p => p.Id == defenseDto.ProgramId);
                if (!programExists)
                {
                    _logger.LogWarning("Program with ID {ProgramId} not found for defense creation.", defenseDto.ProgramId);
                    throw new KeyNotFoundException("Program not found.");
                }

                var defense = DefenseMapper.ToEntity(defenseDto);
                await _defenseRepository.AddDefenseAsync(defense);
                return defense.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add defense with title {DefenseTitle}.", defenseDto.DefenseTitle);
                throw;
            }
        }



        public async Task UpdateDefenseAsync(int id, DefenseCreateDto defenseDto)
        {
            if (defenseDto == null || string.IsNullOrEmpty(defenseDto.CandidateNameSurname) || string.IsNullOrEmpty(defenseDto.DefenseTitle))
            {
                _logger.LogWarning("Invalid defense data provided for update.");
                throw new ArgumentException("CandidateNameSurname and DefenseTitle are required.");
            }

            _logger.LogInformation("Updating defense with ID {DefenseId}.", id);
            try
            {
                // Проверяем, существует ли защита
                var existingDefense = await _defenseRepository.GetDefenseByIdAsync(id);
                if (existingDefense == null)
                {
                    _logger.LogWarning("Defense with ID {DefenseId} not found for update.", id);
                    throw new KeyNotFoundException("Defense not found.");
                }

                // Проверяем, существует ли Program с указанным ProgramId
                var programExists = await _context.Programs.AnyAsync(p => p.Id == defenseDto.ProgramId);
                if (!programExists)
                {
                    _logger.LogWarning("Program with ID {ProgramId} not found for defense update.", defenseDto.ProgramId);
                    throw new KeyNotFoundException("Program not found.");
                }

                // Обновляем поля сущности
                existingDefense.CandidateNameSurname = defenseDto.CandidateNameSurname;
                existingDefense.DefenseTitle = defenseDto.DefenseTitle;
                existingDefense.ScienceTeachers = defenseDto.ScienceTeachers;
                existingDefense.DefenseDate = defenseDto.DefenseDate;
                existingDefense.Address = defenseDto.Address;
                existingDefense.Message = defenseDto.Message;
                existingDefense.Placeholder = defenseDto.Placeholder;
                existingDefense.Members = defenseDto.Members?.Select(m => new CompositionOfRada
                {
                    Position = m.Position,
                    Members = m.Members?.Select(member => new MemberOfRada
                    {
                        NameSurname = member.NameSurname,
                        ToolTip = member.ToolTip
                    }).ToList()
                }).ToList();
                existingDefense.Files = defenseDto.Files?.Select(f => new DefenseFile
                {
                    Name = f.Name,
                    Link = f.Link,
                    Type = f.Type
                }).ToList();
                existingDefense.PublicationDate = defenseDto.PublicationDate;
                existingDefense.ProgramId = defenseDto.ProgramId;

                await _defenseRepository.UpdateDefenseAsync(existingDefense);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update defense with ID {DefenseId}.", id);
                throw;
            }
        }

    }
}
