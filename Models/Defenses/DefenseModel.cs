using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using OntuPhdApi.Models.Defense;
using OntuPhdApi.Models.Programs;

namespace OntuPhdApi.Models.Defense
{
    public class DefenseModel
    {
        public int Id { get; set; }
        public string CandidateNameSurname { get; set; } 
        public string DefenseTitle { get; set; }
        public List<string>? ScienceTeachers { get; set; }
        public DateTime DefenseDate { get; set; }
        public string? Address { get; set; }
        public string? Message { get; set; } 
        public string? Placeholder { get; set; }
        public List<CompositionOfRada>? Members { get; set; }
        public List<DefenseFile>? Files { get; set; } 
        public DateTime PublicationDate { get; set; }

        // Навигационное свойство для связи с Program
        public int ProgramId { get; set; }
        [ForeignKey("ProgramId")]
        public ProgramModel Program { get; set; } // Связь с программой
    }

    public class DefenseFile
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public string Type { get; set; }
    }

    public class CompositionOfRada
    {
        public string Position { get; set; }
        public List<MemberOfRada>? Members { get; set; } 
    }

    public class MemberOfRada
    {
        public string NameSurname { get; set; }
        public string Title { get; set; }
        public string? ToolTip { get; set; }
    }
}