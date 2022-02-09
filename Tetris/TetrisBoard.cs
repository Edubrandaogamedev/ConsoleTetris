using System.Numerics;

namespace Tetris
{
    public class TetrisBoard
    {
        private Int16 boardRows = 20;
        private Int16 boardCols = 20;
        private Int16 infoCols = 20;
        private bool[,]? tetrisBoardMap;
        public bool[,]? TetrisBoardMap { get => tetrisBoardMap;}
        private TetrisBoardUI? boardUI;
        private TetrisPiece? currentPiece;
        public TetrisBoard()
        {
            tetrisBoardMap = new bool[boardRows,boardCols];
            boardUI = new TetrisBoardUI(boardRows,boardCols,infoCols);
            boardUI.DrawFrame();
            boardUI.DrawInfo();
            boardUI.DrawTetrisBoard(tetrisBoardMap);
        }
        public void UpdateTetrisBoard()
        {
            if (boardUI == null || tetrisBoardMap == null) return;
            boardUI.DrawTetrisBoard(tetrisBoardMap);
            if (currentPiece != null && currentPiece.Piece != null)
            {
                boardUI.DrawTetrisPiece(currentPiece.Piece,currentPiece.CurrentPosition);
            }
        }
        public void CreateNewPiece()
        {
            if (tetrisBoardMap == null) return;
            currentPiece = new TetrisPiece(tetrisBoardMap);
            currentPiece.onCollision += AddPieceToStaticPosition;
        }
        private void AddPieceToStaticPosition()
        {
            if (currentPiece == null || currentPiece.Piece == null || tetrisBoardMap == null) return;
            for (int row = 0; row < currentPiece.Piece.GetLength(0); row++)
            {
                for (int col = 0; col < currentPiece.Piece.GetLength(1); col++)
                {
                    if (currentPiece.Piece[row, col])
                    {
                        tetrisBoardMap[(int)currentPiece.CurrentPosition.Y + row, (int)currentPiece.CurrentPosition.X + col] = true;
                    }
                }
            }
            currentPiece.Dispose();
            CreateNewPiece();
            // CurrentFigure = NextFigure;
            // GetNextFigure();
        }
    }
    public class TetrisBoardUI
    {
        private const char UpperLeftBorderSymbol = '╔';
        private const char UpperRightBorderSymbol = '╗';
        private const char BottomRightBorderSymbol = '╝';
        private const char BottomLeftBorderSymbol = '╚';
        private const char LineSymbol = '═';
        private const char ColumnSymbol = '║';
        private const char TUpperSymbol = '╦';
        private const char TBottomSymbol = '╩';
        private const string PieceSymbol = "#";
        private int tetrisRows;
        private int tetrisCols;
        private int infoCols;
        private bool[,]? tetrisField;
        public TetrisBoardUI(int tetrisRows,int tetrisCols, int infoCols)
        {
            this.tetrisRows = tetrisRows;
            this.tetrisCols = tetrisCols;
            this.infoCols = infoCols;
            tetrisField = new bool[tetrisRows,tetrisCols];
        }
        public void DrawFrame()
        {
            string firstLine = "";
            firstLine += UpperLeftBorderSymbol;
            firstLine += new string(LineSymbol, tetrisCols);
            firstLine += TUpperSymbol;
            firstLine += new string(LineSymbol, infoCols);
            firstLine += UpperRightBorderSymbol;
            firstLine += "\n";
            string middleLine = "";
            for (int i = 0; i < tetrisRows; i++)
            {
                middleLine += ColumnSymbol;
                middleLine += new string(' ', tetrisCols) + ColumnSymbol + new string(' ', infoCols) + ColumnSymbol + "\n";
            }
            string endLine = "";
            endLine += BottomLeftBorderSymbol;
            endLine += new string(LineSymbol, tetrisCols);
            endLine += TBottomSymbol;
            endLine += new string('═', infoCols);
            endLine += BottomRightBorderSymbol;
            string borderFrame = firstLine + middleLine + endLine;
            Console.Write(borderFrame);
        }
        public void DrawInfo()
        {
            Write("Score:", 5, tetrisCols + 3);
            Write("0", 7, tetrisCols + 3);
            Write("High Score:", 9, tetrisCols + 3);
            //Write(HighScore.ToString(), 11, board.TetrisCols + 3);
            Write("Next figure:", 13, tetrisCols + 3);
            // DrawNextFigure();
            Write("Keys:", 18, tetrisCols + 3);
            Write("  ^  ", 19, tetrisCols + 3);
            Write("<   >", 20, tetrisCols + 3);
            Write("  v ", 21, tetrisCols + 3);
        }
        public void DrawTetrisBoard(bool [,] _board)
        {

            for (int row = 0; row < _board.GetLength(0); row++)
            {
                string line = "";
                for (int col = 0; col < _board.GetLength(1); col++)
                {
                    if (_board[row, col])
                    {
                        line += $"{PieceSymbol}";
                    }
                    else
                    {
                        line += " ";
                    }
                }
                Write(line, row + 1, 1);
            }
        }
        public void DrawTetrisPiece(bool[,] _pieceType, Vector2 _piecePosition)
        {

            for (int row = 0; row < _pieceType.GetLength(0); row++)
            {
                for (int col = 0; col < _pieceType.GetLength(1); col++)
                {
                    if (_pieceType[row, col])
                    {
                        Write($"{PieceSymbol}", row + 1 + (int)_piecePosition.Y, col + 1 + (int)_piecePosition.X);
                    }
                }
            }
        }
        public void ChangeScore(int _score)
        {
            Write(_score.ToString(),7, tetrisCols +3);
        }
        private void Write(string text, int row, int col)
        {
            Console.SetCursorPosition(col, row);
            Console.Write(text);
        }
    }
    
}