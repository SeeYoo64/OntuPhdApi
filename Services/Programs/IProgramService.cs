using OntuPhdApi.Models.Programs;

namespace OntuPhdApi.Services.Programs
{
    public interface IProgramService
    {
        List<ProgramModel> GetPrograms();
        ProgramModel GetProgramById(int id);
        List<ProgramsDegreeDto> GetProgramsDegrees(string degree = null);
        List<ProgramsFieldDto> GetProgramsFields();
        void AddProgram(ProgramModel program);
    }
}
