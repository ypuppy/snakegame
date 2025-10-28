using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;


namespace SnakeGame
{
    class Game
    {
        private int width, height;
        private Snake snake;
        private Food food;
        private List<Enemy> enemies = new List<Enemy>();
        private bool gameOver = false;
        private int frameDelay;
        private int score = 0;
        private int nextEnemyScore = 50;
        private int highScore = 0;
        private string highScoreFile = "highscore.txt";
        private bool isNewRecord = false;


        public Game(int width, int height, int initialSpeed = 200)
        {
            this.width = width;
            this.height = height;
            this.frameDelay = initialSpeed;
            snake = new Snake(width / 2, height / 2);
            food = new Food(width, height);
        }

        public void Run()
        {
            while (!gameOver)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;
                    snake.ChangeDirection(key);
                }

                snake.Move();

                // 吃到食物
                if (snake.Head.X == food.Position.X && snake.Head.Y == food.Position.Y)
                {
                    HandleFoodEffect();
                    snake.Grow();
                    food.Respawn(width, height);
                }

                // 每 50 分生成一个敌人
                if (score >= nextEnemyScore)
                {
                    enemies.Add(new Enemy(width, height));
                    nextEnemyScore += 50; // 下一个敌人要再多 50 分后才生成
                }


                // 更新敌人
                foreach (var e in enemies)
                {
                    e.Move();
                }

                // 撞墙或自己
                if (snake.Head.X <= 0 || snake.Head.X >= width - 1 ||
                    snake.Head.Y <= 1 || snake.Head.Y >= height - 1 ||
                    snake.CheckSelfCollision())
                {
                    gameOver = true;
                }

                // 撞到敌人
                foreach (var e in enemies)
                {
                    if (snake.Head.X == e.Position.X && snake.Head.Y == e.Position.Y)
                    {
                        gameOver = true;
                        break;
                    }
                }

                // 绘制画面
                Console.Clear();
                DrawBorder();
                DrawHUD();
                snake.Draw();
                food.Draw();
                foreach (var e in enemies) e.Draw();
                Thread.Sleep(frameDelay);
            }

            GameOverAnimation();
        }

        private void HandleFoodEffect()
        {
            switch (food.Type)
            {
                case FoodType.Normal:
                    score += 10;
                    frameDelay = Math.Max(100, frameDelay - 5);
                    break;

                case FoodType.Attack:

                    if (enemies.Count > 0)
                    {
                        Random rand = new Random();
                        int index = rand.Next(enemies.Count);
                        enemies.RemoveAt(index);
                    }
                    break;

                case FoodType.Freeze:
                    foreach (var e in enemies)
                        e.Freeze(2000); // 冻结两秒
                    break;
            }
        }

        private void GameOverAnimation()
        {
            bool isNewRecord = false;

            if (score > highScore)
            {
                highScore = score;
                File.WriteAllText(highScoreFile, highScore.ToString());
                isNewRecord = true;
            }


            for (int i = 0; i < 5; i++)
            {
                Console.Clear();
                if (i % 2 == 0)
                {
                    DrawBorder();
                    DrawHUD();
                    snake.Draw();
                    foreach (var e in enemies) e.Draw();
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

            // 🎉 Flash "NEW HIGH SCORE!" if applicable
            if (isNewRecord)
            {
                for (int i = 0; i < 6; i++)
                {
                    Console.SetCursorPosition(width / 2 - 8, height / 2 + 4);
                    Console.ForegroundColor = (i % 2 == 0) ? ConsoleColor.Yellow : ConsoleColor.Magenta;
                    Console.Write("✨ NEW HIGH SCORE! ✨");
                    Thread.Sleep(250);
                }
                Console.ResetColor();
            }
            else
            {
                Console.SetCursorPosition(width / 2 - 8, height / 2 + 4);
                Console.Write("Press any key to exit...");
            }

            Console.ReadKey(true);
        }

        private void DrawBorder()
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

            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(20, 0);
            Console.Write($"High: {highScore}");
            Console.ResetColor();
        }

    }
}
