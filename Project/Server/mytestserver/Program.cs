using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TestServer
{
    class Program
    {
        public static List<HandleClient> Clients = new List<HandleClient>();
        public static List<RoomInfo> Rooms = new List<RoomInfo>();

        static void Main(string[] args)
        {
            TcpListener Server = new TcpListener(new IPAddress(new byte[] { 127, 0, 0, 1 }), 8888);
            Socket S;

            int counter = 1;

            Server.Start();
            Console.WriteLine(">> Server started.\n");

            //Listen to new connections
            while (true)
            {
                S = Server.AcceptSocket();

                HandleClient C = new HandleClient(S, counter);
                Clients.Add(C);

                counter++;

                Console.WriteLine("\n>> Connected to client ID: {0}", C.ID);
            }
        }
    }
    //
    //RoomInfo class
    //
    class RoomInfo
    {
        public RoomInfo (int no, int maxPlayers)
        {
            roomNo = no;
            this.maxPlayers = maxPlayers;
        }

        int roomNo;
        public int RoomNo
        {
            get { return roomNo; }
            set { roomNo = value; }
        }

        public List<HandleClient> players = new List<HandleClient>();

        int maxPlayers;
        public int MaxPlayers
        {
            get { return maxPlayers; }
            set { maxPlayers = value; }
        }

        HandleClient referee;
        public HandleClient Referee
        {
            get { return referee; }
            set { referee = value; }
        }
    }
    //
    //HandleClient class
    //
    public class HandleClient
    {
        Socket ClientSocket;
        NetworkStream NS;
        BinaryReader br;
        BinaryWriter bw;

        int iD;
        int roomNo;
        string playerType; //regular, ref, or, god forbid, a spectator.

        //used when playerType == "Regular"
        string[] ans = new string[6];
        //
        //Properties
        //
        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }

        public HandleClient(Socket S, int C)
        {
            ClientSocket = S;
            NS = new NetworkStream(ClientSocket);

            iD = C;

            br = new BinaryReader(NS);
            bw = new BinaryWriter(NS);

            Thread ClientRead = new Thread(new ThreadStart(ReadFromStream));
            ClientRead.Start();
        }

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
                            Console.WriteLine("\n>> CREATE requested from client ID: {0}", iD);
                            HandleCreate(tokens);
                            break;

                        case "ROOMS":
                            HandleRooms();
                            break;

                        case "JOIN":
                            Console.WriteLine("\n>> JOIN requested from client ID: {0}", iD);
                            HandleJoin(tokens);
                            break;

                        case "COMPLETE":
                            Console.WriteLine("\n>> Game finished at room: {0}", roomNo);
                            foreach(HandleClient player in Program.Rooms[roomNo - 1].players)
                                if(player.ans[0] != "Disconnected")
                                    player.WriteToStream("FINISH;");
                            break;

                        case "ANS":
                            HandleAns(tokens);
                            break;

                        case "GRADES":
                            foreach(HandleClient player in Program.Rooms[roomNo-1].players)
                            WriteToStream("RESULT;"+tokens[1]);
                            break;
                        //etc.
                
                    } //switch
                
                }
                catch (Exception e)
                {  
                    //
                    // We forgot to copy this line during the intergration phase :)
                    //
                    // This ensures that the answers string is sent to the referee in the correct format.
                    ans[0] = "Disconnected"; ans[1] = "Disconnected"; ans[2] = "Disconnected"; ans[3] = "Disconnected"; ans[4] = "Disconnected"; ans[5] = "1";
                    // This was what was originally sent
                    Console.WriteLine("\n>> Disconnected from client ID: {0}", ID);
                    //Debugging...Lots and lots of debugging
                    Console.WriteLine(e);
                    break; //from the while loop.
                }
            
            } //while
        
        } //ReadFromStream
        public void WriteToStream(string Message)
        {
            bw.Write(Message);
        }
        //
        //Request handling methods
        //
        private void HandleCreate(string[] tokens)
        {
            //CREATE NEW ROOM: CREATE;no_of_players;player_type
            RoomInfo Room = new RoomInfo(Program.Rooms.Count + 1, int.Parse(tokens[1])); // ctor takes the room number and the max number of players.
            Program.Rooms.Add(Room);

            this.roomNo = Program.Rooms.Count; // set the room number of the current HandleClient instance 

            if (tokens[2] == "Referee")
            {
                Room.Referee = this;
                this.playerType = "Referee";
            }
            else
            {
                this.playerType = "Regular";
                Room.players.Add(this); // add the creating player to the list of players in the room
            }

            WriteToStream("CREATE;SUCCESS;" + roomNo.ToString());
            Console.WriteLine("   Success\n");
        }
        private void HandleRooms()
        {
            //temp variables
            string roomNo;
            string currentPlayers;
            string maxPlayers;
            string referee;

            //JOIN: JOIN;room_no;player_type
            string RoomsResponse = "ROOMS;";
            for (int i = 0; i < Program.Rooms.Count; i++)
            {
                roomNo = Program.Rooms[i].RoomNo.ToString();
                maxPlayers = Program.Rooms[i].MaxPlayers.ToString();
                currentPlayers = Program.Rooms[i].players.Count.ToString();

                if (Program.Rooms[i].Referee != null)
                    referee = "Reserved";
                else
                    referee = "Available";

                RoomsResponse += roomNo + " (" + currentPlayers + "/" + maxPlayers + ") - Referee: " + referee + ";";
            }
            WriteToStream(RoomsResponse);
        }
        private void HandleJoin (string[] tokens)
        {
            //temp variables
            int tempRoomNo = int.Parse(tokens[1]) - 1;
            int currentPlayers = Program.Rooms[tempRoomNo].players.Count;
            int maxPlayers = Program.Rooms[tempRoomNo].MaxPlayers;

            //if the player chooses to play AND current players < max players for this particular room
            if (currentPlayers < maxPlayers && tokens[2] == "Regular")
            {
                roomNo = int.Parse(tokens[1]);

                Program.Rooms[roomNo - 1].players.Add(this);
                WriteToStream("JOIN;SUCCESS");
                Console.WriteLine("   Success\n");

                //Check to start the game:
                if (Program.Rooms[roomNo - 1].players.Count == maxPlayers && Program.Rooms[roomNo - 1].Referee != null)
                {
                    Console.WriteLine(">> Game started in room: " + roomNo.ToString());
                    HandleStart(roomNo - 1);
                }
            }
            //if the player chooses to referee and the room didn't have a referee
            else if (Program.Rooms[tempRoomNo].Referee == null && tokens[2] == "Referee")
            {
                roomNo = int.Parse(tokens[1]);
                Program.Rooms[roomNo - 1].Referee = this;
                WriteToStream("JOIN;SUCCESS");
                Console.WriteLine("   Success\n");

                //Check to start the game:
                if (Program.Rooms[roomNo - 1].players.Count == maxPlayers && Program.Rooms[roomNo - 1].Referee != null)
                {
                    Console.WriteLine(">> Game started in room: " + roomNo.ToString());
                    HandleStart(roomNo - 1);
                }
            }
            else
            {
                WriteToStream("JOIN;FAIL");
                Console.WriteLine("   Fail!\n");
            }       
        }
        private void HandleStart (int roomIndex)
        {
            //Generate the game letter
            Random r = new Random();
            char gameLetter = (char) r.Next(65, 90);
            try
            {
                //Respond to player in the room + the referee with START message and the game letter
                foreach (HandleClient player in Program.Rooms[roomIndex].players)
                    player.WriteToStream("START;" + gameLetter.ToString());

                Program.Rooms[roomIndex].Referee.WriteToStream("START;" + gameLetter.ToString());
            }
            catch (Exception e3)
            {
                Console.WriteLine(e3);
            }
        }
        private void HandleAns(string[] tokens)
        {
            for (int i = 1; i < tokens.Length - 1; i++)
                ans[i - 1] = tokens[i];

            ans[5] = "1"; // Set the flag to 1 

            string All = "ALL;";
            int sum = 0;

            foreach (HandleClient player in Program.Rooms[roomNo - 1].players)
                sum += int.Parse(player.ans[5]);

            // if all players' answers were successfully sent to the server
            if (sum == Program.Rooms[roomNo - 1].MaxPlayers)
            {
                foreach (HandleClient player in Program.Rooms[roomNo - 1].players)
                    All += player.ans[0] + ";" + player.ans[1] + ";" + player.ans[2] + ";" + player.ans[3] + ";" + player.ans[4] + ";" + player.ID + ";";

                Program.Rooms[roomNo - 1].Referee.WriteToStream(All);
            }
        }

    } //HandleClient

}