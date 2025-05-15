using Microsoft.EntityFrameworkCore;
using OntuPhdApi.Controllers;
using OntuPhdApi.Data;
using OntuPhdApi.Models.Programs.Components;
using OntuPhdApi.Models.Programs.Dto;
using OntuPhdApi.Repositories.Program;
using OntuPhdApi.Services.Institutes;
using OntuPhdApi.Utilities.Mappers;

namespace OntuPhdApi.Services.Programs
{
    public class ProgramService : IProgramService
    {
        private readonly IProgramRepository _programRepository;
        private readonly IProgramMapper _mapper;
        private readonly ILogger<ProgramService> _logger;
        private readonly AppDbContext _context;

        public ProgramService(
            IProgramRepository programRepository, IProgramMapper mapper,
            AppDbContext context,
            ILogger<ProgramService> logger
            )
        {
            _programRepository = programRepository;
            _mapper = mapper;
            _logger = logger;
            _context = context;
        }


        public async Task<IEnumerable<ProgramResponseDto>> GetAllProgramsAsync()
        {
            var programs = await _programRepository.GetAllAsync();
            return _mapper.ToProgramResponseDtos(programs);
        }

        public async Task<IEnumerable<ProgramShortDto>> GetShortByDegreeAsync(string degree = null)
        {
            var programs = await _programRepository.GetShortByDegreeAsync(degree);
            return _mapper.ToProgramShortDtos(programs);
        }
        public async Task<ProgramResponseDto> GetProgramByIdAsync(int id)
        {
            var program = await _programRepository.GetByIdAsync(id);
            return program != null ? _mapper.ToProgramResponseDto(program) : null;
        }

        public async Task<ProgramResponseDto> CreateProgramAsync(ProgramCreateUpdateDto programDto)
        {
            var program = _mapper.ToProgramModel(programDto);
            await _programRepository.AddAsync(program);
            return _mapper.ToProgramResponseDto(program);
        }

        public async Task<bool> UpdateProgramAsync(int id, ProgramCreateUpdateDto programDto)
        {
            var existingProgram = await _programRepository.GetByIdAsync(id);
            if (existingProgram == null)
                return false;

            try
            {

                if (programDto.Institute.Id != null)
                {
                    var institute = await _context.Institutes.FindAsync(programDto.Institute.Id);
                    if (institute == null) return false;
                    existingProgram.Institute = institute;
                }

                _mapper.UpdateProgramModel(existingProgram, programDto);
                await _programRepository.UpdateAsync(existingProgram);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating program ID = {ProgramId}", id);
                return false;
            }
        }

        public async Task<bool> DeleteProgramAsync(int id)
        {
            var program = await _programRepository.GetByIdAsync(id);
            if (program == null)
            {
                return false;
            }

            await _programRepository.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<ProgramDegreeDto>> GetProgramsByDegreeAsync(string degree)
        {
            var programs = await _programRepository.GetByDegreeAsync(degree);
            return _mapper.ToProgramDegrees(programs);
        }


    }
}
