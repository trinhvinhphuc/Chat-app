using System.Net;
using System.Net.Sockets;

namespace Chat_app_Server
{
    public partial class Server : Form
    {
        private Dictionary<String, String> USER;
        private Dictionary<String, List<String>> GROUP;
        private Dictionary<String, Socket> CLIENT;

        public Server()
        {
            InitializeComponent();
        }

        private void Server_Load(object sender, EventArgs e)
        {
            String IP = null;
            var host = Dns.GetHostByName(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.ToString().Contains('.'))
                {
                    IP = ip.ToString();
                }
            }
            if (IP == null)
            {
                MessageBox.Show("No network adapters with an IPv4 address in the system!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            txtIP.Text = IP;
            txtPort.Text = "2009";

            userInitialize();
        }

        private void userInitialize()
        {
            USER = new Dictionary<String, String>();
            GROUP = new Dictionary<String, List<String>>();
            CLIENT = new Dictionary<String, Socket>();

            for (char uName = 'A'; uName <= 'Z'; uName++)
            {
                String pass = "123";
                USER.Add(uName.ToString(), pass);
            }

            for (int i = 0; i < 5; i++)
            {
                List<string> groupUser = new List<string>();
                for (byte j = 0; j < 3; j++)
                {
                    char u = (Char)('A' + 3 * i + j);
                    groupUser.Add(u.ToString());
                }
                GROUP.Add("Group" + i.ToString(), groupUser);
            }
        }
    }
}