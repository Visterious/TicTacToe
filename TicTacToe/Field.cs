using System;
using System.Collections.Generic;
using static System.Console;

namespace TicTacToe
{
    class Field
    {
        public bool YourTurn { get; set; }
        public string Winner { get; set; }
        public string Move { get; set; }
        public string OpponentMove { get; set; }

        private List<Line> lines { get; set; }
        private string[,] values { get; set; }

        private int sizeOfField { get; set; } = 3;
        private int lengthOfWinComb { get; set; } = 3;

        private Client client;

        public Field()
        {
            client = new Client();
            _connect();
            _init();

            Winner = Move;
            values = _setValuesArray();
            lines = _setLinesList();
        }

        ~Field()
        {
            Disconnect();
        }

        public bool CheckWin(string m)
        {
            List<Line> copyOfLines = new List<Line>(lines);

            for (int i = 0; i < sizeOfField; i++)
            {
                for (int j = 0; j < sizeOfField; j++)
                {
                    if (values[i, j] == " ")
                    {
                        copyOfLines = _deleteLines(i, j, copyOfLines);
                    }
                }
            }

            if (copyOfLines.Count > 0)
            {
                bool res;
                for (int i = 0; i < copyOfLines.Count; i++)
                {
                    res = true;
                    for (int j = 0; j < copyOfLines[i].Points.Count; j++)
                    {
                        res &= (values[copyOfLines[i].Points[j].Row, copyOfLines[i].Points[j].Col] == m);
                    }
                    if (res) return true;
                }
            }
            return false;
        }

        public bool CheckDraw(int i)
        {
            return (i >= sizeOfField*sizeOfField);
        }

        // this method changes of player's turn
        public void ChangePlayer()
        {
            if ((Move == "X" && YourTurn) || (Move == "O" && !YourTurn))
            {
                WriteLine($"\nX's turn!\n");
            }
            else if ((Move == "X" && !YourTurn) || (Move == "O" && YourTurn))
            {
                WriteLine($"\nO's turn!\n");
            }
        }

        // player input method (set the coordinates of move)
        public void SetValue()
        {
            int rowN, columnN;
            string row, column;
            bool res;

            do
            {
                do
                {
                    WriteLine("Enter row: ");
                    row = ReadLine();
                    res = int.TryParse(row, out rowN);
                    if (rowN > sizeOfField || rowN <= 0)
                    {
                        WriteLine("Your input incorrect!");
                        res = false;
                    }
                } while (!(res));
                rowN--;
                do
                {
                    WriteLine("Enter column: ");
                    column = ReadLine();
                    res = int.TryParse(column, out columnN);
                    if (columnN > sizeOfField || columnN <= 0)
                    {
                        WriteLine("Your input incorrect!");
                        res = false;
                    }
                } while (!(res));
                columnN--;
                if (values[rowN, columnN] != " ")
                {
                    WriteLine("It already exists");
                }
            } while (values[rowN, columnN] != " ");
            values[rowN, columnN] = Move;
            Send(rowN, columnN);
        }

        // this method sends a packed data
        public void Send(int x, int y)
        {
            client.Send(_pack(x.ToString(), y.ToString()));
        }

        // this method recieves and unpack a data
        public void Receive()
        {
            string[] data = _unpack(client.Receive());
            if (Move == "X")
            {
                values[Convert.ToInt32(data[0]), Convert.ToInt32(data[1])] = "O";
            }
            else
            {
                values[Convert.ToInt32(data[0]), Convert.ToInt32(data[1])] = "X";
            }
        }

        // this method disconnects the client from server
        public void Disconnect()
        {
            client.Disconnect();
        }

        // this method displays the game field
        public void PrintField()
        {
            WriteLine();
            for (int i = 0; i < sizeOfField; i++)
            {
                Write($" {values[i, 0]}");
                for (int j = 1; j < sizeOfField; j++)
                {
                    Write($" | {values[i, j]}");
                }
                WriteLine();
                if (i != sizeOfField - 1)
                {
                    Write($"--");
                    for (int j = 1; j < sizeOfField; j++)
                    {
                        Write($"-+--");
                    }
                    WriteLine("-");
                }
            }
            WriteLine();
        }

        // this method displays the winner 
        public void PrintWinner()
        {
            PrintField();
            WriteLine($"\nThe current winner is player of {Winner}\n");
        }

        // this method connects to the server and gets your move and opponent move
        private void _connect()
        {
            string[] data = _unpack(client.Connect());
            Move = data[0];
            OpponentMove = data[1];
        }

        // this method set start parameters(size of field and length of win combination)
        private bool _setParameters()
        {
            int size;
            WriteLine("Enter a size of field: ");
            string text = ReadLine();
            try
            {
                size = int.Parse(text);
                if (size < 3)
                {
                    WriteLine("Enter a number more then 3\n");
                    return false;
                }
                sizeOfField = size;
            }
            catch (FormatException)
            {
                WriteLine("Enter a number for parameter.\n");
                return false;
            }
            catch (OverflowException)
            {
                WriteLine("Enter a less number\n");
                return false;
            }
            catch (Exception e)
            {
                WriteLine(e);
                return false;
            }

            int length;
            WriteLine("Enter a length of win combination: ");
            text = ReadLine();
            try
            {
                length = int.Parse(text);
                if (length < 3)
                {
                    WriteLine("Enter a number more then 3\n");
                    return false;
                }
                lengthOfWinComb = length;
            }
            catch (FormatException)
            {
                WriteLine("Enter a number for parameter.\n");
                return false;
            }
            catch (OverflowException)
            {
                WriteLine("Enter a less number\n");
                return false;
            }
            catch (Exception e)
            {
                WriteLine(e);
                return false;
            }

            return true;
        }

        // this method creates an array with empty field values
        private string[,] _setValuesArray()
        {
            string[,] template_values = new string[sizeOfField, sizeOfField];
            for (int i = 0; i < sizeOfField; i++)
            {
                for (int j = 0; j < sizeOfField; j++)
                {
                    template_values[i, j] = " ";
                }
            }
            return template_values;
        } 

        // this method operates with _addLines() and help to add lines to the 'lines'
        private List<Line> _setLinesList()
        {
            lines = new List<Line>();
            for (int i = 0; i <= sizeOfField - lengthOfWinComb; i++)
            {
                for (int j = 0; j <= sizeOfField - lengthOfWinComb; j++)
                {
                    _addLines(i, j);
                }
            }
            return lines;
        }

        // this method add lines to the 'lines'
        private void _addLines(int x, int y)
        {
            Line line;

            // add vertical lines
            for (int i = 0; i < lengthOfWinComb; i++)
            {
                line = new Line();
                for (int j = 0; j < lengthOfWinComb; j++)
                {
                    line.Points.Add(new Point(x + i, y + j));
                }
                lines.Add(line);
            }

            // add horizontal lines
            for (int i = 0; i < lengthOfWinComb; i++)
            {
                line = new Line();
                for (int j = 0; j < lengthOfWinComb; j++)
                {
                    line.Points.Add(new Point(x + j, y + i));
                }
                lines.Add(line);
            }

            // add diagonal from left-top to right-bottom
            line = new Line();
            for (int i = 0; i < lengthOfWinComb; i++)
            {
                line.Points.Add(new Point(x + i, y + i));
            }
            lines.Add(line);
            
            // add diagonal from right-top to left-bottom
            line = new Line();
            for (int k = lengthOfWinComb - 1, l = 0; k >= 0; k--, l++)
            {
                line.Points.Add(new Point(x + k, x + l));
            }
            lines.Add(line);
        }

        // this method deletes lines with one or more empty points from 'copyOfLines'
        private List<Line> _deleteLines(int x, int y, List<Line> copyOfLines)
        {
            for (int i = 0; i < copyOfLines.Count; i++)
            {
                if (copyOfLines[i].IsPointInLine(new Point(x, y)))
                {
                    copyOfLines.RemoveAt(i);
                    i--;
                }
            }
            return copyOfLines;
        }

        // this method provides the exchange of starting parameters 
        private void _init()
        {
            if (Move == "X")
            {
                while (!_setParameters()) { }
                client.Send(_pack(sizeOfField.ToString(), lengthOfWinComb.ToString()));
                YourTurn = true;
            }
            else
            {
                WriteLine("Waiting for parameters...");
                string[] arr = _unpack(client.Receive());
                sizeOfField = Convert.ToInt32(arr[0]);
                lengthOfWinComb = Convert.ToInt32(arr[1]);
                YourTurn = false;
            }
        }

        // this method pack your data from (string str1, string str2) to "<str1>;<str2>". From "1" and "2" to "1;2"
        private string _pack(string x, string y)
        {
            string res = x + ";" + y;
            return res;
        }

        // this method unpack your data from "<str1>;<str2>" to {str1, str2}. From "1;2" to {"1", "2"}
        private string[] _unpack(string str)
        {

            string[] arr = str.Split(';');
            return arr;
        }
    }
}
