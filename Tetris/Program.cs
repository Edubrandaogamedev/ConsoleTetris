﻿using System;
using System.Diagnostics;

namespace Tetris
{
    class Program
    {
        private static InputReader inputReader = new InputReader();
        private static TetrisBoard board = new TetrisBoard();
        private static void Main(string[] args)
        {
            CreateWindow(TetrisBoardData.TetrisRow,TetrisBoardData.TetrisCols+TetrisBoardData.InfoCols);
            board.CreateNewPiece();
            inputReader.Initialize();
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            while(true)
            {
                TimeSpan timePassed = stopWatch.Elapsed;
                if (timePassed.TotalSeconds >= 0.7f) //the reason I choose 0.7 seconds because it's feel better to play
                {
                    board.ForceMovePiece();
                    board.UpdateTetrisBoard();
                    stopWatch.Restart();
                } 
            }
        }
        private static void CreateWindow(int boardRows,int boardColumns)
        {
            Console.CursorVisible = false;
            Console.WindowHeight = boardRows+ 2;
            Console.WindowWidth = boardColumns + 3;
            Console.BufferHeight = boardRows+ 2;
            Console.BufferWidth = boardColumns + 3;
        }
    }
}