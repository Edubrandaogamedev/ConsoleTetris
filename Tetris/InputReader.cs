using System.Numerics;
namespace Tetris
{
    public class InputReader
    {

        public delegate void MovementKeyPressed(Vector2 direction);
        public static event MovementKeyPressed? onMovementKeyPressed;
        public static event MovementKeyPressed? onRotationKeyPressed;
        private static ConsoleKeyInfo input = new ConsoleKeyInfo();
        public void ReadInput()
        {
            input = Console.ReadKey(true);
            switch (input.Key)
            {
                case ConsoleKey.LeftArrow:
                case ConsoleKey.A:
                    onMovementKeyPressed?.Invoke(direction: new Vector2(-1,0));
                    return;
                case ConsoleKey.RightArrow:
                case ConsoleKey.D:
                    onMovementKeyPressed?.Invoke(direction: new Vector2(1,0));
                    return;
                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
                    onRotationKeyPressed?.Invoke(direction: new Vector2(0,-1)); //(0,0 is the top corner of the screen)
                    return;
                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    onMovementKeyPressed?.Invoke(direction: new Vector2(0,1));
                    return;
            }
            input = new ConsoleKeyInfo();
        }
    }
}