using OntuPhdApi.Models.Programs;

namespace OntuPhdApi.Services.Programs
{
    public interface IProgramService
    {
        List<ProgramView> GetPrograms();
        ProgramView GetProgramById(int id);
        List<ProgramsDegreeDto> GetProgramsDegrees(string degree = null);
        List<ProgramsFieldDto> GetProgramsFields();
        void AddProgram(ProgramView program);
    }
}
