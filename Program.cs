using System;
using System.IO;
using System.Threading;

namespace SnakeGame
{
    class Program
    {
        static void Main()
        {
            Console.CursorVisible = false;
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                ShowMenu();
                ConsoleKey key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        StartGame();
                        break;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        ViewHighScore();
                        break;

                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                    case ConsoleKey.Escape:
                        exit = true;
                        break;
                }
            }

            Console.Clear();
            Console.WriteLine("Thanks for playing!");
            Thread.Sleep(1000);
        }

        static void ShowMenu()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=====================================");
            Console.WriteLine("          🐍  SNAKE GAME  🐍");
            Console.WriteLine("=====================================");
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine("  [1] Start Game");
            Console.WriteLine("  [2] View High Score");
            Console.WriteLine("  [3] Quit");
            Console.WriteLine();
            Console.Write("Select an option: ");
        }

        static void StartGame()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Select Difficulty:");
            Console.ResetColor();
            Console.WriteLine("  [1] Easy");
            Console.WriteLine("  [2] Normal");
            Console.WriteLine("  [3] Hard");
            Console.WriteLine();
            Console.Write(": ");

            int width = 30;
            int height = 20;
            int speed = 200;  // frame delay in ms
            Difficulty diff = Difficulty.Normal;

            ConsoleKey key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    width = 45; height = 35; speed = 250;
                    diff = Difficulty.Easy;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Easy Mode Selected, welcome to the world");
                    break;

                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    width = 35; height = 25; speed = 200;
                    diff = Difficulty.Normal;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Normal Mode Selected,welcome to the world");
                    break;

                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    width = 30; height = 20; speed = 150;
                    diff = Difficulty.Hard;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Hard Mode Selected,welcome to the world");
                    break;

                default:
                    Console.WriteLine("Invalid input, defaulting to Normal Mode.");
                    break;
            }

            Console.ResetColor();
            Thread.Sleep(800);

            Console.Clear();
            Game game = new Game(width, height, speed, diff);
            game.Run();
        }

        static void ViewHighScore()
        {
            Console.Clear();
            string highScoreFile = "highscore.txt";
            int highScore = 0;

            if (File.Exists(highScoreFile))
            {
                string content = File.ReadAllText(highScoreFile);
                int.TryParse(content, out highScore);
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("=== HIGH SCORE ===");
            Console.ResetColor();
            Console.WriteLine($"Highest Score: {highScore}");
            Console.WriteLine();
            Console.WriteLine("Press any key to return to menu...");
            Console.ReadKey(true);
        }
    }
}
