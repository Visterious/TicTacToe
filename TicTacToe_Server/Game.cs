using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe_Server
{
    class Game
    {

        public List<Socket> players { get; set; } = new List<Socket>();


        public Game(Socket p1, Socket p2)
        {
            players.Add(p1);
            players.Add(p2);
            Console.WriteLine($"Game created. Players: {p1.RemoteEndPoint}, {p2.RemoteEndPoint}");
        }

        ~Game()
        {
            Console.WriteLine("Game ended.");
        }

        public void Handler(Socket p)
        {
            while (true)
            {
                StringBuilder builder = new StringBuilder();
                int bytes = 0;
                byte[] data = new byte[256];

                do
                {
                    bytes = p.Receive(data);
                    builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                }
                while (p.Available > 0);

                Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());

                if (builder.ToString() == ".")
                {
                    break;
                }

                foreach (Socket player in players)
                {
                    if (player != p)
                    {
                        player.Send(data);
                    }
                }
            }
            Console.WriteLine($"{p.RemoteEndPoint} leave");
        }
    }
}
