namespace OntuPhdApi.Models.Defense
{
    public class DefenseCreateDto
    {
        public string CandidateNameSurname { get; set; }
        public string DefenseTitle { get; set; }
        public List<string>? ScienceTeachers { get; set; }
        public string? CandidateDegree { get; set; }
        public DateTime DefenseDate { get; set; }
        public string? Address { get; set; }
        public string? Message { get; set; }
        public string? Placeholder { get; set; }
        public List<CompositionOfRadaDto>? Members { get; set; }
        public List<DefenseFileDto>? Files { get; set; }
        public DateTime PublicationDate { get; set; }
        public int ProgramId { get; set; }
    }

}
