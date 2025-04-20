using OntuPhdApi.Models.Programs.Components;

namespace OntuPhdApi.Repositories.SpecAndField
{
    public interface ISpecAndFieldRepository
    {
        Task<List<FieldOfStudy>> GetFieldsWithSpecialitiesAsync(string? degree);
        Task<List<Speciality>> GetSpecialitiesByCodeAsync(string code);
    }
}
