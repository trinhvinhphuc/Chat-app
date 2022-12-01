using Communicator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chat_app_Client
{
    public partial class ChatBox : Form
    {
        private Socket server;
        private String name;

        public ChatBox(Socket server, String name)
        {
            this.server = server;
            this.name = name;
            InitializeComponent();
        }

        private void ChatBox_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = "Welcome, " + name;
            Json request = new Json("STARTUP", name);
            sendJson(request, server);

            var mainThread = new Thread(() => receiveTheard(server));
            mainThread.Start();
        }

        private void receiveTheard(Socket server)
        {
            while(server != null)
            {
                try
                {
                    byte[] data = new byte[1024];
                    int recv = server.Receive(data);
                    if (recv == 0) continue;
                    String s = Encoding.ASCII.GetString(data, 0, recv);
                    Json? infoJson = JsonSerializer.Deserialize<Json?>(s);

                    switch (infoJson.type)
                    {
                        case "STARTUP_FEEDBACK":
                            AppendRichTextBox(infoJson.content);
                            Startup startup = JsonSerializer.Deserialize<Startup>(infoJson.content);

                            List<string> groups = JsonSerializer.Deserialize<List<String>>(startup.group);
                            foreach(String group in groups)
                            {
                                addDataInDataGridView(tblGroup, new string[] { group });
                            }

                            List<string> users = JsonSerializer.Deserialize<List<String>>(startup.onlUser);
                            foreach (String user in users)
                            {
                                addDataInDataGridView(tblGroup, new string[] { user });
                            }
                            break;
                    }

                }
                catch
                {
                    break;
                }
            }
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

        private void sendJson(Json json, Socket server)
        {
            String message = JsonSerializer.Serialize(json);
            byte[] data = new byte[1024];
            data = Encoding.ASCII.GetBytes(message);

            server.Send(data, data.Length, SocketFlags.None);
        }

        private void addDataInDataGridView(DataGridView dataGridView, String[] array)
        {
            dataGridView.Invoke(new Action(() => { dataGridView.Rows.Add(array); }));
        }

        private void tblGroup_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtReceiver.Text = tblGroup.Rows[e.RowIndex].Cells[0].Value.ToString();
        }
    }
}
