using System.Numerics;

namespace Tetris
{
    public class TetrisBoard
    {
        public const Int16 BoardRows = 20;
        public const Int16 BoardCols = 20;
        public const Int16 InfoCols = 20;
        private Int16 score = 0;
        private Int16 scorePerLine = 10;
        private bool[,]? tetrisBoardMap;
        private TetrisBoardUI? boardUI;
        private TetrisPiece? currentPiece;
        //private TetrisPiece? nextPiece;
        public TetrisBoard()
        {
            tetrisBoardMap = new bool[BoardRows,BoardCols];
            boardUI = new TetrisBoardUI(BoardRows,BoardCols,InfoCols);
        }
        public void CreateTetrisUI()
        {
            if (boardUI == null || tetrisBoardMap == null) return;
            boardUI.DrawFrame();
            boardUI.DrawInfo();
            boardUI.DrawTetrisBoard(tetrisBoardMap);
        }
        public void ForceMovePiece()
        {
            if (currentPiece == null) return;
                currentPiece.ForcePieceDown();
        }
        public void UpdateTetrisBoard()
        {
            if (boardUI == null || tetrisBoardMap == null) return;
            boardUI.DrawTetrisBoard(tetrisBoardMap);
            if (currentPiece != null && currentPiece.Piece != null)
            {
                boardUI.DrawTetrisPiece(currentPiece.Piece,currentPiece.CurrentPosition,currentPiece.PieceDescription);
            }
            boardUI.ChangeScore(score);
        }
        public void CreateNewPiece()
        {
            if (tetrisBoardMap == null) return;
            currentPiece = new TetrisPiece(tetrisBoardMap);
            currentPiece.onCollision += OnPieceCollision;
            currentPiece.SpawnPiece();
        }
        public void AddScore(Int16 _value)
        {
            score += _value;
            if (boardUI == null) return;
        }
        private void OnPieceCollision()
        {
            AddPieceToStaticPosition();
            if (currentPiece == null || currentPiece.Piece == null) return;
            if (IsGameOver())
                return;
            currentPiece.Dispose();
            CreateNewPiece();
            score += (short)(GetFullLines() * scorePerLine);
            if (boardUI == null) return;
            boardUI.ChangeScore(score);
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
        }
        private bool IsGameOver()
        {
            if (currentPiece == null || currentPiece.Piece == null) return false;
            for (int row = 0; row < currentPiece.Piece.GetLength(0); row++)
            {
                if (currentPiece.CurrentPosition.Y - row < 1)
                {
                    Program.isGameOver = true;
                    if (boardUI != null)
                        boardUI.DrawGameOver();
                    return true;
                }
            }
            return false;
        }
        private Int16 GetFullLines()
        {
            if (tetrisBoardMap == null) return 0;
            Int16 lines = 0;
            for (int row = 0; row < tetrisBoardMap.GetLength(0); row++)
            {
                bool isRowFull = true;
                for (int col = 0; col < tetrisBoardMap.GetLength(1); col++)
                {
                    if (!tetrisBoardMap[row, col])
                    {
                        isRowFull = false;
                        break;
                    }
                }
                if (isRowFull)
                {
                    for (int rowToMove = row; rowToMove >= 1; rowToMove--)
                    {
                        for (int col = 0; col < tetrisBoardMap.GetLength(1); col++)
                        {
                            tetrisBoardMap[rowToMove, col] = tetrisBoardMap[rowToMove - 1, col];
                        }
                    }

                    lines++;
                }
            }
            return lines;
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
        public TetrisBoardUI(int tetrisRows,int tetrisCols, int infoCols)
        {
            this.tetrisRows = tetrisRows;
            this.tetrisCols = tetrisCols;
            this.infoCols = infoCols;
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
            WriteAtPosition("Score:", 5, tetrisCols + 3);
            WriteAtPosition("0", 6, tetrisCols + 3);
            
        }
        public void DrawTetrisBoard(bool [,] _board)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            for (int row = 0; row < _board.GetLength(0); row++)
            {
                string line = "";
                for (int col = 0; col < _board.GetLength(1); col++)
                {
                    if (_board[row, col])
                    {
                        line += PieceSymbol;
                    }
                    else
                    {
                        line += " ";
                    }
                }
                WriteAtPosition(line, row + 1, 1);
            }
        }
        public void DrawTetrisPiece(bool[,] _pieceType, Vector2 _piecePosition, string _pieceDescription)
        {

            for (int row = 0; row < _pieceType.GetLength(0); row++)
            {
                for (int col = 0; col < _pieceType.GetLength(1); col++)
                {
                    if (_pieceType[row, col])
                    {
                        SetPieceColor(_pieceDescription);
                        WriteAtPosition(PieceSymbol, row + (int)_piecePosition.Y+1, col + 1 + (int)_piecePosition.X);
                    }
                }
            }
        }
        public void DrawGameOver()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            WriteAtPosition("GameOver", 11, tetrisCols + 3);
        }
        public void ChangeScore(int _score)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            WriteAtPosition(_score.ToString(),6, tetrisCols +3);
        }
        private void WriteAtPosition(string text, int row, int col)
        {
            Console.SetCursorPosition(col, row);
            Console.Write(text);
        }
        private void SetPieceColor(string _pieceDescription)
        {
            switch (_pieceDescription)
            {
                case "I":
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case "O":
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                case "T":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case "S":
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    break;
                case "Z":
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case "L":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case "J":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;
            }
        }
    }
    
}