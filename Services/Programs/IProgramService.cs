using Npgsql;
using OntuPhdApi.Controllers;
using OntuPhdApi.Models.Programs.Dto;

namespace OntuPhdApi.Services.Programs
{
    public interface IProgramService
    {
        Task<IEnumerable<ProgramResponseDto>> GetAllProgramsAsync();
        Task<IEnumerable<ProgramShortDto>> GetShortByDegreeAsync(string degree = null);
        Task<ProgramResponseDto> GetProgramByIdAsync(int id);
        Task<ProgramResponseDto> CreateProgramAsync(ProgramCreateUpdateDto programDto);
        Task UpdateProgramAsync(int id, ProgramCreateUpdateDto programDto);
        Task<bool> DeleteProgramAsync(int id);
        Task<IEnumerable<ProgramDegreeDto>> GetProgramsByDegreeAsync(string degree);
    }
}
