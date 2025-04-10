using Npgsql;
using OntuPhdApi.Controllers;
using OntuPhdApi.Models.Programs;

namespace OntuPhdApi.Repositories
{
    public interface IProgramRepository
    {
        Task<List<ProgramModel>> GetAllProgramsAsync();
        Task<ProgramModel> GetProgramByIdAsync(int id);
        Task<List<ProgramComponent>> GetProgramComponentsAsync(int programId);
        Task<List<Job>> GetProgramJobsAsync(int programId);
        Task<List<ProgramsDegreeDto>> GetProgramsByDegreeAsync(DegreeType? degree);


        Task<int> InsertProgramAsync(ProgramModel program);
        Task<int> InsertProgramAsync(ProgramModel program, NpgsqlConnection connection, NpgsqlTransaction transaction); // С транзакцией
        Task UpdateProgramAsync(ProgramModel program);
        Task UpdateProgramAsync(ProgramModel program, NpgsqlConnection connection, NpgsqlTransaction transaction); // С транзакцией
        Task DeleteProgramAsync(int id);
        Task DeleteProgramAsync(int id, NpgsqlConnection connection, NpgsqlTransaction transaction); // С транзакцией

    }
}
