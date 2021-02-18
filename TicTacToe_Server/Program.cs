using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace TicTacToe_Server
{
    class Program
    {
        static int port { get; set; } = 8005;
        static List<Socket> connections { get; set; } = new List<Socket>();

        static void Main(string[] args)
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);

            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listenSocket.Bind(ipPoint);

                listenSocket.Listen(10);

                Console.WriteLine("Сервер запущен. Ожидание подключений...");


                while (true)
                {
                    Socket handler = listenSocket.Accept();
                    Console.WriteLine(handler.RemoteEndPoint);

                    connections.Add(handler);

                    if (connections.Count >= 2)
                    {
                        newGame(connections[0], connections[1]);
                        connections.RemoveAt(0);
                        connections.RemoveAt(0);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        async static void newGame(Socket p1, Socket p2)
        {
            await Task.Run(() =>
            {
                Game game = new Game(p1, p2);
                Thread gameThread1 = new Thread(new ThreadStart(() => game.Handler(p1)));
                gameThread1.Start();
                Thread gameThread2 = new Thread(new ThreadStart(() => game.Handler(p2)));
                gameThread2.Start();
            });
        }
    }
}
