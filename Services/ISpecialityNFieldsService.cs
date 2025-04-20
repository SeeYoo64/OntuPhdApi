using OntuPhdApi.Models.Programs;
using OntuPhdApi.Models.Programs.Components;

namespace OntuPhdApi.Services
{
    public interface ISpecialityNFieldsService
    {
        List<FieldOfStudyDto> GetSpecialitiesNFields(string degree = null);

        List<SpecialityDto> GetSpecialitiesByCode(string code);

    }
}
