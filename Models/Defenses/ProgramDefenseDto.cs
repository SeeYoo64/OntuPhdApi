﻿using OntuPhdApi.Models.Programs;
using OntuPhdApi.Models.Programs.Components;

namespace OntuPhdApi.Models.Defense
{
    public class ProgramDefenseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public FieldOfStudyDto? FieldOfStudy { get; set; }
        public SpecialityDto? Speciality { get; set; }
    }
}
