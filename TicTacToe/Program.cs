using static System.Console;

namespace TicTacToe
{
    class Program
    {
        static void Main(string[] args)
        {
            var field = new Field(); // creating a game field

            int i = 0;
            do // game cycle
            {
                field.ChangePlayer(); // change of player's turn
                field.PrintField();
                if (!field.YourTurn) // if not your turn
                {
                    field.Receive(); // you recieve opponent move from the server
                    if (field.CheckWin(field.OpponentMove) || field.CheckDraw(i)) // if opponent wins the game ends
                    {
                        field.Winner = field.OpponentMove;
                        WriteLine("Opponent win!");
                        break;
                    }
                }
                else // if your turn
                {
                    field.SetValue(); // you moves
                }
                field.YourTurn = !field.YourTurn; // change turn
                i++;
            } while (!(field.CheckWin(field.Move) || field.CheckDraw(i))); // check of your win or draw
            if (field.CheckDraw(i)) // draw output
            {
                WriteLine("Draw!");
                field.Disconnect();
            } else // win output
            {
                field.PrintWinner();
                field.Disconnect();
            }
            ReadKey();
        }
    }
}
