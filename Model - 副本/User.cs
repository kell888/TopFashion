using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopFashion
{
    [Serializable]
    public class User
    {
        string username;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        string password;
        public string Password { get { return password; } set { password = value; } }
    }
}
