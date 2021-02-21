using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Configuration;
using static System.Console;

namespace TicTacToe
{
    class Client
    {
        private string address = ConfigurationManager.AppSettings["address"]; // ip address of server. Taken from the 'App.config' file
        private int port = int.Parse(ConfigurationManager.AppSettings["port"]); // listened port. Taken from the 'App.config' file

        private IPEndPoint ipPoint { get; set; }
        private Socket socket { get; set; }

        // this method connect the client and the server. Here the server send to the client his character('X' or 'O')
        public string Connect()
        {
            string res = " "; // 'res' is a variable that contains a client character
            try
            {
                WriteLine("Connecting...");
                ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); // create socket that will connect to the server
                socket.Connect(ipPoint); // connection to the server

                WriteLine("Connected.\nWaiting for players...");

                res = Receive(); // here the client recieve his character
                WriteLine("\nGame started!\n");
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
            }
            return res;
        }

        // this method sends a coordinates to the server
        public void Send(string coords)
        {
            try
            {
                socket.Send(Encoding.UTF8.GetBytes(coords)); // send a coordinates in utf-8 encoding
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
            }
        }


        // this method recieves a coordinates from the server
        public string Receive()
        {
            string res = " "; // 'res' is a variable that contains an opponent move coordinates
            try
            {
                byte[] data = new byte[256];
                StringBuilder builder = new StringBuilder();
                int bytes = 0;

                // receiving a bytes from the server
                do
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                }
                while (socket.Available > 0);
                res = builder.ToString();
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
            }
            return res;
        }

        // this method disconnects the client from the server.
        public void Disconnect()
        {
            try
            {
                socket.Send(Encoding.UTF8.GetBytes(".")); // send to the server end of connection signal
                socket.Shutdown(SocketShutdown.Both); // end of listening
                socket.Close(); // close connection
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
            }
        }
    }
}
