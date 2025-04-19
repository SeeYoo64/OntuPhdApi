using Npgsql;
using OntuPhdApi.Controllers;
using OntuPhdApi.Models.Programs.Dto;

namespace OntuPhdApi.Services.Programs
{
    public interface IProgramService
    {
        Task<IEnumerable<ProgramResponseDto>> GetAllProgramsAsync();
        Task<ProgramResponseDto> GetProgramByIdAsync(int id);
        Task<ProgramResponseDto> CreateProgramAsync(ProgramCreateUpdateDto programDto);
        Task<bool> UpdateProgramAsync(int id, ProgramUpdateDto programDto);
        Task<bool> DeleteProgramAsync(int id);
        Task<IEnumerable<ProgramDegreeDto>> GetProgramsByDegreeAsync(string degree);
    }
}
