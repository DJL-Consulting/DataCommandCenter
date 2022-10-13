using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCommandCenter.DAL.DTO
{
    public class DCCuser
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserType { get; set; }

        public DCCuser(
            int userId,
            string userName,
            string email,
            string firstName,
            string lastName,
            string userType)
        {
            UserId = userId;
            Email = email;
            FirstName = firstName;
            UserName = userName;
            LastName = lastName;
            UserType = userType;
        }
    }
}
