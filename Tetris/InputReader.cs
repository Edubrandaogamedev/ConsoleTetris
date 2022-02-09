using System.Numerics;
namespace Tetris
{
    public class InputReader
    {

        public delegate void MovementKeyPressed(Vector2 direction);
        public static event MovementKeyPressed? onMovementKeyPressed;
        public static event MovementKeyPressed? onRotationKeyPressed;
        private static ConsoleKeyInfo input = new ConsoleKeyInfo();
        public void Initialize()
        {
            Thread inputThread = new Thread(ReadInput);
            inputThread.Start();
        }
        private void ReadInput()
        {
            while (true)
            {
                input = Console.ReadKey(true);
                switch (input.Key)
                {
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        onMovementKeyPressed?.Invoke(direction: new Vector2(-1,0));
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        onMovementKeyPressed?.Invoke(direction: new Vector2(1,0));
                        break;
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        onRotationKeyPressed?.Invoke(direction: new Vector2(0,1));
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        onRotationKeyPressed?.Invoke(direction: new Vector2(0,-1));
                        break;
                }
                input = new ConsoleKeyInfo();
            }
        }
    }
}