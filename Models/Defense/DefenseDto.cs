namespace OntuPhdApi.Models.Defense
{
    public class DefenseDto
    {
        public int Id { get; set; }
        public string CandidateNameSurname { get; set; }
        public string DefenseTitle { get; set; }
        public List<string>? ScienceTeachers { get; set; }
        public DateTime DefenseDate { get; set; }
        public string? Address { get; set; }
        public string? Message { get; set; }
        public string? Placeholder { get; set; }
        public List<CompositionOfRadaDto>? Members { get; set; }
        public List<DefenseFileDto>? Files { get; set; }
        public DateTime PublicationDate { get; set; }
        public ProgramDefenseDto ProgramInfo { get; set; }
    }

    public class DefenseFileDto
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public string Type { get; set; }
    }

    public class CompositionOfRadaDto
    {
        public string Position { get; set; }
        public List<MemberOfRadaDto>? Members { get; set; }
    }

    public class MemberOfRadaDto
    {
        public string NameSurname { get; set; }
        public string? ToolTip { get; set; }
    }
}
