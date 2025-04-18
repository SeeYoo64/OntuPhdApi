namespace OntuPhdApi.Models.Programs.Components
{
    public class ProgramCharacteristics
    {
        public int Id { get; set; }
        public int ProgramId { get; set; }

        public string? Focus { get; set; }
        public string? Features { get; set; }

        public Area Area { get; set; }

        public ProgramModel Program { get; set; }
    }


    public class Area
    {
        public int Id { get; set; }
        public int ProgramCharacteristicsId { get; set; }

        public string? Object { get; set; }
        public string? Aim { get; set; }
        public string? Theory { get; set; }
        public string? Methods { get; set; }
        public string? Instruments { get; set; }

        public ProgramCharacteristics ProgramCharacteristics { get; set; }
    }
}
