using System;
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
            while(true)
            {
                inputReader.ReadInput();
                board.UpdateTetrisBoard();
                // if (board.TetrisBoardMap != null)
                // {
                //     // if (currentPiece.CheckCollision(board.TetrisBoardMap))
                //     // {
                //     //     board.AddPieceToStaticPosition(currentPiece);
                //     //     currentPiece.Dispose();
                //     //     currentPiece = new TetrisPiece(TetrisBoardData.TetrisRow,TetrisBoardData.TetrisCols);
                //     //     //int lines = CheckForFullLines();
                //     //     //add points to score
                //     //     //Score += ScorePerLines[lines] * Level;
                //     //     //CurrentFigure = NextFigure;
                //     // }
                // }
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