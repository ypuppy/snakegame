using System;

namespace SnakeGame
{
    class Enemy
    {
        public Point Position { get; private set; }
        private Random rand = new Random();
        private int width, height;
        private int moveCounter = 0;

        public Enemy(int width, int height)
        {
            this.width = width;
            this.height = height;
            Respawn();
        }

        public void Respawn()
        {
            Position = new Point(rand.Next(2, width - 2), rand.Next(2, height - 2));
        }

        public void Move()
        {
            // 每 5 帧移动一次
            moveCounter++;
            if (moveCounter % 5 != 0) return;

            int dir = rand.Next(4);
            int newX = Position.X;
            int newY = Position.Y;

            switch (dir)
            {
                case 0: newX++; break; // 右
                case 1: newX--; break; // 左
                case 2: newY++; break; // 下
                case 3: newY--; break; // 上
            }

            // 保持在边界内
            if (newX > 1 && newX < width - 1 && newY > 1 && newY < height - 1)
            {
                Position = new Point(newX, newY);
            }
        }

        public void Draw()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(Position.X, Position.Y);
            Console.Write("X");
            Console.ResetColor();
        }
    }
}
