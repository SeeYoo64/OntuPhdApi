using OntuPhdApi.Models.Defense;

namespace OntuPhdApi.Services.Defense
{
    public interface IDefenseService
    {
        Task<List<DefenseDto>> GetDefensesAsync();
        Task<DefenseDto> GetDefenseByIdAsync(int id);
        Task<List<DefenseDto>> GetDefensesByDegreeAsync(string degree);
        Task DeleteDefenseAsync(int id);
    }
}
