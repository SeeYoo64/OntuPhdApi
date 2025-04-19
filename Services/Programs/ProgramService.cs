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

        public ProgramService(
            IProgramRepository programRepository, IProgramMapper mapper,
            AppDbContext context,
            ILogger<ProgramService> logger, IInstituteService instituteService
            )
        {
            _programRepository = programRepository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<IEnumerable<ProgramResponseDto>> GetAllProgramsAsync()
        {
            var programs = await _programRepository.GetAllAsync();
            return _mapper.ToProgramResponseDtos(programs);
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

        public async Task<bool> UpdateProgramAsync(int id, ProgramUpdateDto programDto)
        {
            var existingProgram = await _programRepository.GetByIdAsync(id);
            if (existingProgram == null)
            {
                return false;
            }

            _mapper.UpdateProgramModel(existingProgram, programDto);
            await _programRepository.UpdateAsync(existingProgram);
            return true;
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
