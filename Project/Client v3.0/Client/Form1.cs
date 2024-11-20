using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
    public partial class Form1 : Form
    {
        bool Connected;
        int CreateJoin; //true if CREATE, false if JOIN

        string playerType;

        //new, for the ref
        int currentAnsRow;

        BinaryReader br;
        BinaryWriter bw;
        public Form1()
        {
            InitializeComponent();

            ComboBoxAvailableRooms.Visible = false;
            ComboBoxJoinPlayerType.Visible = false;
            TextBoxNoOfPlayers.Visible = false;
            ComboBoxCreatePlayerType.Visible = false;

            LabelNoOfPlayers.Visible = false;
            LabelCreatePlayerType.Visible = false;
            LabelAvailableRooms.Visible = false;
            LabelJoinPlayerType.Visible = false;

            ButtonOK.Enabled = false;

            IPAddress ip = new IPAddress(new byte[] { 127, 0, 0, 1 });
            TcpClient client = new TcpClient();

            try
            {
                client.Connect(ip, 8888);
                NetworkStream stream = client.GetStream();

                br = new BinaryReader(stream);
                bw = new BinaryWriter(stream);
                
                Connected = true;
                LabelConnectionStatus.ForeColor = Color.Green;
                LabelConnectionStatus.Text = "Connected";
                MessageBox.Show("Connected!");

                Thread ServerRead = new Thread(new ThreadStart(ReadFromStream));
                Control.CheckForIllegalCrossThreadCalls = false;
                ServerRead.Start();
            }
            catch (Exception e)
            {
                Connected = false;
                LabelConnectionStatus.ForeColor = Color.Red;
                LabelConnectionStatus.Text = "Disconnected";
                MessageBox.Show("Couldn't connect to server!");
            }
        }
        //
        //Read method
        //
        public void ReadFromStream()
        {
            string Message;

            while (true)
            {
                try
                {
                    Message = br.ReadString();
                    string[] tokens = Message.Split(';');

                    switch (tokens[0])
                    {
                        case "CREATE":
                            if (tokens[1] == "SUCCESS")
                            {
                                MessageBox.Show("Room created successfully. Your room number is: " + tokens[2]);
                                LabelRoomNo.Text = ", To room: " + tokens[2];

                                DisableInterface(); //disable UI buttons
                            }
                            break;

                        case "ROOMS":
                            for (int i = 1; i < tokens.Length - 1; i++)
                                ComboBoxAvailableRooms.Items.Add(tokens[i]);

                            if (tokens.Length > 2) //at least one room is created
                            {
                                ComboBoxAvailableRooms.SelectedIndex = 0;
                                ButtonOK.Enabled = true;
                            }
                            else
                                ButtonOK.Enabled = false;

                            break;

                        case "JOIN":
                            if (tokens[1] == "SUCCESS")
                            {
                                string[] SplitRoomString = ComboBoxAvailableRooms.SelectedItem.ToString().Split(' ', '/', ')');
                                MessageBox.Show("Room joined successfully. Your room number is: " + SplitRoomString[0]);
                                LabelRoomNo.Text = ", To room: " + SplitRoomString[0];

                                DisableInterface(); //disable UI buttons
                            }
                            else
                            {
                                MessageBox.Show("Couldn't join room");
                            }
                            break;

                        case "START":
                            LabelGameLetter.Text = "Game letter: " + tokens[1];

                            TextBox tempTextBox;
                            if (playerType == "Regular")
                            {
                                tempTextBox = (TextBox)Controls.Find("textBoxMale", true)[0];
                                tempTextBox.ReadOnly = false;

                                tempTextBox = (TextBox)Controls.Find("textBoxFemale", true)[0];
                                tempTextBox.ReadOnly = false;

                                tempTextBox = (TextBox)Controls.Find("textBoxFood", true)[0];
                                tempTextBox.ReadOnly = false;

                                tempTextBox = (TextBox)Controls.Find("textBoxCountry", true)[0];
                                tempTextBox.ReadOnly = false;

                                tempTextBox = (TextBox)Controls.Find("textBoxAnimal", true)[0];
                                tempTextBox.ReadOnly = false;
                            }

                            break;

                        case "FINISH":

                            TextBox tempTextBox2;
                            string answerString = "ANS;";

                            if (playerType == "Regular")
                            {
                                tempTextBox2 = (TextBox)Controls.Find("textBoxMale", true)[0];
                                tempTextBox2.ReadOnly = true;
                                answerString += tempTextBox2.Text + ";";

                                tempTextBox2 = (TextBox)Controls.Find("textBoxFemale", true)[0];
                                tempTextBox2.ReadOnly = true;
                                answerString += tempTextBox2.Text + ";";

                                tempTextBox2 = (TextBox)Controls.Find("textBoxFood", true)[0];
                                tempTextBox2.ReadOnly = true;
                                answerString += tempTextBox2.Text + ";";

                                tempTextBox2 = (TextBox)Controls.Find("textBoxCountry", true)[0];
                                tempTextBox2.ReadOnly = true;
                                answerString += tempTextBox2.Text + ";";

                                tempTextBox2 = (TextBox)Controls.Find("textBoxAnimal", true)[0];
                                tempTextBox2.ReadOnly = true;
                                answerString += tempTextBox2.Text + ";";

                                bw.Write(answerString);
                            }
                            else if (playerType == "Referee")
                            {
                                //Do nothing
                            }

                            break;

                        case "ANS":
                            if (playerType == "Referee")
                            {

                            }
                            break;
                    }
                }
                catch (Exception e)
                {
                    Connected = false;
                    LabelConnectionStatus.ForeColor = Color.Red;
                    LabelConnectionStatus.Text = "Disconnected";
                    LabelRoomNo.Text = "";

                    MessageBox.Show("Disconnected from server");
                    //Debugging
                    //MessageBox.Show(e.ToString());
                    break;
                }
            }
        }
        //
        //Events
        //
        private void ButtonCreate_Click(object sender, EventArgs e)
        {
            ComboBoxAvailableRooms.Visible = false;
            ComboBoxJoinPlayerType.Visible = false;
            LabelAvailableRooms.Visible = false;
            LabelJoinPlayerType.Visible = false;

            TextBoxNoOfPlayers.Visible = true;
            ComboBoxCreatePlayerType.Visible = true;
            ComboBoxCreatePlayerType.SelectedIndex = 0;
            LabelNoOfPlayers.Visible = true;
            LabelCreatePlayerType.Visible = true;
            
            CreateJoin = 1;
            playerType = ComboBoxCreatePlayerType.SelectedItem.ToString();
        }
        private void ButtonJoin_Click(object sender, EventArgs e)
        {
            ComboBoxAvailableRooms.Visible = true;
            ComboBoxJoinPlayerType.Visible = true;
            ComboBoxJoinPlayerType.SelectedIndex = 0;
            LabelAvailableRooms.Visible = true;
            LabelJoinPlayerType.Visible = true;

            TextBoxNoOfPlayers.Visible = false;
            ComboBoxCreatePlayerType.Visible = false;
            LabelNoOfPlayers.Visible = false;
            LabelCreatePlayerType.Visible = false;

            if (Connected)
            {
                //Request available rooms
                bw.Write("ROOMS;");
            }

            CreateJoin = 2;
        }
        private void ButtonOK_Click(object sender, EventArgs e)
        {
            #region reconnect if disconnected
            // Try to reconnect with server if disconnected
            if (!Connected)
            {
                IPAddress ip = new IPAddress(new byte[] { 127, 0, 0, 1 });
                TcpClient client = new TcpClient();

                try
                {
                    client.Connect(ip, 8888);
                    NetworkStream stream = client.GetStream();

                    br = new BinaryReader(stream);
                    bw = new BinaryWriter(stream);

                    Connected = true;
                    LabelConnectionStatus.ForeColor = Color.Green;
                    LabelConnectionStatus.Text = "Connected";

                    Thread ServerRead = new Thread(new ThreadStart(ReadFromStream));
                    Control.CheckForIllegalCrossThreadCalls = false;
                    ServerRead.Start();
                }
                catch (Exception e1)
                {
                    Connected = false;
                    LabelConnectionStatus.ForeColor = Color.Red;
                    LabelConnectionStatus.Text = "Disconnected";
                    MessageBox.Show("Couldn't connect to server!");
                }
            }
            #endregion
            try
            {
                if (CreateJoin == 1 && Connected) //Create room
                {
                    playerType = ComboBoxCreatePlayerType.SelectedItem.ToString();
                    bw.Write("CREATE;" + TextBoxNoOfPlayers.Text + ";" + playerType + ";");
                }
                else if (CreateJoin == 2 && Connected) //Join room
                {
                    playerType = ComboBoxJoinPlayerType.SelectedItem.ToString();

                    string[] SplitRoomString = ComboBoxAvailableRooms.SelectedItem.ToString().Split(' ', '/', ')');
                    //MessageBox.Show("" + ComboBoxAvailableRooms.SelectedItem.ToString()[0]);
                    bw.Write("JOIN;" + SplitRoomString[0] + ";" + playerType + ";");
                }

                //Run-time creation, regular
                if (playerType == "Regular" && Connected)
                    CreatePlayerUI();

                //Run-time creation, referee
                else if (playerType == "Referee" && Connected)
                {
                    if (CreateJoin == 1) //if the textbox has the information readily available
                    {
                        int playerCount = int.Parse(TextBoxNoOfPlayers.Text);
                        CreateRefereeUI(playerCount);
                    }
                    else
                    {

                        if (ComboBoxAvailableRooms.SelectedIndex >= 0)
                        {
                            string[] SplitRoomString = ComboBoxAvailableRooms.Items[ComboBoxAvailableRooms.SelectedIndex].ToString().Split(' ', '/', ')');
                            int playerCount = int.Parse(SplitRoomString[2]);
                            CreateRefereeUI(playerCount);
                        }
                    }
                }
            }
            catch (Exception e1)
            {
                Connected = false;
                LabelConnectionStatus.ForeColor = Color.Red;
                LabelConnectionStatus.Text = "Disconnected";
                LabelRoomNo.Text = "";

                MessageBox.Show("Disconnected from server");
                //Debugging
                //MessageBox.Show(e.ToString());
            }
        }
        private void buttonAutobusComplete_Click(object sender, EventArgs e)
        {
            bw.Write("COMPLETE;");
        }
        private void TextBoxNoOfPlayers_TextChanged(object sender, EventArgs e)
        {
            int val;
            if (int.TryParse(TextBoxNoOfPlayers.Text, out val))
            {
                if (val > 1)
                    ButtonOK.Enabled = true;
            }
            else
                ButtonOK.Enabled = false;
        }
        //
        //Interface methods
        //
        private void DisableInterface()
        {
            ComboBoxAvailableRooms.Visible = false;
            ComboBoxJoinPlayerType.Visible = false;
            TextBoxNoOfPlayers.Visible = false;
            ComboBoxCreatePlayerType.Visible = false;

            LabelNoOfPlayers.Visible = false;
            LabelCreatePlayerType.Visible = false;
            LabelAvailableRooms.Visible = false;
            LabelJoinPlayerType.Visible = false;

            ButtonCreate.Enabled = false;
            ButtonJoin.Enabled = false;
            ButtonOK.Enabled = false;
        }
        private void CreatePlayerUI()
        {
                    TextBox textBoxMale = new TextBox();
                    TextBox textBoxFemale = new TextBox();
                    TextBox textBoxFood = new TextBox();
                    TextBox textBoxCountry = new TextBox();
                    TextBox textBoxAnimal = new TextBox();

                    Label LabelMale = new Label();
                    Label labelFemale = new Label();
                    Label labelFood = new Label();
                    Label labelCountry = new Label();
                    Label labelAnimal = new Label();

                    Button buttonAutobusComplete = new Button();

                    Controls.AddRange(new Control[] { textBoxMale, textBoxFemale, textBoxFood, textBoxCountry, textBoxAnimal
                                                    , LabelMale, labelFemale, labelFood, labelCountry, labelAnimal, buttonAutobusComplete});
                    // 
                    // textBoxMale
                    // 
                    textBoxMale.Location = new System.Drawing.Point(12, 77);
                    textBoxMale.Name = "textBoxMale";
                    textBoxMale.Size = new System.Drawing.Size(100, 20);
                    textBoxMale.TabIndex = 1;
                    textBoxMale.ReadOnly = true;
                    // 
                    // textBoxFemale
                    // 
                    textBoxFemale.Location = new System.Drawing.Point(118, 77);
                    textBoxFemale.Name = "textBoxFemale";
                    textBoxFemale.Size = new System.Drawing.Size(100, 20);
                    textBoxFemale.TabIndex = 2;
                    textBoxFemale.ReadOnly = true;
                    // 
                    // textBoxFood
                    // 
                    textBoxFood.Location = new System.Drawing.Point(224, 77);
                    textBoxFood.Name = "textBoxFood";
                    textBoxFood.Size = new System.Drawing.Size(100, 20);
                    textBoxFood.TabIndex = 3;
                    textBoxFood.ReadOnly = true;
                    // 
                    // textBoxCountry
                    // 
                    textBoxCountry.Location = new System.Drawing.Point(330, 77);
                    textBoxCountry.Name = "textBoxCountry";
                    textBoxCountry.Size = new System.Drawing.Size(100, 20);
                    textBoxCountry.TabIndex = 4;
                    textBoxCountry.ReadOnly = true;
                    // 
                    // textBoxAnimal
                    // 
                    textBoxAnimal.Location = new System.Drawing.Point(436, 77);
                    textBoxAnimal.Name = "textBoxAnimal";
                    textBoxAnimal.Size = new System.Drawing.Size(100, 20);
                    textBoxAnimal.TabIndex = 5;
                    textBoxAnimal.ReadOnly = true;
                    // 
                    // LabelMale
                    // 
                    LabelMale.Location = new System.Drawing.Point(12, 54);
                    LabelMale.Name = "LabelMale";
                    LabelMale.Size = new System.Drawing.Size(100, 20);
                    LabelMale.TabIndex = 6;
                    LabelMale.Text = "Male";
                    // 
                    // labelFemale
                    // 
                    labelFemale.Location = new System.Drawing.Point(118, 54);
                    labelFemale.Name = "labelFemale";
                    labelFemale.Size = new System.Drawing.Size(100, 20);
                    labelFemale.TabIndex = 7;
                    labelFemale.Text = "Female";
                    // 
                    // labelFood
                    // 
                    labelFood.Location = new System.Drawing.Point(224, 54);
                    labelFood.Name = "labelFood";
                    labelFood.Size = new System.Drawing.Size(100, 20);
                    labelFood.TabIndex = 8;
                    labelFood.Text = "Food";
                    // 
                    // labelCountry
                    // 
                    labelCountry.Location = new System.Drawing.Point(330, 54);
                    labelCountry.Name = "labelCountry";
                    labelCountry.Size = new System.Drawing.Size(100, 20);
                    labelCountry.TabIndex = 9;
                    labelCountry.Text = "Country";
                    // 
                    // labelAnimal
                    // 
                    labelAnimal.Location = new System.Drawing.Point(436, 54);
                    labelAnimal.Name = "labelAnimal";
                    labelAnimal.Size = new System.Drawing.Size(100, 20);
                    labelAnimal.TabIndex = 10;
                    labelAnimal.Text = "Animal";
                    // 
                    // buttonAutobusComplete
                    // 
                    buttonAutobusComplete.Location = new System.Drawing.Point(542, 75);
                    buttonAutobusComplete.Name = "buttonAutobusComplete";
                    buttonAutobusComplete.Size = new System.Drawing.Size(150, 23);
                    buttonAutobusComplete.TabIndex = 11;
                    buttonAutobusComplete.Text = "Autobus Complete!";
                    buttonAutobusComplete.UseVisualStyleBackColor = true;
                    buttonAutobusComplete.Click += new System.EventHandler(this.buttonAutobusComplete_Click);
        }
        private void CreateRefereeUI(int playerCount)
        {
                #region labels
                    Label LabelMale = new Label();
                    Label labelFemale = new Label();
                    Label labelFood = new Label();
                    Label labelCountry = new Label();
                    Label labelAnimal = new Label();

                    Controls.AddRange(new Control[] { LabelMale, labelFemale, labelFood, labelCountry, labelAnimal });

                    // 
                    // LabelMale
                    // 
                    LabelMale.Location = new System.Drawing.Point(12, 54);
                    LabelMale.Name = "LabelMale";
                    LabelMale.Size = new System.Drawing.Size(100, 20);
                    LabelMale.TabIndex = 6;
                    LabelMale.Text = "Male";
                    // 
                    // labelFemale
                    // 
                    labelFemale.Location = new System.Drawing.Point(118, 54);
                    labelFemale.Name = "labelFemale";
                    labelFemale.Size = new System.Drawing.Size(100, 20);
                    labelFemale.TabIndex = 7;
                    labelFemale.Text = "Female";
                    // 
                    // labelFood
                    // 
                    labelFood.Location = new System.Drawing.Point(224, 54);
                    labelFood.Name = "labelFood";
                    labelFood.Size = new System.Drawing.Size(100, 20);
                    labelFood.TabIndex = 8;
                    labelFood.Text = "Food";
                    // 
                    // labelCountry
                    // 
                    labelCountry.Location = new System.Drawing.Point(330, 54);
                    labelCountry.Name = "labelCountry";
                    labelCountry.Size = new System.Drawing.Size(100, 20);
                    labelCountry.TabIndex = 9;
                    labelCountry.Text = "Country";
                    // 
                    // labelAnimal
                    // 
                    labelAnimal.Location = new System.Drawing.Point(436, 54);
                    labelAnimal.Name = "labelAnimal";
                    labelAnimal.Size = new System.Drawing.Size(100, 20);
                    labelAnimal.TabIndex = 10;
                    labelAnimal.Text = "Animal";
                    #endregion

                for (int i = 0; i < playerCount; i++)
                {
                    #region declarations
                        TextBox textBox1 = new TextBox();
                        TextBox textBox2 = new TextBox();
                        TextBox textBox3 = new TextBox();
                        TextBox textBox4 = new TextBox();
                        TextBox textBox5 = new TextBox();
                        TextBox textBox6 = new TextBox();
                        NumericUpDown numericUpDown1 = new NumericUpDown();
                        NumericUpDown numericUpDown2 = new NumericUpDown();
                        NumericUpDown numericUpDown3 = new NumericUpDown();
                        NumericUpDown numericUpDown4 = new NumericUpDown();
                        NumericUpDown numericUpDown5 = new NumericUpDown();
                        #endregion

                    #region properties
                        // 
                        // textBox1
                        // 
                        textBox1.Location = new System.Drawing.Point(12, 80 + 61 * i);
                        textBox1.Name = "textBox_1_" + i.ToString();
                        textBox1.ReadOnly = true;
                        textBox1.Size = new System.Drawing.Size(100, 20);
                        textBox1.TabIndex = 13;
                        // 
                        // numericUpDown1
                        // 
                        numericUpDown1.Increment = new decimal(new int[] { 5, 0, 0, 0 });
                        numericUpDown1.Location = new System.Drawing.Point(12, 101 + 61 * i);
                        numericUpDown1.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
                        numericUpDown1.Name = "numericUpDown_1_" + i.ToString();
                        numericUpDown1.Size = new System.Drawing.Size(100, 20);
                        numericUpDown1.TabIndex = 14;
                        // 
                        // numericUpDown2
                        // 
                        numericUpDown2.Increment = new decimal(new int[] { 5, 0, 0, 0 });
                        numericUpDown2.Location = new System.Drawing.Point(118, 101 + 61 * i);
                        numericUpDown2.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
                        numericUpDown2.Name = "numericUpDown_2_" + i.ToString();
                        numericUpDown2.Size = new System.Drawing.Size(100, 20);
                        numericUpDown2.TabIndex = 16;
                        // 
                        // textBox2
                        // 
                        textBox2.Location = new System.Drawing.Point(118, 80 + 61 * i);
                        textBox2.Name = "textBox_2_" + i.ToString();
                        textBox2.ReadOnly = true;
                        textBox2.Size = new System.Drawing.Size(100, 20);
                        textBox2.TabIndex = 15;
                        // 
                        // numericUpDown3
                        // 
                        numericUpDown3.Increment = new decimal(new int[] { 5, 0, 0, 0 });
                        numericUpDown3.Location = new System.Drawing.Point(224, 101 + 61 * i);
                        numericUpDown3.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
                        numericUpDown3.Name = "numericUpDown_3_" + i.ToString();
                        numericUpDown3.Size = new System.Drawing.Size(100, 20);
                        numericUpDown3.TabIndex = 18;
                        // 
                        // textBox3
                        // 
                        textBox3.Location = new System.Drawing.Point(224, 80 + 61 * i);
                        textBox3.Name = "textBox_3_" + i.ToString();
                        textBox3.ReadOnly = true;
                        textBox3.Size = new System.Drawing.Size(100, 20);
                        textBox3.TabIndex = 17;
                        // 
                        // numericUpDown4
                        // 
                        numericUpDown4.Increment = new decimal(new int[] { 5, 0, 0, 0 });
                        numericUpDown4.Location = new System.Drawing.Point(330, 101 + 61 * i);
                        numericUpDown4.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
                        numericUpDown4.Name = "numericUpDown_4_" + i.ToString();
                        numericUpDown4.Size = new System.Drawing.Size(100, 20);
                        numericUpDown4.TabIndex = 20;
                        // 
                        // textBox4
                        // 
                        textBox4.Location = new System.Drawing.Point(330, 80 + 61 * i);
                        textBox4.Name = "textBox_4_" + i.ToString();
                        textBox4.ReadOnly = true;
                        textBox4.Size = new System.Drawing.Size(100, 20);
                        textBox4.TabIndex = 19;
                        // 
                        // numericUpDown5
                        // 
                        numericUpDown5.Increment = new decimal(new int[] { 5, 0, 0, 0 });
                        numericUpDown5.Location = new System.Drawing.Point(436, 101 + 61 * i);
                        numericUpDown5.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
                        numericUpDown5.Name = "numericUpDown_5_" + i.ToString();
                        numericUpDown5.Size = new System.Drawing.Size(100, 20);
                        numericUpDown5.TabIndex = 22;
                        // 
                        // textBox5
                        // 
                        textBox5.Location = new System.Drawing.Point(436, 80 + 61 * i);
                        textBox5.Name = "textBox_5_" + i.ToString();
                        textBox5.ReadOnly = true;
                        textBox5.Size = new System.Drawing.Size(100, 20);
                        textBox5.TabIndex = 21;
                        // 
                        // textBox6
                        // 
                        textBox6.Location = new System.Drawing.Point(542, 80 + 61 * i);
                        textBox6.Multiline = true;
                        textBox6.Name = "textBox_6_" + i.ToString();
                        textBox6.ReadOnly = true;
                        textBox6.Size = new System.Drawing.Size(30, 41);
                        textBox6.TabIndex = 23;

                        textBox6.Text = i.ToString();
                        #endregion

                    #region AddControls
                        Controls.Add(textBox1);
                        Controls.Add(textBox2);
                        Controls.Add(textBox3);
                        Controls.Add(textBox4);
                        Controls.Add(textBox5);
                        Controls.Add(textBox6);

                        Controls.Add(numericUpDown1);
                        Controls.Add(numericUpDown2);
                        Controls.Add(numericUpDown3);
                        Controls.Add(numericUpDown4);
                        Controls.Add(numericUpDown5);
                        #endregion
                }
        }
        //
        //Handling methods
        //
    } // Form1 class
} // namespace
