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
        private string pieceDescription = "";
        public string PieceDescription {get => pieceDescription;}
        private Vector2 currentPosition;
        public Vector2 CurrentPosition { get => currentPosition;}
        public TetrisPiece(bool [,] _board)
        {
            this.board = _board;
            InputReader.onMovementKeyPressed += TetrisMovement;   
            InputReader.onRotationKeyPressed +=  TetrisTryRotation;
        }
        public void Dispose()
        {
            InputReader.onMovementKeyPressed -= TetrisMovement;   
            InputReader.onRotationKeyPressed -=  TetrisTryRotation;
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
                if (currentPosition.Y + piece.GetLength(0) < board.GetLength(0)-1) //board row length - 1, because the border
                    currentPosition += downDir;
            }
            CheckCollision();
        }
        public void SpawnPiece()
        {
            if (board == null) return;
            Int16 initialLine = 1;
            var randomPiece = tetrisAssets.GetRandomPiece();
            piece = randomPiece.Key;
            pieceDescription = randomPiece.Value;
            currentPosition = new Vector2(new Random().Next(0,board.GetLength(1)-piece.GetLength(1)),initialLine);
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
                    if (piece[row, col] && board[(int)currentPosition.Y + row +1, (int)currentPosition.X + col])
                    {
                        onCollision?.Invoke();
                    }
                }
            }
        }
        private void TetrisMovement(Vector2 _direction)
        {
            if (piece == null || board == null) return;
            if (_direction.X == 1) //left (right user perspective)
            {
                if (currentPosition.X + piece.GetLength(1) < board.GetLength(1))
                {
                    bool movementAllowed = false;
                    for (int row = 0; row < piece.GetLength(0); row++)
                    {
                        if (piece[row, piece.GetLength(1)-1] && !board[(int)currentPosition.Y + row, (int)currentPosition.X +piece.GetLength(1)-1+(int)_direction.X])
                            movementAllowed = true;
                        else if (piece[row,piece.GetLength(1)-1] && board[(int)currentPosition.Y + row, (int)currentPosition.X +piece.GetLength(1)-1+(int)_direction.X])
                            movementAllowed = false;
                    }
                    if (movementAllowed)
                        currentPosition += _direction;
                }
            }
            else if(_direction.X == -1) //right (left user perspective)
            {
                if (currentPosition.X > 0)
                {
                    bool movementAllowed = false;
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
        private void TetrisTryRotation(Vector2 _direction)
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
    }
    public class TetrisAssets
    {
        private Random Random = new Random();
        private Dictionary<bool[,],string> pieces = new Dictionary<bool[,], string>()
        {
            {
                new bool[,] {{true,true,true,true}},"I"
            },
            {
                new bool[,] 
                {
                    {true,true},
                    {true,true}
                }, 
                "O"
            },
            {
                new bool [,]
                {
                    {false, true, false},
                    {true, true ,true}
                },
                "T"
            },
            {
                new bool [,]
                {
                    {false, true, true},
                    {true, true, false}
                },
                "S"
            },
            {
                new bool[,] // Z
                {
                    {true, true, false},
                    {false, true, true}
                },
                "Z"
            },
            {
                new bool[,] // J
                {
                    {false, false, true},
                    {true, true, true}
                },
                "J"
            },
            {
                new bool[,] // L
                {
                    {true, false, false},
                    {true, true, true}
                },
                "L"
            }
        };
        public KeyValuePair<bool[,],string> GetRandomPiece()
        {
            return pieces.ElementAt(Random.Next(0,pieces.Count));
        }

    }

}