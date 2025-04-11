namespace OntuPhdApi.Models.Authorization
{
    public class VerificationToken
    {
        public string Identifier { get; set; } = null!;
        public DateTime Expires { get; set; }
        public string Token { get; set; } = null!;
    }
}
