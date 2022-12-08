using Communicator;
using Microsoft.VisualBasic.ApplicationServices;
using System.IO;
using System.Net;
using System.Net.Http;
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
        private TcpListener server;
        private Dictionary<String, String> USER;
        private Dictionary<String, List<String>> GROUP;
        private Dictionary<String, TcpClient> CLIENT;

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
            CLIENT = new Dictionary<String, TcpClient>();

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
                if (!groupUser.Contains("A"))
                {
                    groupUser.Add("A");
                }
                GROUP.Add("Group " + i.ToString(), groupUser);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            iep = new IPEndPoint(IPAddress.Parse(txtIP.Text), int.Parse(txtPort.Text));
            server = new TcpListener(iep);
            server.Start();

            Thread ServerThread = new Thread(new ThreadStart(ServerStart));
            ServerThread.IsBackground = true;
            ServerThread.Start();
        }

        private void ServerStart()
        {
            try
            {
                AppendRichTextBox("Start accept connect from client!");
                changeButtonEnable(btnStart, false);
                changeButtonEnable(btnStop, true);
                //Clipboard.SetText(txtIP.Text);
                while (active)
                {
                    TcpClient client = server.AcceptTcpClient();
                    var clientThread = new Thread(() => clientService(client));
                    clientThread.Start();
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void clientService(TcpClient client)
        {
            StreamReader streamReader = new StreamReader(client.GetStream());
            String s = streamReader.ReadLine();
            Json infoJson = JsonSerializer.Deserialize<Json>(s);

            if (infoJson != null)
            {
                switch (infoJson.type)
                {
                    case "SIGNIN":
                        reponseSignin(infoJson, client);
                        break;
                    case "LOGIN":
                        reponseLogin(infoJson, client);
                        break;
                }
            }

            try
            {
                bool threadActive = true;
                while (threadActive && client != null)
                {
                    s = streamReader.ReadLine();
                    infoJson = JsonSerializer.Deserialize<Json>(s);
                    if (infoJson != null && infoJson.content != null)
                    {
                        switch (infoJson.type)
                        {
                            case "MESSAGE":
                                if (infoJson.content != null)
                                {
                                    reponseMessage(infoJson);
                                }
                                break;
                            case "CREATE_GROUP":
                                if (infoJson.content != null)
                                {
                                    createGroup(infoJson);
                                }
                                break;
                            case "FILE":
                                if (infoJson.content != null)
                                {
                                    reponseFile(infoJson, client);
                                }
                                break;
                            case "LOGOUT":
                                if (infoJson.content != null)
                                {
                                    CLIENT[infoJson.content].Close();
                                    CLIENT.Remove(infoJson.content);
                                    AppendRichTextBox(infoJson.content + "logged out.");
                                    threadActive = false;
                                }
                                break;
                        }
                    }
                }
            }
            catch
            {
                //client.Close();
            }
        }

        private void reponseSignin(Json infoJson, TcpClient client)
        {
            Account account = JsonSerializer.Deserialize<Account>(infoJson.content);

            if (account != null && account.userName != null && !USER.ContainsKey(account.userName) && !CLIENT.ContainsKey(account.userName))
            {
                Json notification = new Json("SIGNIN_FEEDBACK", "TRUE");
                sendJson(notification, client);
                AppendRichTextBox(account.userName + " signed in!");

                USER.Remove(account.userName);
                USER.Add(account.userName, account.password);

                CLIENT.Remove(account.userName);
                CLIENT.Add(account.userName, client);

                foreach (String key in CLIENT.Keys)
                {
                    startupClient(CLIENT[key], key);
                }
            }
        }

        private void reponseLogin(Json infoJson, TcpClient client)
        {
            Account account = JsonSerializer.Deserialize<Account>(infoJson.content);
            if (account != null && account.userName != null && USER.ContainsKey(account.userName) && !CLIENT.ContainsKey(account.userName) && USER[account.userName] == account.password)
            {
                Json notification = new Json("LOGIN_FEEDBACK", "TRUE");
                sendJson(notification, client);
                AppendRichTextBox(account.userName + " logged in!");

                CLIENT.Remove(account.userName);
                CLIENT.Add(account.userName, client);
                
                foreach(String key in CLIENT.Keys)
                {
                    startupClient(CLIENT[key], key);
                }
            }
            else
            {
                Json notification = new Json("LOGIN_FEEDBACK", "FALSE");
                sendJson(notification, client);
                AppendRichTextBox(account.userName + " can not login!");
            }
        }

        private void startupClient(TcpClient client, String name)
        {
            List<String> onlUser = new List<string>(CLIENT.Keys);
            onlUser.Remove(name);

            List<String> group = new List<string>();
            foreach (String key in GROUP.Keys)
            {
                if (GROUP[key].Contains(name))
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

        private void reponseMessage(Json infoJson)
        {
            Messages messages = JsonSerializer.Deserialize<Messages>(infoJson.content);
            if (messages != null && CLIENT.ContainsKey(messages.receiver))
            {
                AppendRichTextBox(messages.sender + " to " + messages.receiver + ": " + messages.message);

                TcpClient receiver = CLIENT[messages.receiver];
                sendJson(infoJson, receiver);
                receiver = CLIENT[messages.sender];
                sendJson(infoJson, receiver);
            }
            else
            {
                if (GROUP.ContainsKey(messages.receiver))
                {
                    AppendRichTextBox(messages.sender + " to " + messages.receiver + ": " + messages.message);
                    foreach (String user in GROUP[messages.receiver])
                    {
                        if (CLIENT.ContainsKey(user))
                        {
                            TcpClient receiver = CLIENT[user];
                            sendJson(infoJson, receiver);
                        }
                    }
                }
            }
        }

        private void createGroup(Json infoJson)
        {
            List<string> groupUser = new List<string>();
            Group group = JsonSerializer.Deserialize<Group>(infoJson.content);

            string[] values = group.members.Split(',');

            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Trim();
                groupUser.Add(values[i]);
            }

            GROUP.Add(group.name, groupUser);

            foreach (String key in CLIENT.Keys)
            {
                startupClient(CLIENT[key], key);
            }
        }

        private void reponseFile(Json infoJson, TcpClient client)
        {
            FileMessage fileMessage = JsonSerializer.Deserialize<FileMessage>(infoJson.content);

            try
            {

                int length = Convert.ToInt32(fileMessage.lenght);
                byte[] buffer = new byte[length];
                int received = 0;
                int read = 0;
                int size = 1024;
                int remaining = 0;

                // Read bytes from the client using the length sent from the client    
                while (received < length)
                {
                    remaining = length - received;
                    if (remaining < size)
                    {
                        size = remaining;
                    }

                    read = client.GetStream().Read(buffer, received, size);
                    received += read;
                }

                BufferFile bufferFile = new BufferFile(fileMessage.sender, fileMessage.receiver, buffer, fileMessage.extension);

                String jsonString = JsonSerializer.Serialize(bufferFile);
                Json json = new Json("FILE", jsonString);

                if (CLIENT.ContainsKey(fileMessage.receiver))
                {
                    TcpClient receiver = CLIENT[fileMessage.receiver];
                    sendJson(json, receiver);
                }

                else
                {
                    if (GROUP.ContainsKey(fileMessage.receiver))
                    {
                        foreach (String user in GROUP[fileMessage.receiver])
                        {
                            if (CLIENT.ContainsKey(user))
                            {
                                TcpClient receiver = CLIENT[user];
                                sendJson(json, receiver);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }

        private void sendJson(Json json, TcpClient client)
        {
            byte[] jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(json);
            StreamWriter streamWriter = new StreamWriter(client.GetStream());

            String S = Encoding.ASCII.GetString(jsonUtf8Bytes, 0, jsonUtf8Bytes.Length);

            streamWriter.WriteLine(S);
            streamWriter.Flush();
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

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (CLIENT.Count() > 0)
            {
                MessageBox.Show("The server has " + CLIENT.Count + " user(s) logged in.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            active = false;
            Environment.Exit(0);
        }
    }
}