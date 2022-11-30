using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communicator
{
    internal class Account
    {
        public string userName { get; set; }
        public string password { get; set; }
        public Account(string userName, string password)
        {
            this.userName = userName;
            this.password = password;
        }
    }
}
