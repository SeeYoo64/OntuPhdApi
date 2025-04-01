namespace OntuPhdApi.Models.Programs
{
    public class ProgramCharacteristics
    {
        public Area Area { get; set; }
        public string Focus { get; set; }
        public List<string> Features { get; set; }
    }

    public class Area
    {
        public string Object { get; set; }
        public string Aim { get; set; }
        public string Theory { get; set; }
        public string Methods { get; set; }
        public string Instruments { get; set; }
    }

}