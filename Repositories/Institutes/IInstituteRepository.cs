using OntuPhdApi.Models.Institutes;

namespace OntuPhdApi.Repositories.Institutes
{
    public interface IInstituteRepository
    {
        Task<List<Institute>> GetAllInstitutesAsync();
        Task<Institute> GetInstituteByIdAsync(int id);
        Task<int> InsertInstituteAsync(Institute institute);
        Task UpdateInstituteAsync(Institute institute);
        Task DeleteInstituteAsync(int id);
    }
}
