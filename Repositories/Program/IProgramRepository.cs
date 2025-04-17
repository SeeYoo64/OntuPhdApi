using Npgsql;
using OntuPhdApi.Controllers;
using OntuPhdApi.Models.Programs;

namespace OntuPhdApi.Repositories.Program
{
    public interface IProgramRepository
    {
        Task<IEnumerable<ProgramModel>> GetAllProgramsAsync();
        Task<ProgramModel> GetProgramByIdAsync(int id);
        Task<IEnumerable<ProgramModel>> GetProgramsByDegreeAsync(string degreeType);
        Task AddProgramAsync(ProgramModel program);
        Task UpdateProgramAsync(ProgramModel program);
        Task DeleteProgramAsync(int id);
    }
}
