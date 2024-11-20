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

                                DisableUI(); //disable UI buttons
                            }
                            break;

                        case "ROOMS":
                            for (int i = 1; i < tokens.Length - 1; i++)
                                ComboBoxAvailableRooms.Items.Add(tokens[i]);
                            break;

                        case "JOIN":
                            if (tokens[1] == "SUCCESS")
                            {
                                MessageBox.Show("Room joined successfully. Your room number is: " + ComboBoxAvailableRooms.SelectedItem.ToString()[0]);
                                LabelRoomNo.Text = ", To room: " + ComboBoxAvailableRooms.SelectedItem.ToString()[0];

                                DisableUI(); //disable UI buttons
                            }
                            else
                            {
                                MessageBox.Show("Couldn't join room");
                            }
                            break;
                    }
                }
                catch (Exception e)
                {
                    Connected = false;
                    LabelConnectionStatus.ForeColor = Color.Red;
                    LabelConnectionStatus.Text = "Disconnected";
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
            LabelNoOfPlayers.Visible = true;
            LabelCreatePlayerType.Visible = true;

            CreateJoin = 1;
        }

        private void ButtonJoin_Click(object sender, EventArgs e)
        {
            ComboBoxAvailableRooms.Visible = true;
            ComboBoxJoinPlayerType.Visible = true;
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
                    MessageBox.Show("Connected!");
                }
                catch (Exception e1)
                {
                    Connected = false;
                    LabelConnectionStatus.ForeColor = Color.Red;
                    LabelConnectionStatus.Text = "Disconnected";
                    MessageBox.Show("Couldn't connect to server!");
                }
            }

            if (CreateJoin == 1 && Connected) //Create room
            {
                bw.Write("CREATE;" + TextBoxNoOfPlayers.Text + ";" + ComboBoxCreatePlayerType.SelectedText + ";");
            }
            else if (CreateJoin == 2 && Connected) //Join room
            {
                //MessageBox.Show("" + ComboBoxAvailableRooms.SelectedItem.ToString()[0]);
                bw.Write("JOIN;" + ComboBoxAvailableRooms.SelectedItem.ToString()[0] + ";" + ComboBoxJoinPlayerType.SelectedText + ";");
            }

            //Run-time creation
            //if (ComboBoxCreatePlayerType.SelectedText == "Regular")
            //{
            //    for (int i = 0; i < 6; i++)
            //    { 
            //        TextBox textBox = new TextBox();
            //        Controls.Add(textBox);

            //        textBox.Location = new System.Drawing.Point(10 + 110 * i, 100);
            //        textBox.Name = "textBox" + i.ToString();
            //        textBox.Size = new System.Drawing.Size(100, 20);
            //        textBox.TabIndex = i;
            //    }
            //}
        }

        private void buttonAutobusComplete_Click(object sender, EventArgs e)
        {
            if (Connected)
                bw.Write("I'm done here");
        }

        private void DisableUI()
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

        //??
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
            // 
            // textBoxFemale
            // 
            textBoxFemale.Location = new System.Drawing.Point(118, 77);
            textBoxFemale.Name = "textBoxFemale";
            textBoxFemale.Size = new System.Drawing.Size(100, 20);
            textBoxFemale.TabIndex = 2;
            // 
            // textBoxFood
            // 
            textBoxFood.Location = new System.Drawing.Point(224, 77);
            textBoxFood.Name = "textBoxFood";
            textBoxFood.Size = new System.Drawing.Size(100, 20);
            textBoxFood.TabIndex = 3;
            // 
            // textBoxCountry
            // 
            textBoxCountry.Location = new System.Drawing.Point(330, 77);
            textBoxCountry.Name = "textBoxCountry";
            textBoxCountry.Size = new System.Drawing.Size(100, 20);
            textBoxCountry.TabIndex = 4;
            // 
            // textBoxAnimal
            // 
            textBoxAnimal.Location = new System.Drawing.Point(436, 77);
            textBoxAnimal.Name = "textBoxAnimal";
            textBoxAnimal.Size = new System.Drawing.Size(100, 20);
            textBoxAnimal.TabIndex = 5;
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
    }
}
