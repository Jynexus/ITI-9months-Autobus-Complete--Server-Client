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
            playerID = new int [maxPlayers];
        }

        int roomNo;
        public int RoomNo
        {
            get { return roomNo; }
            set { roomNo = value; }
        }

        public int[] playerID;

        int currentPlayerCount;
        public int CurrentPlayerCount
        {
            get { return currentPlayerCount; }
            set { currentPlayerCount = value; }
        }

        int refereeID; // Holds referee ID
        public int RefereeID
        {
            get { return refereeID; }
            set { refereeID = value; }
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

        string[] ans;
        int[] grades;
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
                            //Broadcast FINISH to other clients in the room.
                            break;

                        //etc.
                
                    } //switch
                
                }
                catch (Exception e)
                {
                    Console.WriteLine("\n>> Disconnected from client ID: {0}", ID);
                    //Debugging...Lots and lots of debugging
                    //Console.WriteLine(e);
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
                Room.RefereeID = iD;
                this.playerType = "Referee";
            }
            else
            {
                this.playerType = "Regular";
                Room.playerID[0] = iD; // add the creating player to the list of players in the room
                Program.Rooms[Room.RoomNo - 1].CurrentPlayerCount++;
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
            string refereeID;

            //JOIN: JOIN;room_no;player_type
            string RoomsResponse = "ROOMS;";
            for (int i = 0; i < Program.Rooms.Count; i++)
            {
                roomNo = Program.Rooms[i].RoomNo.ToString();
                currentPlayers = Program.Rooms[i].CurrentPlayerCount.ToString();
                maxPlayers = Program.Rooms[i].playerID.Length.ToString();
                refereeID = Program.Rooms[i].RefereeID.ToString();

                RoomsResponse += roomNo + " (" + currentPlayers + "/" + maxPlayers + ") - Referee: " + refereeID + ";";
            }
            WriteToStream(RoomsResponse);
        }

        private void HandleJoin (string[] tokens)
        {
            //temp variables
            int tempRoomNo = int.Parse(tokens[1]) - 1;
            int currentPlayers = Program.Rooms[tempRoomNo].CurrentPlayerCount;
            int maxPlayers = Program.Rooms[tempRoomNo].playerID.Length;

            //if the player chooses to play AND current players < max players for this particular room
            if (currentPlayers < maxPlayers && tokens[2] == "Regular")
            {
                roomNo = int.Parse(tokens[1]);
                int currentIndex = Program.Rooms[roomNo - 1].CurrentPlayerCount;

                Program.Rooms[roomNo - 1].playerID[currentIndex] = iD;
                Program.Rooms[roomNo - 1].CurrentPlayerCount++;
                WriteToStream("JOIN;SUCCESS");
                Console.WriteLine("   Success\n");

                //Check to start the game:
                if (++currentPlayers == maxPlayers && Program.Rooms[roomNo - 1].RefereeID != 0)
                    HandleStart(roomNo - 1);
            }
            //if the player chooses to referee and the room didn't have a referee
            else if (Program.Rooms[tempRoomNo].RefereeID == 0 && tokens[2] == "Referee")
            {
                roomNo = int.Parse(tokens[1]);
                Program.Rooms[roomNo - 1].RefereeID = iD;
                WriteToStream("JOIN;SUCCESS");
                Console.WriteLine("   Success\n");
            }
            else
            {
                WriteToStream("JOIN;FAIL");
                Console.WriteLine("   Fail!\n");
            }       
        }

        private void HandleStart (int roomIndex)
        {
            for (int i = 0; i < Program.Rooms[roomIndex].CurrentPlayerCount; i++)
                ; //CODE :)
        }

    } //HandleClient

}