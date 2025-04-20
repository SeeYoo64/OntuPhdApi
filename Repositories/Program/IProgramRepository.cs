using OntuPhdApi.Models.Programs;

namespace OntuPhdApi.Repositories.Program
{
    public interface IProgramRepository
    {
        Task<IEnumerable<ProgramModel>> GetAllAsync();
        Task<ProgramModel> GetByIdAsync(int id);
        Task AddAsync(ProgramModel program);
        Task UpdateAsync(ProgramModel program);
        Task DeleteAsync(int id);
        Task<IEnumerable<ProgramModel>> GetByDegreeAsync(string degree);

    }
}
