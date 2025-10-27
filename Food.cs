using System;

namespace SnakeGame
{
    class Food
    {
        public Point Position { get; private set; }
        private Random rand = new Random();
        private ConsoleColor color;

        public Food(int width, int height)
        {
            Respawn(width, height);
        }

        public void Respawn(int width, int height)
        {
            Position = new Point(rand.Next(2, width - 2), rand.Next(2, height - 2));
            color = (ConsoleColor)rand.Next(1, 15); // 随机颜色
        }

        public void Draw()
        {
            Console.ForegroundColor = color;

            // 让食物颜色轻微闪烁
            if (rand.Next(0, 5) == 0)
            {
                color = (ConsoleColor)rand.Next(1, 15);
            }

            Console.SetCursorPosition(Position.X, Position.Y);
            Console.Write("@");
            Console.ResetColor();
        }
    }
}
