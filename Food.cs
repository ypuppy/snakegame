using System;

namespace SnakeGame
{
    enum FoodType { Normal, Attack, Freeze }

    class Food
    {
        public Point Position { get; private set; }
        public FoodType Type { get; private set; }
        private Random rand = new Random();
        private ConsoleColor color;

        public Food(int width, int height)
        {
            Respawn(width, height);
        }

        public void Respawn(int width, int height, int specialChance = 10)
        {
            Position = new Point(rand.Next(2, width - 2), rand.Next(2, height - 2));

            // use difficulty-based probability
            int r = rand.Next(100);
            if (r < specialChance / 2)
                Type = FoodType.Attack;
            else if (r < specialChance)
                Type = FoodType.Freeze;
            else
                Type = FoodType.Normal;

            color = Type switch
            {
                FoodType.Attack => ConsoleColor.Magenta,
                FoodType.Freeze => ConsoleColor.Cyan,
                _ => (ConsoleColor)rand.Next(1, 15)
            };
        }


        public void Draw()
        {
            Console.ForegroundColor = color;

            string symbol = Type switch
            {
                FoodType.Attack => "⚔",
                FoodType.Freeze => "❄",
                _ => "@"
            };

            Console.SetCursorPosition(Position.X, Position.Y);
            Console.Write(symbol);
            Console.ResetColor();
        }
    }
}
