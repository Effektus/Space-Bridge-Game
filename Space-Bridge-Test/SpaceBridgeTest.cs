using System;
using System.Collections.Generic;
using System.Threading;

namespace Space_Bridge_Test
{
    public class SpaceBridge
    {
        public static void Main()
        {
            int Points = 0;
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
            Object userObject = new Object(x, y, symbol, color);
            Object newObject = new Object(9, 0, "$", ConsoleColor.Green);
            bool bridgeHitted = false;
            while (true)
            {
                //4.Move the bridge
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
                //Moving our alien....
                if (!bridgeHitted)
                {
                    if (newObject.Y < Console.WindowHeight)
                    {
                        newObject.Y++;
                    }
                    if (newObject.X < Console.WindowWidth)
                    {
                        if (newObject.X == 15)
                        {
                            newObject.X--;
                        }
                        newObject.X++;
                    }
                    if (newObject.Y == userObject.Y && newObject.X == userObject.X + 1)
                    {
                        Points++;
                        bridgeHitted = true;
                    }
                }
                if (bridgeHitted)
                {
                    if (newObject.Y > Console.WindowHeight - 7)
                    {
                        newObject.Y--;
                    }
                    else
                    {
                        bridgeHitted = false;
                    }
                    if (newObject.X < Console.WindowWidth)
                    {
                        newObject.X++;
                    }
                }
                //remove objects which are outside the field and create new
                if (newObject.X > 42)
                {
                    newObject = new Object(9, 0, "$", ConsoleColor.Green);
                }
                //7.Clear the console with 
                Console.Clear();
                //Print other object
                PrintOnPosition(newObject.X, newObject.Y, newObject.Symbol, newObject.Color);
                //8.print our object
                PrintOnPosition(userObject.X, userObject.Y, userObject.Symbol, userObject.Color);
                //printing points
                PrintOnPosition(41, 0, "P:" + Points.ToString(), ConsoleColor.DarkRed);
                //Print board for the field
                PrintTheSideBoard(8);
                PrintTheSideBoard(40);

                //9.Draw info
                //10.Slow down program
                Thread.Sleep(500);
            }
        }

        private static void PrintTheSideBoard(int x)
        {
            for (int i = 0; i < Console.WindowHeight; i++)
            {
                PrintOnPosition(x, i, "|", ConsoleColor.White);
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
}