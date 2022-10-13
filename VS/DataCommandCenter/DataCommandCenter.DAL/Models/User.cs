using System;
using System.Collections.Generic;

namespace DataCommandCenter.DAL.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? PasswordHash { get; set; }
        public string? Email { get; set; }
        public string? UserType { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? CreatedDatetime { get; set; }
        public DateTime? LastLoginDatetime { get; set; }
    }
}
