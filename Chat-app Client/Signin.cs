using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Communicator;
using System.Text.Json;

namespace Chat_app_Client
{
    public partial class Signin : Form
    {
        private bool active = true;
        private IPEndPoint ipe;
        private TcpClient server;
        private StreamReader streamReader;
        private StreamWriter streamWriter;

        public Signin()
        {
            InitializeComponent();
        }

        private void btnSignin_Click(object sender, EventArgs e)
        {
            if (txtSigninIP.Text == "" || txtSigninPassword.Text == "" || txtSigninUsername.Text == "")
            {
                MessageBox.Show("Empty Fields", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ipe = new IPEndPoint(IPAddress.Parse(txtSigninIP.Text), 2009);
            server = new TcpClient();

            server.Connect(ipe);

            streamReader = new StreamReader(server.GetStream());
            streamWriter = new StreamWriter(server.GetStream());

            var threadSign = new Thread(new ThreadStart(waitForSigninFeedback));
            threadSign.IsBackground = true;
            threadSign.Start();
        }

        private void waitForSigninFeedback()
        {
            Account account = new Account(txtSigninUsername.Text, txtSigninPassword.Text);
            String accountJson = JsonSerializer.Serialize(account);
            Json json = new Json("SIGNIN", accountJson);

            sendJson(json);

            accountJson = streamReader.ReadLine();
            Json? feedback = JsonSerializer.Deserialize<Json?>(accountJson);

            try
            {
                if (feedback != null)
                {
                    switch (feedback.type)
                    {
                        case "SIGNIN_FEEDBACK":
                            if (feedback.content == "TRUE")
                            {
                                new Thread(() => Application.Run(new ChatBox(server, account.userName))).Start();
                                this.Invoke((MethodInvoker)delegate
                                {
                                    this.Close();
                                });
                                break;
                            }
                            if (feedback.content == "FALSE")
                            {
                                MessageBox.Show("Sign in failed!!", "Notification");
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

        private void sendJson(Json json)
        {
            byte[] jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(json);
            String S = Encoding.ASCII.GetString(jsonUtf8Bytes, 0, jsonUtf8Bytes.Length);
            streamWriter.WriteLine(S);
            streamWriter.Flush();
        }

        private void lblSignin_Click(object sender, EventArgs e)
        {
            new Thread(() => Application.Run(new Login())).Start();
            this.Close();
        }
    }
}
