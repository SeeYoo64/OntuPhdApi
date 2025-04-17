using Microsoft.EntityFrameworkCore;
using OntuPhdApi.Data;
using OntuPhdApi.Models.Institutes;
using OntuPhdApi.Repositories.Institutes;

namespace OntuPhdApi.Services.Institutes
{
    public class InstituteService : IInstituteService
    {
        private readonly IInstituteRepository _instituteRepository;
        private readonly AppDbContext _context;
        private readonly ILogger<InstituteService> _logger;

        public InstituteService(
            IInstituteRepository instituteRepository,
            AppDbContext context,
            ILogger<InstituteService> logger)
        {
            _instituteRepository = instituteRepository;
            _context = context;
            _logger = logger;
        }

        public async Task<List<InstituteDto>> GetInstitutesAsync()
        {
            _logger.LogInformation("Fetching all institutes.");
            var institutes = await _instituteRepository.GetAllInstitutesAsync();
            return institutes.Select(MapToDto).ToList();
        }

        public async Task<InstituteDto> GetInstituteAsync(int id)
        {
            _logger.LogInformation("Fetching institute with ID {InstituteId}.", id);
            var institute = await _instituteRepository.GetInstituteByIdAsync(id);
            return institute != null ? MapToDto(institute) : null;
        }

        public async Task<InstituteDto> AddInstituteAsync(InstituteDto instituteDto)
        {
            _logger.LogInformation("Adding new institute {InstituteName}.", instituteDto.Name);
            try
            {
                var institute = MapToModel(instituteDto);
                institute.Id = await _instituteRepository.InsertInstituteAsync(institute);
                _logger.LogInformation("Institute {InstituteName} added with ID {InstituteId}.", institute.Name, institute.Id);
                return MapToDto(institute);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add institute {InstituteName}.", instituteDto.Name);
                throw;
            }
        }

        public async Task<InstituteDto> UpdateInstituteAsync(int id, InstituteDto instituteDto)
        {
            _logger.LogInformation("Updating institute with ID {InstituteId}.", id);
            try
            {
                var existingInstitute = await _instituteRepository.GetInstituteByIdAsync(id);
                if (existingInstitute == null)
                {
                    _logger.LogWarning("Institute with ID {InstituteId} not found.", id);
                    throw new Exception($"Institute with ID {id} not found.");
                }

                var institute = MapToModel(instituteDto);
                institute.Id = id;
                await _instituteRepository.UpdateInstituteAsync(institute);
                _logger.LogInformation("Institute {InstituteName} with ID {InstituteId} updated.", institute.Name, id);
                return MapToDto(institute);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update institute {InstituteName} with ID {InstituteId}.", instituteDto.Name, id);
                throw;
            }
        }

        public async Task DeleteInstituteAsync(int id)
        {
            _logger.LogInformation("Deleting institute with ID {InstituteId}.", id);
            try
            {
                var institute = await _instituteRepository.GetInstituteByIdAsync(id);
                if (institute == null)
                {
                    _logger.LogWarning("Institute with ID {InstituteId} not found for deletion.", id);
                    throw new Exception($"Institute with ID {id} not found.");
                }

                var hasPrograms = await _context.Programs.AnyAsync(p => p.InstituteId == id);
                if (hasPrograms)
                {
                    _logger.LogWarning("Cannot delete institute with ID {InstituteId} because it is referenced by programs.", id);
                    throw new Exception("Cannot delete institute because it is referenced by one or more programs.");
                }

                await _instituteRepository.DeleteInstituteAsync(id);
                _logger.LogInformation("Institute with ID {InstituteId} deleted.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete institute with ID {InstituteId}.", id);
                throw;
            }
        }

        private Institute MapToModel(InstituteDto dto)
        {
            return new Institute
            {
                Name = dto.Name
            };
        }

        private InstituteDto MapToDto(Institute institute)
        {
            return new InstituteDto
            {
                Id = institute.Id,
                Name = institute.Name
            };
        }

    }
}
