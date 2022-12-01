namespace Communicator
{
    public class Group
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
