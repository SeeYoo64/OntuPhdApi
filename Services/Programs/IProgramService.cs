﻿using OntuPhdApi.Models.Programs;

namespace OntuPhdApi.Services.Programs
{
    public interface IProgramService
    {
        List<ProgramModel> GetPrograms();
        ProgramModel GetProgramById(int id);
        List<ProgramsFieldDto> GetProgramsFields();
        List<ProgramsDegreeDto> GetProgramsDegrees(string degree = null);
        void AddProgram(ProgramModel program);
    }
}
