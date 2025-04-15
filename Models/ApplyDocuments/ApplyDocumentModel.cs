using System.Text.Json.Serialization;

namespace OntuPhdApi.Models.ApplyDocuments
{
    public class ApplyDocumentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Requirements> Requirements { get; set; } 
        public List<Requirements> OriginalsRequired { get; set; } 
    }

    public class Requirements
    {
        [JsonPropertyName("Title")]
        public string Title { get; set; }
        [JsonPropertyName("Description")]
        public string Description { get; set; }
    }


    public class ApplyDocumentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<RequirementDto> Requirements { get; set; }
        public List<RequirementDto> OriginalsRequired { get; set; }
    }

    public class RequirementDto
    {
        [JsonPropertyName("Title")]
        public string Title { get; set; }
        [JsonPropertyName("Description")]
        public string Description { get; set; }
    }

    public class ApplyDocumentCreateUpdateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<RequirementDto> Requirements { get; set; }
        public List<RequirementDto> OriginalsRequired { get; set; }
    }
}