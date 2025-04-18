namespace OntuPhdApi.Models.Programs.Components
{
    public class ProgramCompetence
    {
        public int Id { get; set; }
        public int ProgramId { get; set; }

        public string? IntegralCompetence { get; set; }

        public List<OverallCompetence> OverallCompetences { get; set; }
        public List<SpecialCompetence> SpecialCompetences { get; set; }

        public ProgramModel Program { get; set; }
    }

    public class OverallCompetence
    {
        public int Id { get; set; }
        public int ProgramCompetenceId { get; set; }

        public string Description { get; set; }

        public ProgramCompetence ProgramCompetence { get; set; }
    }

    public class SpecialCompetence
    {
        public int Id { get; set; }
        public int ProgramCompetenceId { get; set; }

        public string Description { get; set; }

        public ProgramCompetence ProgramCompetence { get; set; }
    }
}
