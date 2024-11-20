using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] bt = new byte[] { 127, 0, 0, 1 };
            IPAddress ip = new IPAddress(bt);
            TcpClient client = new TcpClient();
            
            try
            {
                client.Connect(ip, 8888);
                NetworkStream stream = client.GetStream();

                BinaryReader br = new BinaryReader(stream);
                BinaryWriter bw = new BinaryWriter(stream);

                Console.WriteLine("Connected!");
                bw.Write("CREATE;5;PLAYER;C");
                stream.Flush();

                //Console.WriteLine("disconnected!");

            }
            catch (Exception e)
            {
                Console.WriteLine("couldn't connect to server!");
            }

            Console.ReadLine();

        }
    }
}
