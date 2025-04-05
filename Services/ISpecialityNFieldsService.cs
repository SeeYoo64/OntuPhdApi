using OntuPhdApi.Models.Programs;

namespace OntuPhdApi.Services
{
    public interface ISpecialityNFieldsService
    {
        List<FieldOfStudyDto> GetSpecialitiesNFields(string degree = null);

        List<ShortSpeciality> GetSpecialitiesByCode(string code);

    }
}
