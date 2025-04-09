using OntuPhdApi.Controllers;
using OntuPhdApi.Models.Programs;

namespace OntuPhdApi.Services.Programs
{
    public interface IProgramService
    {
        List<ProgramModel> GetPrograms();
        Task<ProgramModel> GetProgram(int id);
        List<ProgramsDegreeDto> GetProgramsDegrees(DegreeType? degree);
        Task AddProgram(ProgramModel program, string filePath, string contentType, long fileSize);
        Task UpdateProgram(ProgramModel program);
        Task UpdateProgramWithDocument(ProgramModel program, string filePath, string fileName, string contentType, long fileSize);
        Task DeleteProgram(int id);
    }
}
