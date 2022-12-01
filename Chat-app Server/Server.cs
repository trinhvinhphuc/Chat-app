using Communicator;
using Microsoft.VisualBasic.ApplicationServices;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;
using System.Xml;

namespace Chat_app_Server
{
    public partial class Server : Form
    {
        private bool active = true;
        private IPEndPoint iep;
        private Socket server, client;
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
                groupUser.Add("A");
                GROUP.Add("Group " + i.ToString(), groupUser);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Thread ServerThread = new Thread(new ThreadStart(ServerStart));
            ServerThread.Start();
            ServerThread.IsBackground = true;
        }

        private void ServerStart()
        {
            iep = new IPEndPoint(IPAddress.Parse(txtIP.Text), Int32.Parse(txtPort.Text));
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                server.Bind(iep);
                server.Listen(100);

                AppendRichTextBox("Start accept connect from client!");
                changeButtonEnable(btnStart, false);
                changeButtonEnable(btnStop, true);
                //Clipboard.SetText(txtIP.Text);
                while (active)
                {
                    client = server.Accept();
                    byte[] data = new byte[1024];
                    int recv = client.Receive(data);
                    if (recv == 0) continue;
                    String s = Encoding.ASCII.GetString(data, 0, recv);

                    Json infoJson = JsonSerializer.Deserialize<Json>(s);

                    if (infoJson != null)
                    {
                        switch (infoJson.type)
                        {
                            case "SIGNIN":
                                //reponseSignin(infoJson, client);
                                break;
                            case "LOGIN":
                                reponseLogin(infoJson, client);
                                //AppendRichTextBox(infoJson.content);
                                break;
                        }
                    }

                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void reponseLogin(Json infoJson, Socket client)
        {
            Account account = JsonSerializer.Deserialize<Account>(infoJson.content);
            if (account != null && account.userName != null && USER.ContainsKey(account.userName) && !CLIENT.ContainsKey(account.userName) && USER[account.userName] == account.password)
            {
                Json notification = new Json("LOGIN_FEEDBACK", "TRUE");
                sendJson(notification, client);
                AppendRichTextBox(account.userName + " logged in!");

                CLIENT.Add(account.userName, client);
                clientService(client);
            }
            else
            {
                Json notification = new Json("LOGIN_FEEDBACK", "FALSE");
                sendJson(notification, client);
                AppendRichTextBox(account.userName + " can not login!");
            }
        }

        private void clientService(Socket client)
        {
            bool threadActive = true;
            Thread clientThread = new Thread(() =>
            {
                while (threadActive && client != null)
                {
                    try
                    {
                        byte[] data = new byte[1024];
                        int recv = client.Receive(data);
                        if (recv == 0) continue;
                        String s = Encoding.ASCII.GetString(data, 0, recv);
                        Json infoJson = JsonSerializer.Deserialize<Json>(s);

                        switch (infoJson.type)
                        {
                            case "STARTUP":
                                if (infoJson.content != null)
                                {
                                    List<String> onlUser = new List<string>(CLIENT.Keys);
                                    onlUser.Remove(infoJson.content);

                                    List<String> group = new List<string>();
                                    foreach(String key in GROUP.Keys)
                                    {
                                        if (GROUP[key].Contains(infoJson.content))
                                        {
                                            group.Add(key);
                                        }
                                    }

                                    string jsonUser = JsonSerializer.Serialize<List<String>>(onlUser);
                                    string jsonGroup = JsonSerializer.Serialize<List<String>>(group);

                                    Startup startup = new Startup(jsonUser, jsonGroup);
                                    String startupJson = JsonSerializer.Serialize(startup);
                                    Json json = new Json("STARTUP_FEEDBACK", startupJson);
                                    sendJson(json, client);
                                }
                                break;
                        }
                    }
                    catch
                    {
                        threadActive = false;
                    }                   
                }
            });
            clientThread.Start();
            clientThread.IsBackground = true;
        }

        private void sendJson(Json json, Socket server)
        {
            String message = JsonSerializer.Serialize(json);
            byte[] data = new byte[1024];
            data = Encoding.ASCII.GetBytes(message);

            server.Send(data, data.Length, SocketFlags.None);
        }

        private void AppendRichTextBox(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendRichTextBox), new object[] { value });
                return;
            }
            rtbDialog.AppendText(value);
            rtbDialog.AppendText(Environment.NewLine);
        }

        private void changeButtonEnable(Button btn, bool enable)
        {
            btn.BeginInvoke(new MethodInvoker(() =>
            {
                btn.Enabled = enable;
            }));
        }
    }
}