namespace Chat_app_Client
{
    partial class ChatBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tblUser = new System.Windows.Forms.DataGridView();
            this.Online = new System.Windows.Forms.DataGridViewButtonColumn();
            this.tblGroup = new System.Windows.Forms.DataGridView();
            this.Group = new System.Windows.Forms.DataGridViewButtonColumn();
            this.rtbDialog = new System.Windows.Forms.RichTextBox();
            this.btnPicture = new System.Windows.Forms.PictureBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.PictureBox();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.txtReceiver = new System.Windows.Forms.TextBox();
            this.btnCreateGroup = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.tblUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSend)).BeginInit();
            this.SuspendLayout();
            // 
            // tblUser
            // 
            this.tblUser.AllowUserToAddRows = false;
            this.tblUser.AllowUserToDeleteRows = false;
            this.tblUser.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tblUser.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Online});
            this.tblUser.Location = new System.Drawing.Point(12, 43);
            this.tblUser.Name = "tblUser";
            this.tblUser.ReadOnly = true;
            this.tblUser.RowTemplate.Height = 25;
            this.tblUser.Size = new System.Drawing.Size(152, 201);
            this.tblUser.TabIndex = 4;
            this.tblUser.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tblUser_CellContentClick);
            // 
            // Online
            // 
            this.Online.HeaderText = "Online";
            this.Online.Name = "Online";
            this.Online.ReadOnly = true;
            this.Online.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Online.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Online.Width = 120;
            // 
            // tblGroup
            // 
            this.tblGroup.AllowUserToAddRows = false;
            this.tblGroup.AllowUserToDeleteRows = false;
            this.tblGroup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tblGroup.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Group});
            this.tblGroup.Location = new System.Drawing.Point(12, 250);
            this.tblGroup.Name = "tblGroup";
            this.tblGroup.ReadOnly = true;
            this.tblGroup.RowTemplate.Height = 25;
            this.tblGroup.Size = new System.Drawing.Size(152, 201);
            this.tblGroup.TabIndex = 3;
            this.tblGroup.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tblGroup_CellContentClick);
            // 
            // Group
            // 
            this.Group.HeaderText = "Group";
            this.Group.Name = "Group";
            this.Group.ReadOnly = true;
            this.Group.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Group.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Group.Width = 120;
            // 
            // rtbDialog
            // 
            this.rtbDialog.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.rtbDialog.Location = new System.Drawing.Point(179, 43);
            this.rtbDialog.Name = "rtbDialog";
            this.rtbDialog.Size = new System.Drawing.Size(609, 408);
            this.rtbDialog.TabIndex = 2;
            this.rtbDialog.Text = "";
            // 
            // btnPicture
            // 
            this.btnPicture.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPicture.Image = global::Chat_app_Client.Properties.Resources.file;
            this.btnPicture.Location = new System.Drawing.Point(179, 457);
            this.btnPicture.Name = "btnPicture";
            this.btnPicture.Size = new System.Drawing.Size(30, 30);
            this.btnPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnPicture.TabIndex = 2;
            this.btnPicture.TabStop = false;
            this.btnPicture.Click += new System.EventHandler(this.btnPicture_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtMessage.Location = new System.Drawing.Point(215, 458);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(538, 29);
            this.txtMessage.TabIndex = 1;
            this.txtMessage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMessage_KeyPress);
            // 
            // btnSend
            // 
            this.btnSend.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSend.Image = global::Chat_app_Client.Properties.Resources.send;
            this.btnSend.Location = new System.Drawing.Point(759, 457);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(30, 30);
            this.btnSend.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnSend.TabIndex = 2;
            this.btnSend.TabStop = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Times New Roman", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.lblWelcome.ForeColor = System.Drawing.Color.Maroon;
            this.lblWelcome.Location = new System.Drawing.Point(12, 9);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(106, 22);
            this.lblWelcome.TabIndex = 4;
            this.lblWelcome.Text = "Welcome, ...";
            // 
            // txtReceiver
            // 
            this.txtReceiver.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtReceiver.Location = new System.Drawing.Point(179, 7);
            this.txtReceiver.Name = "txtReceiver";
            this.txtReceiver.Size = new System.Drawing.Size(111, 29);
            this.txtReceiver.TabIndex = 0;
            // 
            // btnCreateGroup
            // 
            this.btnCreateGroup.BackColor = System.Drawing.Color.RosyBrown;
            this.btnCreateGroup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateGroup.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnCreateGroup.ForeColor = System.Drawing.Color.Maroon;
            this.btnCreateGroup.Location = new System.Drawing.Point(12, 458);
            this.btnCreateGroup.Name = "btnCreateGroup";
            this.btnCreateGroup.Size = new System.Drawing.Size(152, 29);
            this.btnCreateGroup.TabIndex = 5;
            this.btnCreateGroup.Text = "Create Group";
            this.btnCreateGroup.UseVisualStyleBackColor = false;
            this.btnCreateGroup.Click += new System.EventHandler(this.btnCreateGroup_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.DarkRed;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button1.ForeColor = System.Drawing.Color.LightCoral;
            this.button1.Location = new System.Drawing.Point(701, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 29);
            this.button1.TabIndex = 29;
            this.button1.Text = "Logout";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ChatBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 497);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnCreateGroup);
            this.Controls.Add(this.lblWelcome);
            this.Controls.Add(this.txtReceiver);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnPicture);
            this.Controls.Add(this.rtbDialog);
            this.Controls.Add(this.tblGroup);
            this.Controls.Add(this.tblUser);
            this.Name = "ChatBox";
            this.Text = "ChatBox";
            this.Load += new System.EventHandler(this.ChatBox_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tblUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSend)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridView tblUser;
        private DataGridView tblGroup;
        private RichTextBox rtbDialog;
        private PictureBox btnPicture;
        private TextBox txtMessage;
        private PictureBox btnSend;
        private Label lblWelcome;
        private TextBox txtReceiver;
        private Button btnCreateGroup;
        private Button button1;
        private DataGridViewButtonColumn Online;
        private DataGridViewButtonColumn Group;
    }
}