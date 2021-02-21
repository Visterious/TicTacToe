using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System.Configuration;

namespace TicTacToe_Server
{
    class Server
    {
        private static string address = ConfigurationManager.AppSettings["address"];
        private static int port = int.Parse(ConfigurationManager.AppSettings["port"]);

        static List<Socket> connections { get; set; } = new List<Socket>();

        static void Main(string[] args)
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

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
                try
                {
                    p1.Send(Encoding.UTF8.GetBytes("X;O"));
                    p2.Send(Encoding.UTF8.GetBytes("O;X"));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Thread gameThread1 = new Thread(new ThreadStart(() => game.Handler(p1)));
                gameThread1.Start();
                Thread gameThread2 = new Thread(new ThreadStart(() => game.Handler(p2)));
                gameThread2.Start();
            });
        }
    }
}
