﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe
{
    class Point
    {
        public int Row { get; set; }
        public int Col { get; set; }

        public Point(int row, int col)
        {
            Row = row;
            Col = col;
        }
    }
}