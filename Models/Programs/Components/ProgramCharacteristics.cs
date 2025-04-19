using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
        [Key]
        public int ProgramCharacteristicsId { get; set; } // PK + FK
        public string? Object { get; set; }
        public string? Aim { get; set; }
        public string? Theory { get; set; }
        public string? Methods { get; set; }
        public string? Instruments { get; set; }

        [ForeignKey(nameof(ProgramCharacteristicsId))]
        public ProgramCharacteristics ProgramCharacteristics { get; set; }
    }
}
