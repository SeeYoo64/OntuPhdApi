using System.Text.Json.Serialization;

namespace OntuPhdApi.Models.Programs.Components
{
    public class ProgramComponent
    {
        public int Id { get; set; }
        public int ProgramId { get; set; }

        public string? ComponentType { get; set; }
        public string? ComponentName { get; set; }
        public int? ComponentCredits { get; set; }
        public int? ComponentHours { get; set; }

        public List<ControlForm> ControlForms { get; set; }

        public ProgramModel ProgramModel { get; set; }
    }

    public class ControlForm
    {
        public int Id { get; set; }
        public int ProgramComponentId { get; set; }
        public string Type { get; set; }

        public ProgramComponent ProgramComponent { get; set; }
    }

}
