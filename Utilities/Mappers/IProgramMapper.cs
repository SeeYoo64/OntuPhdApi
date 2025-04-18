using OntuPhdApi.Models.Programs;

namespace OntuPhdApi.Utilities.Mappers
{
    public interface IProgramMapper
    {
        ProgramResponseDto ToProgramResponseDto(ProgramModel program);
        IEnumerable<ProgramResponseDto> ToProgramResponseDtos(IEnumerable<ProgramModel> programs);
        ProgramModel ToProgramModel(ProgramCreateDto programDto);
        void UpdateProgramModel(ProgramModel program, ProgramUpdateDto programDto);
    }
}
