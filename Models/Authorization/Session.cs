﻿namespace OntuPhdApi.Models.Authorization
{
    public class Session
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public DateTime Expires { get; set; }
        public string SessionToken { get; set; } = null!;
    }
}
