namespace Communicator
{
    public class Startup
    {
        public String onlUser { get; set; }
        public String group { get; set; }
        public Startup(String onlUser, String group)
        {
            this.onlUser = onlUser;
            this.group = group;
        }
    }
}
