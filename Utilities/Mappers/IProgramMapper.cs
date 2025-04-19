using OntuPhdApi.Models.Programs;

namespace OntuPhdApi.Utilities.Mappers
{
    public interface IProgramMapper
    {
        ProgramResponseDto ToProgramResponseDto(ProgramModel model);
        List<ProgramResponseDto> ToProgramResponseDtos(IEnumerable<ProgramModel> models);
        ProgramModel ToProgramModel(ProgramCreateDto programDto);
        void UpdateProgramModel(ProgramModel program, ProgramUpdateDto programDto);
        ProgramDegreeDto ToProgramDegree(ProgramModel program);
        List<ProgramDegreeDto> ToProgramDegrees(IEnumerable<ProgramModel> models);
    }
}
