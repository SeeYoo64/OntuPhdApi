using OntuPhdApi.Models.Defense;

namespace OntuPhdApi.Services.Defense
{
    public interface IDefenseService
    {
        List<DefenseModel> GetDefenses();
        DefenseModel GetDefenseById(int id);
        List<DefenseModel> GetDefensesByDegree(string degree);
    }
}
