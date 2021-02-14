using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace TicTacToe
{
    class Line
    {
        public List<Point> Points;

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

        public void Print()
        {
            //WriteLine($"[({Points[0].Row}, {Points[0].Col}), ({Points[1].Row}, {Points[1].Col}), ({Points[2].Row}, {Points[2].Col})]");
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
