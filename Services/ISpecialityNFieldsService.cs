using OntuPhdApi.Models.Programs;

namespace OntuPhdApi.Services
{
    public interface ISpecialityNFieldsService
    {
        List<FieldOfStudyDto> GetSpecialitiesNFields();
    }
}
