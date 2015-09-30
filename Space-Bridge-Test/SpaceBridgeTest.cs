using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;

public class SpaceBridgeTest
{
    public class UserObject
    {
        private int x;
        private int y;
        private ConsoleColor color;
        private string symbol;

        public UserObject(int x, int y, string symbol, ConsoleColor color = ConsoleColor.Green)
        {
            this.X = x;
            this.Y = y;
            this.Symbol = symbol;
            this.Color = color;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public string Symbol { get; set; }
        public ConsoleColor Color { get; set; }
    }
    public class Object
    {
        private int x;
        private int y;
        private ConsoleColor color;
        private string symbol;

        public Object(int x, int y, string symbol, ConsoleColor color = ConsoleColor.Green)
        {

            this.X = x;
            this.Y = y;
            this.Symbol = symbol;
            this.Color = color;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public string Symbol { get; set; }
        public ConsoleColor Color { get; set; }
    }

    public static void Main()
    {
        int points = 0;
        //2.Draw playfield
        int playfieldWidth = 35;
        Console.BufferHeight = Console.WindowHeight = 10;//This is the size of the console window height.
        Console.BufferWidth = Console.WindowWidth = 49;//size of window width.                              
        Console.BackgroundColor = ConsoleColor.Black;//color of playfield

        //3.Make object
        int x = 23;
        int y = Console.WindowHeight - 3;
        string symbol = "___";
        ConsoleColor color = ConsoleColor.DarkRed;
        UserObject userObject = new UserObject(x, y, symbol, color);

        List<Object> objects = new List<Object>();
        Random randomGenerator = new Random();
        while (true)
        {
            bool hit = false;

            int xx = 9;
            Object newObject = new Object(xx, 0, "$", ConsoleColor.Green);
            objects.Add(newObject);

            //4.Move our object
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                }
                if (pressedKey.Key == ConsoleKey.LeftArrow)
                {
                    if (userObject.X > 15)
                    {
                        userObject.X -= 9;
                    }
                }
                else if (pressedKey.Key == ConsoleKey.RightArrow)
                {
                    if (userObject.X + 1 < playfieldWidth - 7)
                    {
                        userObject.X += 9;
                    }
                }
            }

            if (objects[0].Y < Console.WindowHeight)
            {
                objects[0].Y = objects[0].Y + 1;
            }
            if (objects[0].X < Console.WindowWidth)
            {
                if (objects[0].X == 15)
                {
                    objects[0].X--;
                }
                objects[0].X++;
            }
            if (objects.Count > 0)
            {
                if (objects[0].Y == userObject.Y && objects[0].X == userObject.X + 1)
                {
                    points++;
                }
            }
            // Check for HIT!!!

            //7.Clear the console with 
            Console.Clear();
            //Print other object
            foreach (Object element in objects)
            {
                PrintOnPosition(element.X, element.Y, element.Symbol, element.Color);
            }
            //8.print our object
            PrintOnPosition(userObject.X, userObject.Y, userObject.Symbol, userObject.Color);
            //printing userObject lives

            PrintOnPosition(41, 0, "P:" + points.ToString(), ConsoleColor.DarkRed);
            //Print board for the field
            for (int i = 0; i < Console.WindowHeight; i++)
            {
                PrintOnPosition(8, i, "|", ConsoleColor.White);
            }
            PrintTheSideBoard();
            //remove objects which are outside the field
            if (objects.Count > 0)
            {
                if (objects[0].Y == Console.WindowHeight - 1)
                {
                    objects.RemoveAt(0);
                }
            }
            //9.Draw info
            //10.Slow down program
            Thread.Sleep(300);
        }
    }

    private static void PrintTheSideBoard()
    {
        for (int i = 0; i < Console.WindowHeight; i++)
        {
            PrintOnPosition(40, i, "|", ConsoleColor.White);
        }
    }

    //Method which print object 
    static void PrintOnPosition(int x, int y, string symbol, ConsoleColor color = ConsoleColor.Green)
    {
        //Console.SetCursorPosition move our cursor in place of what we write.
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = color;
        Console.Write(symbol);
    }
}