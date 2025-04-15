using OntuPhdApi.Models.Defense;

namespace OntuPhdApi.Repositories.Defense
{
    public interface IDefenseRepository
    {
        Task<List<DefenseModel>> GetAllDefensesAsync();
        Task<DefenseModel> GetDefenseByIdAsync(int id);
        Task<List<DefenseModel>> GetDefensesByDegreeAsync(string degree);
        Task DeleteDefenseAsync(int id);
        Task UpdateDefenseAsync(DefenseModel defense);
        Task AddDefenseAsync(DefenseModel defense);
    }
}
