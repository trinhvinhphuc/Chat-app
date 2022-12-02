using System.Net.Sockets;
using System.Net;
using System.Security.Principal;
using System.Text.Json;
using Communicator;
using System.Text;
using System.Windows.Forms;

namespace Chat_app_Client
{
    public partial class Login : Form
    {
        private bool active = true;
        private IPEndPoint ipe;
        private TcpClient server;
        private StreamReader streamReader;
        private StreamWriter streamWriter;

        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            ipe = new IPEndPoint(IPAddress.Parse(txtLoginIP.Text), 2009);           
            server = new TcpClient();

            server.Connect(ipe);

            streamReader = new StreamReader(server.GetStream());
            streamWriter = new StreamWriter(server.GetStream());

            var threadLog = new Thread(new ThreadStart(waitForLoginFeedback));
            threadLog.IsBackground = true;
            threadLog.Start();
        }

        private void waitForLoginFeedback()
        {
            Account account = new Account(txtLoginUsername.Text, txtLoginPassword.Text);
            String accountJson = JsonSerializer.Serialize(account);
            Json json = new Json("LOGIN", accountJson);

            sendJson(json, server);

            accountJson = streamReader.ReadLine();
            Json? feedback = JsonSerializer.Deserialize<Json?>(accountJson);

            try
            {
                if (feedback != null)
                {
                    switch (feedback.type)
                    {
                        case "LOGIN_FEEDBACK":
                            if (feedback.content == "TRUE")
                            {
                                new Thread(() => Application.Run(new ChatBox(server, txtLoginUsername.Text))).Start();
                                this.Invoke((MethodInvoker)delegate
                                {
                                    this.Close();
                                });
                                break;
                            }
                            if (feedback.content == "FALSE")
                            {
                                MessageBox.Show("Login failed!!", "Notification");
                            }
                            break;
                    }
                }
            }
            catch
            {

            }
        }

        private void sendJson(Json json, TcpClient client)
        {
            //String message = JsonSerializer.Serialize(json);
            //byte[] data = new byte[1024];
            //data = Encoding.ASCII.GetBytes(message);
            //server.Send(data, data.Length, SocketFlags.None);

            byte[] jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(json);
            StreamWriter streamWriter = new StreamWriter(client.GetStream());
            String S = Encoding.ASCII.GetString(jsonUtf8Bytes, 0, jsonUtf8Bytes.Length);
            streamWriter.WriteLine(S);
            streamWriter.Flush();
        }

        private void lblSignin_Click(object sender, EventArgs e)
        {
            new Signin().Show();
            this.Close();
        }
    }
}