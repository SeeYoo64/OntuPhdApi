using Npgsql;
using OntuPhdApi.Controllers;
using OntuPhdApi.Models.Programs;

namespace OntuPhdApi.Services.Programs
{
    public interface IProgramService
    {
        Task<List<ProgramModel>> GetPrograms();
        Task<ProgramModel> GetProgram(int id);
        Task AddProgram(ProgramModel program, string? filePath, string? contentType, long fileSize);
        Task AddProgram(ProgramModel program, string? filePath, string? contentType, long fileSize, NpgsqlConnection connection, NpgsqlTransaction transaction); 
        Task UpdateProgramWithDocument(ProgramModel program, string filePath, string fileName, string contentType, long fileSize);
        Task UpdateProgramWithDocument(ProgramModel program, string filePath, string fileName, string contentType, long fileSize, NpgsqlConnection connection, NpgsqlTransaction transaction); 
        Task UpdateProgram(ProgramModel program);
        Task DeleteProgram(int id);
        Task DeleteProgram(int id, NpgsqlConnection connection, NpgsqlTransaction transaction);
        Task<List<ProgramsDegreeDto>> GetProgramsDegrees(DegreeType? degree);
    }
}
