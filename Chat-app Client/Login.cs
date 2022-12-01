using System.Net.Sockets;
using System.Net;
using System.Security.Principal;
using System.Text.Json;
using Communicator;
using System.Text;

namespace Chat_app_Client
{
    public partial class Login : Form
    {
        private bool active = true;
        private IPEndPoint ipe;
        private Socket server;
        private Form form;

        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            ipe = new IPEndPoint(IPAddress.Parse(txtLoginIP.Text), 2009);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Connect(ipe);

            Account account = new Account(txtLoginUsername.Text, txtLoginPassword.Text);

            String loginJson = JsonSerializer.Serialize(account);
            Json json = new Json("LOGIN", loginJson);
            sendJson(json, server);
            var threadLog = new Thread(new ThreadStart(waitForLoginFeedback));
            threadLog.Start();
        }

        private void waitForLoginFeedback()
        {
            while (active && server != null)
            {
                byte[] data = new byte[1024];
                int recv = 0;

                try
                {
                    recv = server.Receive(data);
                }
                catch
                {
                    active = false;
                    this.Close();
                }
                if (recv == 0) continue;

                String feedbackJson = Encoding.ASCII.GetString(data, 0, recv);
                Json? feedback = JsonSerializer.Deserialize<Json?>(feedbackJson);

                if (feedback != null)
                {
                    switch (feedback.type)
                    {
                        case "LOGIN_FEEDBACK":
                            if (feedback.content == "TRUE")
                            {
                                //MessageBox.Show("Login successes!!", "Notification");

                                //this.Invoke((MethodInvoker)delegate () {
                                    Application.Run(new ChatBox(server, txtLoginUsername.Text));
                                //});
                                //this.Invoke(new MethodInvoker(this.Close));
                                this.Close();
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
        }

        private void sendJson(Json json, Socket server)
        {
            String message = JsonSerializer.Serialize(json);
            byte[] data = new byte[1024];
            data = Encoding.ASCII.GetBytes(message);

            server.Send(data, data.Length, SocketFlags.None);
        }

        private void lblSignin_Click(object sender, EventArgs e)
        {
            form = new Signin();
            form.Show();
            this.Close();
        }
    }
}