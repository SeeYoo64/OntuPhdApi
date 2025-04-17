using Npgsql;
using OntuPhdApi.Controllers;
using OntuPhdApi.Models.Programs;

namespace OntuPhdApi.Services.Programs
{
    public interface IProgramService
    {
        Task<IEnumerable<ProgramModel>> GetAllProgramsAsync();
        Task<ProgramModel> GetProgramByIdAsync(int id);
        Task AddProgramAsync(ProgramCreateUpdateDto programDto);
        Task UpdateProgramAsync(int id, ProgramCreateUpdateDto programDto);
        Task DeleteProgramAsync(int id);
        Task<IEnumerable<ProgramsDegreeDto>> GetProgramsByDegreeAsync(string degreeType);
    }
}
