namespace OntuPhdApi.Models
{
    public class ProgramCharacteristics
    {
        public Area Area { get; set; }
        public string Focus { get; set; }
        public Features Features { get; set; }
    }

    public class Area
    {
        public string Object { get; set; }
        public string Aim { get; set; }
        public string Theory { get; set; }
        public string Methods { get; set; }
        public string Instruments { get; set; }
    }

    public class Features
    {
        public string Feature1 { get; set; }
        public string Feature2 { get; set; }
    }
}