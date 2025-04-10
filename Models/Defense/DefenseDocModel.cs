namespace OntuPhdApi.Models.Defense
{
    public class DefenseDocModel
    {
        public int Id { get; set; }
        public int ProgramId { get; set; }
        public string NameSurname { get; set; }
        public string DefenseName { get; set; }
        public ProgramDefense ProgramInfo { get; set; }
        public DateTime DateOfDefense { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public List<FilesDefense> FilesDefense { get; set; }
        public DateTime DateOfPublication { get; set; }
    }
}
