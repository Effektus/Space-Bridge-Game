using System;
using System.Collections.Generic;
using System.Threading;

public class SpaceBridgeTest
{
    //1.Structure "Object" contains four variables - coordinates, color, and its symbols.
    class userObject
    {
        public int x;
        public int y;
        public ConsoleColor color;
        public string symbol;
        public userObject(int x, int y, string symbol, ConsoleColor color = ConsoleColor.Green)
        {
            this.x = x;
            this.y = y;
            this.symbol = symbol;
            this.color = color;
        }
    }
    class Object
    {
        public int x;
        public int y;
        public ConsoleColor color;
        public string symbol;
        public Object(int x, int y, string symbol, ConsoleColor color = ConsoleColor.Green)
        {
            this.x = x;
            this.y = y;
            this.symbol = symbol;
            this.color = color;
        }
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
        userObject userObject = new userObject(x, y, symbol, color);

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
                    if (userObject.x > 15)
                    {
                        userObject.x -= 9;
                    }
                }
                else if (pressedKey.Key == ConsoleKey.RightArrow)
                {
                    if (userObject.x + 1 < playfieldWidth - 7)
                    {
                        userObject.x += 9;
                    }
                }
            }
            //Struct was converted to Class , so now we can change (y) easily!

            if (objects[0].y < Console.WindowHeight)
            {
                objects[0].y = objects[0].y + 1;
            }
            if (objects[0].x < Console.WindowWidth)
            {
                if (objects[0].x == 15)
                {
                    objects[0].x--;
                }
                objects[0].x++;
            }
            if (objects.Count > 0)
            {
                if (objects[0].y == userObject.y && objects[0].x == userObject.x + 1)
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
                PrintOnPosition(element.x, element.y, element.symbol, element.color);
            }
            //8.print our object
            PrintOnPosition(userObject.x, userObject.y, userObject.symbol, userObject.color);
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
                if (objects[0].y == Console.WindowHeight - 1)
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