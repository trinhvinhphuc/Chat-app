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
        private IPEndPoint ipe;
        private TcpClient server;
        private StreamReader streamReader;
        private StreamWriter streamWriter;
        private String name;

        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtLoginPassword.Text == "" || txtLoginIP.Text == "" || txtLoginUsername.Text == "")
            {
                MessageBox.Show("Empty Fields", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ipe = new IPEndPoint(IPAddress.Parse(txtLoginIP.Text), 2009);
                server = new TcpClient();

                server.Connect(ipe);

                name = txtLoginUsername.Text;
                streamReader = new StreamReader(server.GetStream());
                streamWriter = new StreamWriter(server.GetStream());

                var threadLog = new Thread(new ThreadStart(waitForLoginFeedback));
                threadLog.IsBackground = true;
                threadLog.Start();
            }
            catch
            {
                MessageBox.Show("Cannot connect to server", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
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
                                new Thread(() => Application.Run(new ChatBox(server, this.name))).Start();
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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

        private void lblSignin_Click(object sender, EventArgs e)
        {
            new Thread(() => Application.Run(new Signin())).Start();
            this.Close();
        }
    }
}