namespace OntuPhdApi.Models
{
    public class ProgramComponent
    {
        public int Id { get; set; }
        public int ProgramId { get; set; }
        public string ComponentType { get; set; }
        public string ComponentName { get; set; }
        public int ComponentCredits { get; set; }
        public int ComponentHours { get; set; }
        public List<string> ControlForm { get; set; } // Массив форм контроля
    }
}