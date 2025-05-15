using OntuPhdApi.Models.Programs;
using OntuPhdApi.Models.Programs.Dto;

namespace OntuPhdApi.Utilities.Mappers
{
    public interface IProgramMapper
    {
        ProgramResponseDto ToProgramResponseDto(ProgramModel model);
        List<ProgramResponseDto> ToProgramResponseDtos(IEnumerable<ProgramModel> models);
        ProgramModel ToProgramModel(ProgramCreateUpdateDto programDto);
        void UpdateProgramModel(ProgramModel program, ProgramCreateUpdateDto dto);
        ProgramDegreeDto ToProgramDegree(ProgramModel program);
        List<ProgramDegreeDto> ToProgramDegrees(IEnumerable<ProgramModel> models);

        ProgramShortDto ToProgramShortDto(ProgramModel program);
        List<ProgramShortDto> ToProgramShortDtos(IEnumerable<ProgramModel> models);
    }
}
