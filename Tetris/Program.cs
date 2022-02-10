using System.Diagnostics;
using System;
namespace Tetris
{
    class Program
    {
        private static InputReader inputReader = new InputReader();
        private static TetrisBoard board = new TetrisBoard();
        public static bool isGameOver = false;
        private static void Main(string[] args)
        {
            CreateWindow(TetrisBoard.BoardRows,TetrisBoard.BoardCols+TetrisBoard.InfoCols);
            inputReader.Initialize();
            board.CreateTetrisUI();
            board.CreateNewPiece();
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            while(!isGameOver)
            {
                TimeSpan timePassed = stopWatch.Elapsed;
                if (timePassed.TotalSeconds >= 0.7f) //tick time
                {
                    board.ForceMovePiece();
                    board.UpdateTetrisBoard();
                    board.AddScore(1);
                    stopWatch.Restart();
                } 
            }
        }
        private static void CreateWindow(int boardRows,int boardColumns)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.CursorVisible = false;
            Console.WindowHeight = TetrisBoard.BoardRows + 2;
            Console.WindowWidth = TetrisBoard.BoardCols + TetrisBoard.InfoCols + 3;
            Console.BufferHeight = TetrisBoard.BoardRows + 3;
            Console.BufferWidth = TetrisBoard.BoardCols + TetrisBoard.InfoCols + 10;
        }
    }
}