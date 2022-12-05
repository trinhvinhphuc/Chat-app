using Communicator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chat_app_Client
{
    public partial class ImageView : Form
    {
        private BufferFile bufferFile;

        public ImageView(BufferFile bufferFile)
        {
            this.bufferFile = bufferFile;
            InitializeComponent();
            this.Text = bufferFile.receiver + " - Picture from " + bufferFile.sender;
        }

        private void ImageView_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromStream(new MemoryStream(bufferFile.buffer));
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            string fileName = @Environment.CurrentDirectory + "/" + String.Format("{0:yyyy-MM-dd HH-mm-ss}__{1}", DateTime.Now, bufferFile.sender) + bufferFile.extension;
            FileInfo fi = new FileInfo(fileName);

            try
            {
                // Check if file already exists. If yes, delete it.     
                if (fi.Exists)
                {
                    fi.Delete();
                }

                using (FileStream fStream = File.Create(fileName))
                {
                    fStream.Write(bufferFile.buffer, 0, bufferFile.buffer.Length);
                    fStream.Flush();
                    fStream.Close();
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }

            this.Close();
        }
    }
}
