// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static void Main()
    {
        Console.CursorVisible = false;
        Game game = new Game(30, 20);
        game.Run();
    }
}

class Game
{
    private int width, height;
    private Snake snake;
    private Food food;
    private bool gameOver = false;
    private int frameDelay = 200; // 初始帧延迟
    private int score = 0;

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

            // 绘制
            Console.Clear();
            DrawBorder();
            DrawHUD();
            snake.Draw();
            food.Draw();
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

struct Point
{
    public int X;
    public int Y;
    public Point(int x, int y)
    {
        X = x; Y = y;
    }
}
