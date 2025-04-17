using OntuPhdApi.Models.Institutes;

namespace OntuPhdApi.Services.Institutes
{
    public interface IInstituteService
    {
        Task<List<InstituteDto>> GetInstitutesAsync();
        Task<InstituteDto> GetInstituteAsync(int id);
        Task<InstituteDto> AddInstituteAsync(InstituteDto instituteDto);
        Task<InstituteDto> UpdateInstituteAsync(int id, InstituteDto instituteDto);
        Task DeleteInstituteAsync(int id);
    }
}
