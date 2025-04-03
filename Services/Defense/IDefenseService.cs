using OntuPhdApi.Models.Defense;

namespace OntuPhdApi.Services.Defense
{
    public interface IDefenseService
    {
        List<DefensePhdModel> GetDefenses();
    }
}
