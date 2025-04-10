using Npgsql;
using OntuPhdApi.Controllers;
using OntuPhdApi.Models.Programs;

namespace OntuPhdApi.Repositories
{
    public interface IProgramRepository
    {
        Task<List<ProgramModel>> GetAllProgramsAsync();
        Task<ProgramModel> GetProgramByIdAsync(int id);
        Task<int> InsertProgramAsync(ProgramModel program);
        Task UpdateProgramAsync(ProgramModel program);
        Task DeleteProgramAsync(int id);
        Task<List<ProgramsDegreeDto>> GetProgramsByDegreeAsync(DegreeType? degree);
        Task<List<ProgramComponent>> GetProgramComponentsAsync(int programId);
        Task<List<Job>> GetProgramJobsAsync(int programId);
    }
}
