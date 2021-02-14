using System;
using System.Collections.Generic;
using static System.Console;

namespace TicTacToe
{
    class Program
    {
        static int sizeOfField;
        static int lengthOfWinComb;

        static void Main(string[] args)
        {
            //List<Line> lines = new List<Line>();

            //for (int i = 0; i < 5; i++)
            //{
            //    lines.Add(new Line(i, i));
            //    WriteLine($"line{i}, row: {lines[i].Row} col: {lines[i].Col}");
            //}


            
            while (!SetParameters()) { }
            var field = new Field(sizeOfField, lengthOfWinComb);

            do
            {
                field.ChangePlayer();
                field.PrintField();
                field.SetValue();
            } while (!field.CheckWin());
            field.PrintWinner();
        }

        static public bool SetParameters()
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
    }
}
