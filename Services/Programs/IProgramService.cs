using Npgsql;
using OntuPhdApi.Controllers;
using OntuPhdApi.Models.Programs.Dto;

namespace OntuPhdApi.Services.Programs
{
    public interface IProgramService
    {
        Task<IEnumerable<ProgramResponseDto>> GetAllProgramsAsync();
        Task<ProgramShortDto> GetShortByIdAsync(int id);
        Task<ProgramResponseDto> GetProgramByIdAsync(int id);
        Task<ProgramResponseDto> CreateProgramAsync(ProgramCreateUpdateDto programDto);
        Task<bool> UpdateProgramAsync(int id, ProgramCreateUpdateDto programDto);
        Task<bool> DeleteProgramAsync(int id);
        Task<IEnumerable<ProgramDegreeDto>> GetProgramsByDegreeAsync(string degree);
    }
}
