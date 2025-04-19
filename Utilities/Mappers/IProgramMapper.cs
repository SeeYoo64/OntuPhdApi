using OntuPhdApi.Models.Programs;
using OntuPhdApi.Models.Programs.Dto;

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
        IEnumerable<ProgramResponseDto> ToProgramResponseDto1s(IEnumerable<ProgramModel> models);
    }
}
