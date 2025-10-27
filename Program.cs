using System;

namespace SnakeGame
{
    class Program
    {
        static void Main()
        {
            Console.CursorVisible = false;
            Game game = new Game(30, 20);
            game.Run();
        }
    }
}
