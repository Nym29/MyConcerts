using System;
using SQLite;

namespace MyConcerts.Models
{
    [Table("Users")]
    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string Picture { get; set; }

        public User()
        {
        }
    }
}
