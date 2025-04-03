namespace OntuPhdApi.Models.Defense
{
    public class DefensePhdModel
    {
        public int Id { get; set; }
        public int ProgramId { get; set; }
        public string NameSurname { get; set; }
        public string DefenseName { get; set; }
        public ProgramDefense ProgramInfo { get; set; }
        public string ScienceTeachers { get; set; }
        public DateTime DateOfDefense { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string NameOfCompositionOfRada { get; set; }
        public List<MemberOfRada> Members { get; set; }
        public List<Files> Files { get; set; }
        public DateTime DateOfPublication {get; set;}

    }

    public class Files
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public string Type { get; set; }

    }

    public class MemberOfRada
    {
        public string Position { get; set; }
        public List<string> NameSurname { get; set; }
        public string ToolTip { get; set; }
    }
}
