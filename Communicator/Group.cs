using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communicator
{
    internal class Group
    {
        public String name { get; set; }
        public String members { get; set; }
        public Group(string name, string members)
        {
            this.name = name;
            this.members = members;
        }
    }
}
