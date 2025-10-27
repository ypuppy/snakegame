using System;
using System.Threading;

namespace SnakeGame
{
    class Game
    {
        private int width, height;
        private Snake snake;
        private Food food;
        private bool gameOver = false;
        private int frameDelay = 200; // 初始帧延迟
        private int score = 0;
        private Enemy enemy;
        private bool enemySpawned = false;


        public Game(int width, int height)
        {
            this.width = width;
            this.height = height;
            snake = new Snake(width / 2, height / 2);
            food = new Food(width, height);
        }

        public void Run()
        {
            while (!gameOver)
            {
                // 输入
                if (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;
                    snake.ChangeDirection(key);
                }

                // 更新
                snake.Move();

                // 分数 >= 100 时生成敌人
                if (!enemySpawned && score >= 10)
                {
                    enemy = new Enemy(width, height);
                    enemySpawned = true;
                }


                // 吃到食物
                if (snake.Head.X == food.Position.X && snake.Head.Y == food.Position.Y)
                {
                    snake.Grow();
                    food.Respawn(width, height);
                    score += 10; // 🍏 加分
                    frameDelay = Math.Max(100, frameDelay - 5);
                }

                // 撞墙或自己
                if (snake.Head.X <= 0 || snake.Head.X >= width - 1 ||
                    snake.Head.Y <= 1 || snake.Head.Y >= height - 1 ||
                    snake.CheckSelfCollision())
                {
                    gameOver = true;
                }
                // 如果蛇头碰到敌人 → Game Over
                if (enemySpawned && snake.Head.X == enemy.Position.X && snake.Head.Y == enemy.Position.Y)
                {
                    gameOver = true;
                }


                // 绘制
                Console.Clear();
                DrawBorder();
                DrawHUD();
                snake.Draw();
                food.Draw();
                if (enemySpawned)
                {
                    enemy.Move();
                    enemy.Draw();
                }

                Thread.Sleep(frameDelay);
            }

            // ⚠️ Game Over 动画：闪烁
            for (int i = 0; i < 5; i++)
            {
                Console.Clear();
                if (i % 2 == 0)
                {
                    DrawBorder();
                    DrawHUD();
                    snake.Draw();
                }
                Thread.Sleep(200);
            }

            Console.Clear();
            Console.SetCursorPosition(width / 2 - 4, height / 2);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("GAME OVER!");
            Console.ResetColor();

            Console.SetCursorPosition(width / 2 - 6, height / 2 + 2);
            Console.Write($"Final Score: {score}");
            Console.SetCursorPosition(width / 2 - 8, height / 2 + 4);
            Console.Write("Press any key to exit...");
            Console.ReadKey(true);
        }

        void DrawBorder()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            for (int x = 0; x < width; x++)
            {
                Console.SetCursorPosition(x, 1);
                Console.Write("-");
                Console.SetCursorPosition(x, height - 1);
                Console.Write("-");
            }

            for (int y = 1; y < height; y++)
            {
                Console.SetCursorPosition(0, y);
                Console.Write("|");
                Console.SetCursorPosition(width - 1, y);
                Console.Write("|");
            }
            Console.ResetColor();
        }

        void DrawHUD()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(2, 0);
            Console.Write($"Score: {score}");
            Console.ResetColor();
        }
    }
}
