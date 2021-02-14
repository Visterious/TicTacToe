using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace TicTacToe
{
    class Field
    {
        private List<Line> lines { get; set; }
        private char[,] values { get; set; }
        private char move { get; set; } = 'O';
        private int sizeOfField { get; set; }
        private int lengthOfWinComb = 3;

        public Field(int sof, int lowc)
        {
            sizeOfField = sof;
            lengthOfWinComb = lowc;
            values = setValuesArray();
            lines = setLinesList();
            //for (int i = 0; i < (8*(SizeOfField - 2)*(SizeOfField - 2)); i++)
            //{
            //    Write($"{i} ");
            //    lines[i].Print();
            //}
        }

        public bool CheckWin()
        {
            List<Line> copyOfLines = new List<Line>(lines);

            for (int i = 0; i < sizeOfField; i++)
            {
                for (int j = 0; j < sizeOfField; j++)
                {
                    if (values[i, j] == ' ')
                    {
                        copyOfLines = DeleteLines(i, j, copyOfLines);
                    }
                }
            }

            //WriteLine();
            //for (int i = 0; i < copyOfLines.Count; i++)
            //{
            //    Write($"{i} ");
            //    copyOfLines[i].Print();
            //}
            if (copyOfLines.Count > 0)
            {
                bool res;
                for (int i = 0; i < copyOfLines.Count; i++)
                {
                    res = true;
                    for (int j = 0; j < copyOfLines[i].Points.Count; j++)
                    {
                        res &= (values[copyOfLines[i].Points[j].Row, copyOfLines[i].Points[j].Col] == move);
                    }
                    if (res) return true;
                }
            }
            return false;
        }

        public void ChangePlayer()
        {
            if (move == 'X')
            {
                move = 'O';
            }
            else
            {
                move = 'X';
            }
        }

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
                if (values[rowN, columnN] != ' ')
                {
                    WriteLine("It already exists");
                }
            } while (values[rowN, columnN] != ' ');
            values[rowN, columnN] = move;
        }

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

        public void PrintWinner()
        {
            PrintField();
            WriteLine();
            if (move == 'X')
                WriteLine("The current winner is player of X");
            else
                WriteLine("The current winner is player of O");
            WriteLine();
        }

        private char[,] setValuesArray()
        {
            char[,] template_values = new char[sizeOfField, sizeOfField];
            for (int i = 0; i < sizeOfField; i++)
            {
                for (int j = 0; j < sizeOfField; j++)
                {
                    template_values[i, j] = ' ';
                }
            }
            return template_values;
        }

        private List<Line> setLinesList()
        {
            lines = new List<Line>();
            for (int i = 0; i <= sizeOfField - lengthOfWinComb; i++)
            {
                for (int j = 0; j <= sizeOfField - lengthOfWinComb; j++)
                {
                    AddLines(i, j);
                }
            }
            return lines;
        }

        private void AddLines(int x, int y)
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

        private List<Line> DeleteLines(int x, int y, List<Line> copyOfLines)
        {
            for (int i = 0; i < copyOfLines.Count; i++)
            {
                //Write($"\n{i} -> X:{x}, Y:{y}");
                if (copyOfLines[i].IsPointInLine(new Point(x, y)))
                {
                    //Write(" - Deleted");
                    copyOfLines.RemoveAt(i);
                    i--;
                }
            }
            return copyOfLines;
        }
    }
}
