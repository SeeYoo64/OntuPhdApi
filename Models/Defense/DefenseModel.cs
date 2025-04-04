using System.Text.Json.Serialization;

namespace OntuPhdApi.Models.Defense
{
    public class DefenseModel
    {
        public int Id { get; set; }
        [JsonIgnore]
        public int ProgramId { get; set; }
        public string NameSurname { get; set; }
        public string DefenseName { get; set; }
        public ProgramDefense ProgramInfo { get; set; }
        public List<string> ScienceTeachers { get; set; }
        public DateTime DateOfDefense { get; set; }
        public string Address { get; set; }
        public string Message { get; set; }
        public string NameOfCompositionOfRada { get; set; }
        public string Placeholder { get; set; }
        public List<CompositionOfRada> Members { get; set; }
        public List<Files> Files { get; set; }
        public DateTime DateOfPublication {get; set;}

    }

    public class Files
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public string Type { get; set; }

    }

    public class CompositionOfRada
    {
        public string Position { get; set; }
        public List<MembersOfRada> Member { get; set; }
    }

    public class MembersOfRada
    {
        public string NameSurname { get; set; }
        public string ToolTip { get; set; }
    }
}
