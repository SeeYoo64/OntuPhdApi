using OntuPhdApi.Models.Programs;
using OntuPhdApi.Models.Programs.Components;

namespace OntuPhdApi.Services.SpecAndFields
{
    public interface ISpecialityNFieldsService
    {
        Task<List<FieldOfStudyResponseDto>> GetFieldsWithSpecialitiesAsync(string? degree);
        Task<List<SpecialityDto>> GetSpecialitiesByCodeAsync(string code);

    }
}
