using System;
using System.Collections.Generic;
using System.Text;

namespace ModelKlasser
{
    public class User
    {
        public User()
        {
            
        }

        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
    }
}
