namespace Client
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ButtonCreate = new System.Windows.Forms.ToolStripButton();
            this.LabelNoOfPlayers = new System.Windows.Forms.ToolStripLabel();
            this.TextBoxNoOfPlayers = new System.Windows.Forms.ToolStripTextBox();
            this.LabelCreatePlayerType = new System.Windows.Forms.ToolStripLabel();
            this.ComboBoxCreatePlayerType = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ButtonJoin = new System.Windows.Forms.ToolStripButton();
            this.LabelAvailableRooms = new System.Windows.Forms.ToolStripLabel();
            this.ComboBoxAvailableRooms = new System.Windows.Forms.ToolStripComboBox();
            this.LabelJoinPlayerType = new System.Windows.Forms.ToolStripLabel();
            this.ComboBoxJoinPlayerType = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ButtonOK = new System.Windows.Forms.ToolStripButton();
            this.textBoxMale = new System.Windows.Forms.TextBox();
            this.textBoxFemale = new System.Windows.Forms.TextBox();
            this.textBoxFood = new System.Windows.Forms.TextBox();
            this.textBoxCountry = new System.Windows.Forms.TextBox();
            this.textBoxAnimal = new System.Windows.Forms.TextBox();
            this.LabelMale = new System.Windows.Forms.Label();
            this.labelFemale = new System.Windows.Forms.Label();
            this.labelFood = new System.Windows.Forms.Label();
            this.labelCountry = new System.Windows.Forms.Label();
            this.labelAnimal = new System.Windows.Forms.Label();
            this.buttonAutobusComplete = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.LabelConnectionStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.LabelRoomNo = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ButtonCreate,
            this.LabelNoOfPlayers,
            this.TextBoxNoOfPlayers,
            this.LabelCreatePlayerType,
            this.ComboBoxCreatePlayerType,
            this.toolStripSeparator1,
            this.ButtonJoin,
            this.LabelAvailableRooms,
            this.ComboBoxAvailableRooms,
            this.LabelJoinPlayerType,
            this.ComboBoxJoinPlayerType,
            this.toolStripSeparator2,
            this.ButtonOK});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(716, 26);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // ButtonCreate
            // 
            this.ButtonCreate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonCreate.Image = ((System.Drawing.Image)(resources.GetObject("ButtonCreate.Image")));
            this.ButtonCreate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonCreate.Name = "ButtonCreate";
            this.ButtonCreate.Size = new System.Drawing.Size(23, 23);
            this.ButtonCreate.Text = "Create room";
            this.ButtonCreate.Click += new System.EventHandler(this.ButtonCreate_Click);
            // 
            // LabelNoOfPlayers
            // 
            this.LabelNoOfPlayers.Name = "LabelNoOfPlayers";
            this.LabelNoOfPlayers.Size = new System.Drawing.Size(83, 23);
            this.LabelNoOfPlayers.Text = "No. of players ";
            // 
            // TextBoxNoOfPlayers
            // 
            this.TextBoxNoOfPlayers.Name = "TextBoxNoOfPlayers";
            this.TextBoxNoOfPlayers.Size = new System.Drawing.Size(100, 26);
            // 
            // LabelCreatePlayerType
            // 
            this.LabelCreatePlayerType.Name = "LabelCreatePlayerType";
            this.LabelCreatePlayerType.Size = new System.Drawing.Size(68, 23);
            this.LabelCreatePlayerType.Text = "Player type ";
            // 
            // ComboBoxCreatePlayerType
            // 
            this.ComboBoxCreatePlayerType.Items.AddRange(new object[] {
            "Regular",
            "Referee"});
            this.ComboBoxCreatePlayerType.Name = "ComboBoxCreatePlayerType";
            this.ComboBoxCreatePlayerType.Size = new System.Drawing.Size(121, 26);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 26);
            // 
            // ButtonJoin
            // 
            this.ButtonJoin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonJoin.Image = ((System.Drawing.Image)(resources.GetObject("ButtonJoin.Image")));
            this.ButtonJoin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonJoin.Name = "ButtonJoin";
            this.ButtonJoin.Size = new System.Drawing.Size(23, 23);
            this.ButtonJoin.Text = "Join room";
            this.ButtonJoin.Click += new System.EventHandler(this.ButtonJoin_Click);
            // 
            // LabelAvailableRooms
            // 
            this.LabelAvailableRooms.Name = "LabelAvailableRooms";
            this.LabelAvailableRooms.Size = new System.Drawing.Size(95, 23);
            this.LabelAvailableRooms.Text = "Available rooms ";
            // 
            // ComboBoxAvailableRooms
            // 
            this.ComboBoxAvailableRooms.Name = "ComboBoxAvailableRooms";
            this.ComboBoxAvailableRooms.Size = new System.Drawing.Size(121, 26);
            // 
            // LabelJoinPlayerType
            // 
            this.LabelJoinPlayerType.Name = "LabelJoinPlayerType";
            this.LabelJoinPlayerType.Size = new System.Drawing.Size(68, 15);
            this.LabelJoinPlayerType.Text = "Player type ";
            // 
            // ComboBoxJoinPlayerType
            // 
            this.ComboBoxJoinPlayerType.Items.AddRange(new object[] {
            "Regular",
            "Referee"});
            this.ComboBoxJoinPlayerType.Name = "ComboBoxJoinPlayerType";
            this.ComboBoxJoinPlayerType.Size = new System.Drawing.Size(121, 23);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 26);
            // 
            // ButtonOK
            // 
            this.ButtonOK.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonOK.Image = ((System.Drawing.Image)(resources.GetObject("ButtonOK.Image")));
            this.ButtonOK.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(23, 20);
            this.ButtonOK.Text = "OK";
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // textBoxMale
            // 
            this.textBoxMale.Location = new System.Drawing.Point(12, 77);
            this.textBoxMale.Name = "textBoxMale";
            this.textBoxMale.Size = new System.Drawing.Size(100, 20);
            this.textBoxMale.TabIndex = 1;
            // 
            // textBoxFemale
            // 
            this.textBoxFemale.Location = new System.Drawing.Point(118, 77);
            this.textBoxFemale.Name = "textBoxFemale";
            this.textBoxFemale.Size = new System.Drawing.Size(100, 20);
            this.textBoxFemale.TabIndex = 2;
            // 
            // textBoxFood
            // 
            this.textBoxFood.Location = new System.Drawing.Point(224, 77);
            this.textBoxFood.Name = "textBoxFood";
            this.textBoxFood.Size = new System.Drawing.Size(100, 20);
            this.textBoxFood.TabIndex = 3;
            // 
            // textBoxCountry
            // 
            this.textBoxCountry.Location = new System.Drawing.Point(330, 77);
            this.textBoxCountry.Name = "textBoxCountry";
            this.textBoxCountry.Size = new System.Drawing.Size(100, 20);
            this.textBoxCountry.TabIndex = 4;
            // 
            // textBoxAnimal
            // 
            this.textBoxAnimal.Location = new System.Drawing.Point(436, 77);
            this.textBoxAnimal.Name = "textBoxAnimal";
            this.textBoxAnimal.Size = new System.Drawing.Size(100, 20);
            this.textBoxAnimal.TabIndex = 5;
            // 
            // LabelMale
            // 
            this.LabelMale.Location = new System.Drawing.Point(12, 54);
            this.LabelMale.Name = "LabelMale";
            this.LabelMale.Size = new System.Drawing.Size(100, 20);
            this.LabelMale.TabIndex = 6;
            this.LabelMale.Text = "Male";
            // 
            // labelFemale
            // 
            this.labelFemale.Location = new System.Drawing.Point(118, 54);
            this.labelFemale.Name = "labelFemale";
            this.labelFemale.Size = new System.Drawing.Size(100, 20);
            this.labelFemale.TabIndex = 7;
            this.labelFemale.Text = "Female";
            // 
            // labelFood
            // 
            this.labelFood.Location = new System.Drawing.Point(224, 54);
            this.labelFood.Name = "labelFood";
            this.labelFood.Size = new System.Drawing.Size(100, 20);
            this.labelFood.TabIndex = 8;
            this.labelFood.Text = "Food";
            // 
            // labelCountry
            // 
            this.labelCountry.Location = new System.Drawing.Point(330, 54);
            this.labelCountry.Name = "labelCountry";
            this.labelCountry.Size = new System.Drawing.Size(100, 20);
            this.labelCountry.TabIndex = 9;
            this.labelCountry.Text = "Country";
            // 
            // labelAnimal
            // 
            this.labelAnimal.Location = new System.Drawing.Point(436, 54);
            this.labelAnimal.Name = "labelAnimal";
            this.labelAnimal.Size = new System.Drawing.Size(100, 20);
            this.labelAnimal.TabIndex = 10;
            this.labelAnimal.Text = "Animal";
            // 
            // buttonAutobusComplete
            // 
            this.buttonAutobusComplete.Location = new System.Drawing.Point(542, 75);
            this.buttonAutobusComplete.Name = "buttonAutobusComplete";
            this.buttonAutobusComplete.Size = new System.Drawing.Size(150, 23);
            this.buttonAutobusComplete.TabIndex = 11;
            this.buttonAutobusComplete.Text = "Autobus Complete!";
            this.buttonAutobusComplete.UseVisualStyleBackColor = true;
            this.buttonAutobusComplete.Click += new System.EventHandler(this.buttonAutobusComplete_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LabelConnectionStatus,
            this.LabelRoomNo});
            this.statusStrip1.Location = new System.Drawing.Point(0, 136);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(716, 22);
            this.statusStrip1.TabIndex = 12;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // LabelConnectionStatus
            // 
            this.LabelConnectionStatus.ForeColor = System.Drawing.Color.Red;
            this.LabelConnectionStatus.Name = "LabelConnectionStatus";
            this.LabelConnectionStatus.Size = new System.Drawing.Size(79, 17);
            this.LabelConnectionStatus.Text = "Disconnected";
            // 
            // LabelRoomNo
            // 
            this.LabelRoomNo.Name = "LabelRoomNo";
            this.LabelRoomNo.Size = new System.Drawing.Size(0, 17);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(716, 158);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.buttonAutobusComplete);
            this.Controls.Add(this.labelAnimal);
            this.Controls.Add(this.labelCountry);
            this.Controls.Add(this.labelFood);
            this.Controls.Add(this.labelFemale);
            this.Controls.Add(this.LabelMale);
            this.Controls.Add(this.textBoxAnimal);
            this.Controls.Add(this.textBoxCountry);
            this.Controls.Add(this.textBoxFood);
            this.Controls.Add(this.textBoxFemale);
            this.Controls.Add(this.textBoxMale);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Autobus Complete";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton ButtonCreate;
        private System.Windows.Forms.ToolStripButton ButtonJoin;
        private System.Windows.Forms.ToolStripComboBox ComboBoxAvailableRooms;
        private System.Windows.Forms.ToolStripTextBox TextBoxNoOfPlayers;
        private System.Windows.Forms.ToolStripComboBox ComboBoxCreatePlayerType;
        private System.Windows.Forms.ToolStripComboBox ComboBoxJoinPlayerType;
        private System.Windows.Forms.ToolStripButton ButtonOK;
        private System.Windows.Forms.ToolStripLabel LabelNoOfPlayers;
        private System.Windows.Forms.ToolStripLabel LabelCreatePlayerType;
        private System.Windows.Forms.ToolStripLabel LabelAvailableRooms;
        private System.Windows.Forms.ToolStripLabel LabelJoinPlayerType;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.TextBox textBoxMale;
        private System.Windows.Forms.TextBox textBoxFemale;
        private System.Windows.Forms.TextBox textBoxFood;
        private System.Windows.Forms.TextBox textBoxCountry;
        private System.Windows.Forms.TextBox textBoxAnimal;
        private System.Windows.Forms.Label LabelMale;
        private System.Windows.Forms.Label labelFemale;
        private System.Windows.Forms.Label labelFood;
        private System.Windows.Forms.Label labelCountry;
        private System.Windows.Forms.Label labelAnimal;
        private System.Windows.Forms.Button buttonAutobusComplete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel LabelConnectionStatus;
        private System.Windows.Forms.ToolStripStatusLabel LabelRoomNo;
    }
}

