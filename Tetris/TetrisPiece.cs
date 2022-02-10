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
            if (onCollision != null)
            {
                foreach(Delegate d in onCollision.GetInvocationList())
                {
                    onCollision -= (Collision)d;
                }
            }
            GC.SuppressFinalize(this);
        }
        public void ForcePieceDown()
        {
            Vector2 downDir = new Vector2(0,1);
            if (piece == null || board == null) return;
            {
                if (currentPosition.Y < board.GetLength(0)-1-piece.GetLength(0))
                    currentPosition += downDir;
            }
            CheckCollision();
        }
        private void CheckCollision()
        {
            if (piece == null || board == null) return;
            if (currentPosition.X + piece.GetLength(1) > board.GetLength(1))
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
            if (_direction.X == 1) //left (right person perspective)
            {
                if (currentPosition.X + piece.GetLength(1) < board.GetLength(1))
                {
                    bool movementAllowed = true;
                    for (int row = 0; row < piece.GetLength(0); row++)
                    {
                        for (int col = 0; col < piece.GetLength(1); col++)
                        {
                            if (piece[row, col] && !board[(int)currentPosition.Y + row, (int)currentPosition.X +col+(int)_direction.X])
                                movementAllowed = true;
                            else if (piece[row,col] && board[(int)currentPosition.Y + row, (int)currentPosition.X +col+(int)_direction.X])
                                movementAllowed = false;
                        }
                    }
                    if (movementAllowed)
                        currentPosition += _direction;
                }
            }
            else if(_direction.X == -1) //right (left person perspective)
            {
                if (currentPosition.X > 0)
                {
                    bool movementAllowed = true;
                    for (int row = 0; row < piece.GetLength(0); row++)
                    {
                        for (int col = 0; col < piece.GetLength(1); col++)
                        {
                            if (piece[row, col] && !board[(int)currentPosition.Y + row, (int)currentPosition.X+(int)_direction.X])
                                movementAllowed = true;
                            else if (piece[row,col] && board[(int)currentPosition.Y + row, (int)currentPosition.X+(int)_direction.X])
                                movementAllowed = false;
                        }
                    }
                    if (movementAllowed)
                        currentPosition += _direction;
                }
            }
            CheckCollision();
        }
        private void TetrisRotation(Vector2 _direction)
        {
            if( piece == null) return;
            bool[,] previewRotatedPiece = new bool[piece.GetLength(1), piece.GetLength(0)];
            if (_direction.Y == 1)
            {   
                for (int row = 0; row < piece.GetLength(0); row++)
                {
                    for (int col = 0; col < piece.GetLength(1); col++)
                    {
                        previewRotatedPiece[col, piece.GetLength(0) - row - 1] = piece[row, col];
                    }
                }
            }
            else if (_direction.Y == -1)
            {
                for (int row = 0; row < piece.GetLength(0); row++)
                {
                    for (int col = 0; col < piece.GetLength(1); col++)
                    {
                        previewRotatedPiece[piece.GetLength(1) - col - 1 , row] = piece[row, col];
                    }
                }
            }
            if (CanRotate(previewRotatedPiece))
            {
                piece = previewRotatedPiece;
                CheckCollision();
            }
        }
        private bool CanRotate(bool[,] _targetPiece)
        {
            if (_targetPiece == null || board == null) return false;
            if (currentPosition.X > board.GetLength(1) - _targetPiece.GetLength(1))
            {
                return false;
            }
            if (currentPosition.Y + _targetPiece.GetLength(0) == board.GetLength(0)-1)
            {
                return false;
            }
            for (int row = 0; row < _targetPiece.GetLength(0); row++)
            {
                for (int col = 0; col < _targetPiece.GetLength(1); col++)
                {
                    if (_targetPiece[row, col] && board[(int)currentPosition.Y + row + 1, (int)currentPosition.X + col])
                    {
                        return false;
                    }
                }
            }
            return true;
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