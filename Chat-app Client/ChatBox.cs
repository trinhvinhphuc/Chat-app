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
using static System.Net.Mime.MediaTypeNames;

namespace Chat_app_Client
{
    public partial class ChatBox : Form
    {
        private TcpClient server;
        private String name;
        private StreamReader streamReader;
        private StreamWriter streamWriter;

        public ChatBox(TcpClient server, String name)
        {
            this.server = server;
            this.name = name;
            InitializeComponent();
        }

        private void ChatBox_Load(object sender, EventArgs e)
        {
            streamReader = new StreamReader(server.GetStream());
            streamWriter = new StreamWriter(server.GetStream());

            lblWelcome.Text = "Welcome, " + name;

            var mainThread = new Thread(() => receiveTheard());
            mainThread.Start();
            mainThread.IsBackground = true;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (txtMessage.Text == "" || txtReceiver.Text == "")
            {
                MessageBox.Show("Empty Fields", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Messages messages = new Messages(this.name, txtReceiver.Text, txtMessage.Text);
            String messageJson = JsonSerializer.Serialize(messages);
            Json json = new Json("MESSAGE", messageJson);
            sendJson(json);
        }

        private void receiveTheard()
        {
            Json request = new Json("STARTUP", name);
            sendJson(request);

            bool threadActive = true;
            while(server != null && threadActive)
            {
                try
                {
                    String jsonString = streamReader.ReadLine();
                    Json? infoJson = JsonSerializer.Deserialize<Json?>(jsonString);

                    switch (infoJson.type)
                    {
                        case "STARTUP_FEEDBACK":
                            //AppendRichTextBox(infoJson.content);
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
                        case "MESSAGE":
                            if (infoJson.content != null)
                            {
                                Messages message = JsonSerializer.Deserialize<Messages?>(infoJson.content);
                                if (message != null)
                                {
                                    AppendRichTextBox(message.sender, message.message, "Download here");
                                }
                            }
                            break;
                    }

                }
                catch
                {
                    threadActive = false;
                }
            }
        }

        private void AppendRichTextBox(string name, string message, string link)
        {
            rtbDialog.BeginInvoke(new MethodInvoker(() =>
            {
                Font currentFont = rtbDialog.SelectionFont;
                //Username
                rtbDialog.SelectionStart = rtbDialog.TextLength;
                rtbDialog.SelectionLength = 0;

                rtbDialog.SelectionColor = Color.Red;
                rtbDialog.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, FontStyle.Bold);
                rtbDialog.AppendText(name);
                rtbDialog.SelectionColor = rtbDialog.ForeColor;

                rtbDialog.AppendText(": ");

                //Message
                rtbDialog.SelectionStart = rtbDialog.TextLength;
                rtbDialog.SelectionLength = 0;

                rtbDialog.SelectionColor = Color.Green;
                rtbDialog.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, FontStyle.Regular);
                rtbDialog.AppendText(message);
                rtbDialog.SelectionColor = rtbDialog.ForeColor;

                //link
                rtbDialog.SelectionStart = rtbDialog.TextLength;
                rtbDialog.SelectionLength = 0;

                rtbDialog.SelectionColor = Color.Blue;
                rtbDialog.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, FontStyle.Underline);
                rtbDialog.AppendText(" " + link);
                rtbDialog.SelectionColor = rtbDialog.ForeColor;

                rtbDialog.AppendText(Environment.NewLine);
            }));
        }

        private void sendJson(Json json)
        {
            byte[] jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(json);
            String S = Encoding.ASCII.GetString(jsonUtf8Bytes, 0, jsonUtf8Bytes.Length);

            streamWriter.WriteLine(S);
            streamWriter.Flush();
        }

        private void addDataInDataGridView(DataGridView dataGridView, String[] array)
        {
            dataGridView.Invoke(new Action(() => { dataGridView.Rows.Add(array); }));
        }

        private void tblGroup_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtReceiver.Text = tblGroup.Rows[e.RowIndex].Cells[0].Value.ToString();
        }

        private void txtMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == 13)
            {
                btnSend_Click(this.btnSend, e);
            }
        }
    }
}
