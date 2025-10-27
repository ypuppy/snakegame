using System;
using System.Collections.Generic;

namespace SnakeGame
{
    class Snake
    {
        public LinkedList<Point> Body { get; private set; }
        public Point Head => Body.First.Value;
        private int dx = 1, dy = 0; // 初始方向：向右

        public Snake(int startX, int startY)
        {
            Body = new LinkedList<Point>();
            Body.AddFirst(new Point(startX, startY));
        }

        public void ChangeDirection(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow: if (dy == 0) { dx = 0; dy = -1; } break;
                case ConsoleKey.DownArrow: if (dy == 0) { dx = 0; dy = 1; } break;
                case ConsoleKey.LeftArrow: if (dx == 0) { dx = -1; dy = 0; } break;
                case ConsoleKey.RightArrow: if (dx == 0) { dx = 1; dy = 0; } break;
            }
        }

        public void Move()
        {
            Point newHead = new Point(Head.X + dx, Head.Y + dy);
            Body.AddFirst(newHead);
            Body.RemoveLast();
        }

        public void Grow()
        {
            Point newHead = new Point(Head.X + dx, Head.Y + dy);
            Body.AddFirst(newHead);
        }

        public bool CheckSelfCollision()
        {
            foreach (var part in Body)
            {
                if (!part.Equals(Head) && part.X == Head.X && part.Y == Head.Y)
                    return true;
            }
            return false;
        }

        public void Draw()
        {
            foreach (var part in Body)
            {
                Console.SetCursorPosition(part.X, part.Y);
                Console.Write("O");
            }
        }
    }
}
