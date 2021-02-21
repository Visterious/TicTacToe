using System.Collections.Generic;
using static System.Console;

namespace TicTacToe
{
    class Line
    {
        public List<Point> Points { get; set; }

        public Line()
        {
            Points = new List<Point>();
        }

        // return true if some point in line
        public bool IsPointInLine(Point p)
        {
            for (int i = 0; i < Points.Count; i++)
            {
                if (p.Row == Points[i].Row && p.Col == Points[i].Col)
                {
                    return true;
                }
            }
            return false;
        }

        // print coordinates of line points on console
        public void Print()
        {
            Write("[");
            for (int i = 0; i < Points.Count; i++)
            {
                Write($"({ Points[i].Row}, { Points[i].Col})");
                if (i < Points.Count - 1) Write(", ");
            }
            WriteLine("]");
        }
    }
}
