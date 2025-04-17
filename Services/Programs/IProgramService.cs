using Npgsql;
using OntuPhdApi.Controllers;
using OntuPhdApi.Models.Programs;

namespace OntuPhdApi.Services.Programs
{
    public interface IProgramService
    {
        Task<List<ProgramModelDto>> GetPrograms();
        Task<ProgramModelDto> GetProgram(int id);
        Task AddProgram(ProgramModel program, string? filePath, string? contentType, long fileSize, string? instituteName);
        Task UpdateProgramWithDocument(ProgramModel program, string filePath, string fileName, string contentType, long fileSize, string? instituteName);
        Task UpdateProgram(ProgramModel program, string? instituteName);
        Task DeleteProgram(int id);
        Task<List<ProgramsDegreeDto>> GetProgramsDegrees(DegreeType? degree);
    }
}
