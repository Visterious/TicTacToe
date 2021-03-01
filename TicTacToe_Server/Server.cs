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
        // address and port that server must listens
        private static string address = ConfigurationManager.AppSettings["address"];
        private static int port = int.Parse(ConfigurationManager.AppSettings["port"]);

        static List<Socket> connections { get; set; } = new List<Socket>(); // list of player sockets that connects to the server

        static void Main(string[] args)
        {
            // create a listen socket
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listenSocket.Bind(ipPoint); // listen our ip:port

                listenSocket.Listen(10); // we can listen listen to 10 clients maximum

                Console.WriteLine("Сервер запущен. Ожидание подключений...");


                while (true) // cycle of accepting connections
                {
                    Socket handler = listenSocket.Accept(); // accept connection
                    Console.WriteLine(handler.RemoteEndPoint);

                    connections.Add(handler); // add new connection(socket) to 'connections'

                    // if count of connections >= 2 then start new game and remove this connections from 'connections'
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

        // method that start new game
        async static void newGame(Socket p1, Socket p2)
        {
            await Task.Run(() =>
            {
                using (Game game = new Game(p1, p2)) // create a game
                {
                    try
                    {
                        // send to players their symbols 
                        p1.Send(Encoding.UTF8.GetBytes("X;O"));
                        p2.Send(Encoding.UTF8.GetBytes("O;X"));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    // start handling players messages in different threads
                    Thread gameThread1 = new Thread(new ThreadStart(() => game.Handler(p1)));
                    gameThread1.Start();
                    Thread gameThread2 = new Thread(new ThreadStart(() => game.Handler(p2)));
                    gameThread2.Start();
                    gameThread1.Join();
                }
            });
        }
    }
}
