﻿using System.Security.Principal;

namespace OntuPhdApi.Models.Authorization
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public DateTime? EmailVerified { get; set; }
        public string? Image { get; set; }

        public ICollection<Account> Accounts { get; set; } = new List<Account>();
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
