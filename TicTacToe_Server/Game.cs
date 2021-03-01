using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace TicTacToe_Server
{
    class Game : IDisposable
    {

        public List<Socket> players { get; set; } = new List<Socket>(); // list of player sockets in game


        public Game(Socket p1, Socket p2)
        {
            // adding sockets to 'players'
            players.Add(p1);
            players.Add(p2);
            Console.WriteLine($"Game created. Players: {p1.RemoteEndPoint}, {p2.RemoteEndPoint}");
        }

        ~Game()
        {
            Console.WriteLine("Game ended.");
        }

        // this method handle messages from a client
        public void Handler(Socket p)
        {
            try
            {
                while (true) // handle cycle
                {
                    // empty variables for recieved data
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    byte[] data = new byte[256];

                    // recieving data in cycle while a client sends bytes
                    do
                    {
                        bytes = p.Receive(data);
                        builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                    }
                    while (p.Available > 0);

                    Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString()); // message in string

                    // if message == '.' then game ends and handle cycle ends
                    if (builder.ToString() == ".")
                    {
                        break;
                    }

                    // send data to another players besides the sender
                    foreach (Socket player in players)
                    {
                        if (player != p)
                        {
                            player.Send(data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine($"{p.RemoteEndPoint} leave");
        }

        // dispose methods. I don't think it actually works
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                players = null;
            }
        }
    }
}
