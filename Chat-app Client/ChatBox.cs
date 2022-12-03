using Communicator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.LinkLabel;
using Application = System.Windows.Forms.Application;

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

            txtMessage.Clear();
        }

        private void txtMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == 13)
            {
                btnSend_Click(this.btnSend, e);
            }
        }

        private void tblGroup_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtReceiver.Text = tblGroup.Rows[e.RowIndex].Cells[0].Value.ToString();
        }

        private void tblUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtReceiver.Text = tblUser.Rows[e.RowIndex].Cells["Online"].Value.ToString();
        }

        private void btnCreateGroup_Click(object sender, EventArgs e)
        {
            new Thread(() => Application.Run(new GroupCreator(server))).Start();
        }

        private void btnPicture_Click(object sender, EventArgs e)
        {
            Thread dialogThread = new Thread(() =>
            {
                try
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    //ofd.Filter = "jpg files(*.jpg)|*.jpg| PNG files(*.png)|*.png| All files(*.*)|*.*";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        //if (Path.GetExtension(ofd.FileName) == ".txt")
                        //{
                            AppendRichTextBox(ofd.FileName, "textfile", "Download here");

                            String fName = ofd.FileName;
                            String path = "";
                            fName = fName.Replace("\\", "/");
                            while (fName.IndexOf("/") > -1)
                            {
                                path += fName.Substring(0, fName.IndexOf("/") + 1);
                                fName = fName.Substring(fName.IndexOf("/") + 1);
                            }
                            String s = JsonSerializer.Serialize(File.ReadAllText(path+fName));

                            AppendRichTextBox(s, "textfile", "Download here");

                            FileMessage message = new FileMessage(this.name, txtReceiver.Text, s, Path.GetExtension(ofd.FileName));

                            Json json = new Json("FILE", JsonSerializer.Serialize(message));
                            sendJson(json);
                        //}
                    //sendFile(ofd.FileName);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("An Error Occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
            dialogThread.SetApartmentState(ApartmentState.STA);
            dialogThread.Start();
            dialogThread.IsBackground = true;
        }

        private void receiveTheard()
        {
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
                            cleanDataGridView(tblGroup);
                            cleanDataGridView(tblUser);

                            Startup startup = JsonSerializer.Deserialize<Startup>(infoJson.content);

                            List<string> groups = JsonSerializer.Deserialize<List<String>>(startup.group);
                            foreach(String group in groups)
                            {
                                addDataInDataGridView(tblGroup, new string[] { group });
                            }

                            List<string> users = JsonSerializer.Deserialize<List<String>>(startup.onlUser);
                            foreach (String user in users)
                            {
                                addDataInDataGridView(tblUser, new string[] { user });
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

        private void cleanDataGridView(DataGridView dataGridView)
        {
            dataGridView.BeginInvoke(new MethodInvoker(() =>
            {
                dataGridView.Rows.Clear();
            }));
            
        }

        private void addDataInDataGridView(DataGridView dataGridView, String[] array)
        {
            dataGridView.Invoke(new Action(() => { dataGridView.Rows.Add(array); }));
        }

        private void sendJson(Json json)
        {
            byte[] jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(json);
            String S = Encoding.ASCII.GetString(jsonUtf8Bytes, 0, jsonUtf8Bytes.Length);

            streamWriter.WriteLine(S);
            streamWriter.Flush();
        }

        private void sendFile(String fName)
        {
            try
            {
                String path = "";
                fName = fName.Replace("\\", "/");
                while (fName.IndexOf("/") > -1)
                {
                    path += fName.Substring(0, fName.IndexOf("/") + 1);
                    fName = fName.Substring(fName.IndexOf("/") + 1);
                }

                byte[] fNameByte = Encoding.ASCII.GetBytes(fName);
                byte[] fileData = File.ReadAllBytes(path + fName);

                byte[] clientData = new byte[4 + fNameByte.Length + fileData.Length];
                byte[] fNameLen = BitConverter.GetBytes(fNameByte.Length);

                fNameLen.CopyTo(clientData, 0);
                fNameByte.CopyTo(clientData, 4 + fName.Length);

                streamWriter.Write(System.Convert.ToBase64String(File.ReadAllBytes(path + fName)));
                //AppendRichTextBox(path + fName, "", "");
            }
            catch (Exception e)
            {

            }
        }
    }
}
