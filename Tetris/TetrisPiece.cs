using System.Numerics;
namespace Tetris
{
    public class TetrisPiece : IDisposable
    {
        public delegate void Collision();
        public event Collision? onCollision;
        private TetrisAssets tetrisAssets = new TetrisAssets();
        private bool [,]? board;
        private bool[,]? piece;
        public bool[,]? Piece { get => piece;}
        private Vector2 currentPosition;
        public Vector2 CurrentPosition { get => currentPosition;}

        public TetrisPiece(bool [,] _board)
        {
            this.board = _board;
            InputReader.onMovementKeyPressed += TetrisMovement;   
            InputReader.onRotationKeyPressed +=  TetrisRotation;
            SpawnPiece();
        }
        public void Dispose()
        {
            InputReader.onMovementKeyPressed -= TetrisMovement;   
            InputReader.onRotationKeyPressed -=  TetrisRotation;
            foreach(Delegate d in onCollision.GetInvocationList())
            {
                onCollision -= (Collision)d;
            }
            GC.SuppressFinalize(this);
        }
        // public void MoveDown(bool [,] _board)
        // {
        //     if (piece == null) return;
        //     Vector2 downDir = new Vector2(0,1);
        //     if (currentPosition.Y < boardRows-1-piece.GetLength(0))
        //         currentPosition += downDir;
        // }
        
        public void CheckCollision()
        {
            if (piece == null || board == null) return;
            if (currentPosition.X > board.GetLength(1) - piece.GetLength(1))
            {
                onCollision?.Invoke();
            }
            if (currentPosition.Y + piece.GetLength(0) == board.GetLength(0)-1)
            {
                onCollision?.Invoke();
            }
            for (int row = 0; row < piece.GetLength(0); row++)
            {
                for (int col = 0; col < piece.GetLength(1); col++)
                {
                    if (piece[row, col] && board[(int)currentPosition.Y + row + 1, (int)currentPosition.X + col])
                    {
                        onCollision?.Invoke();
                    }
                }
            }
        }
        private void TetrisMovement(Vector2 _direction)
        {
            if (piece == null || board == null) return;
            if (_direction.X == 1)
            {
                if ((currentPosition.X < board.GetLength(1) - piece.GetLength(1)))
                    currentPosition += _direction;
            }
            else if(_direction.X == -1)
            {
                if (currentPosition.X >= 1)
                    currentPosition += _direction;
            }
            else if (_direction.Y == 1)
            {
                if (currentPosition.Y < board.GetLength(0)-1-piece.GetLength(0))
                    currentPosition += _direction;
            }
            CheckCollision();
        }
        private void TetrisRotation(Vector2 _direction)
        {
            //Console.WriteLine(direction);
        }
        private void SpawnPiece()
        {
            if (board == null) return;
            Int16 initialLine = 1;
            piece = tetrisAssets.GetRandomPiece();
            currentPosition = new Vector2(new Random().Next(0,board.GetLength(1)-piece.GetLength(1)),initialLine);
        }
    }
    public class TetrisAssets
    {
        private Random Random = new Random();
        List<bool[,]> tetrisFigures = new List<bool[,]>()
        {
            new bool [,] // I
            {
                {true, true, true, true }
            },
            new bool [,] // O
            {
                {true, true },
                {true, true }
            },
            new bool [,] // T
            {
                {false, true, false},
                {true, true ,true}
            },
            new bool [,] // S
            {
                {false, true, true},
                {true, true, false}
            },
            new bool[,] // Z
            {
                {true, true, false},
                {false, true, true}
            },
            new bool[,] // J
            {
                {false, false, true},
                {true, true, true}
            },
            new bool[,] // L
            {
                {true, false, false},
                {true, true, true}
            }
        };
        public bool[,] GetRandomPiece()
        {
            return tetrisFigures[Random.Next(0, tetrisFigures.Count)];
        }
    }

}