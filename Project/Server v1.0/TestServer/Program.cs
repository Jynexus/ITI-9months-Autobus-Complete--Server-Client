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
        static List<HandleClient> Clients = new List<HandleClient> ();

        static void Main(string[] args)
        {
            TcpListener Server = new TcpListener(new IPAddress(new byte[] { 127, 0, 0, 1 }), 8888);
            Socket S;
            
            int counter = 0;
            
            Server.Start();
            Console.WriteLine(">> Server started.\n");

            //Start a new thread to check client connectivity:
            Thread ConCheck = new Thread(new ThreadStart(CheckConnectivity));
            ConCheck.Start();

            //Listen to new connections:
            while (true)
            {
                S = Server.AcceptSocket();
                
                HandleClient C = new HandleClient(S, counter);
                Clients.Add(C);

                counter++;

                Console.WriteLine(">> Connected to client {0}", Clients.Count);
            }
        }
        static void CheckConnectivity ()
        {
            while(true)
            {
                for (int i = 0; i < Clients.Count; i++)
                {
                    try //Try to write an empty string to the clients' stream.
                    {
                        Clients[i].WriteToStream("");
                    }
                    catch (Exception e) //If the client disconnects.
                    {
                        Console.WriteLine("\n>> Removed client ID: {0}", Clients[i].IDProperty);
                        Clients.RemoveAt(i);
                    }
                }
            }
        }

    }
    public class HandleClient
    {
        Socket ClientSocket;
        NetworkStream NS;

        int ID;

        public int IDProperty
        {
            get { return ID; }
            set { ID = value; }
        }

        BinaryReader br;
        BinaryWriter bw;

        public HandleClient(Socket S, int C)
        {
            ClientSocket = S;
            NS = new NetworkStream(ClientSocket);

            ID = C;

            br = new BinaryReader(NS);
            bw = new BinaryWriter(NS);

            Thread ClientRead = new Thread(new ThreadStart (ReadFromStream));
            ClientRead.Start();
        }

        public void ReadFromStream()
        {
            string Message; 

            while(true)
            {
                Message = br.ReadString();
                Console.WriteLine(Message);
                Console.WriteLine("");

                string [] tokens = Message.Split(';');

                switch (tokens[0]) 
                {
                    case "CREATE":
                        Console.WriteLine("\n>> CREATE requested from client ID: {0}", ID);
                        //CREATE NEW ROOM.
                        //Write RESPONSE.
                        break;
                    case "JOIN":
                        //JOIN if possible.
                        break;
                    case "COMPLETE":
                        //Broadcast COMPLETE to other clients in the room.
                        break;

                    //etc.
                }
            }
        }

        public void WriteToStream(string Message)
        {
            bw.Write(Message);
        }
        
    }
}
