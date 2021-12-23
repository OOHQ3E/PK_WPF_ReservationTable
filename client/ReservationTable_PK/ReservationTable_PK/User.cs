using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationTable_PK
{
    public class User
    {
        public User() { }

        public User(string name, string password)
        {
            Name = name;
            Password = password;
        }

        private int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

    }
}
