using System;
using System.Collections.Generic;
using System.Threading;

namespace Space_Bridge_Test
{
    public class SpaceBridge
    {
        public static void Main()
        {
            //2.Draw playfield
            const int playfieldWidth = 35;
            Console.BufferHeight = Console.WindowHeight = 10;//This is the size of the console window height.
            Console.BufferWidth = Console.WindowWidth = 49;//size of window width.                              
            Console.BackgroundColor = ConsoleColor.Gray;//color of playfield

            int xCoordinate = 23;
            int yCoordinate = Console.WindowHeight - 3;
            string symbolForBridge = "___";
            ConsoleColor colorOfBridge = ConsoleColor.Black;
            Object bridge = new Object(xCoordinate, yCoordinate, symbolForBridge, colorOfBridge, false);
            bool bridgeHitted = false;
            Object cosmonaut = new Object(9, 0, "$", ConsoleColor.DarkGreen, bridgeHitted);
            int lives = 3;
            int points = 0;
            double speed = 10.0;
            double acceleration = 0.01;
            List<Object> cosmonauts = new List<Object>();
            cosmonauts.Add(cosmonaut);
            Random rnd = new Random();
            while (true)
            {
                speed += acceleration;
                if (speed > 400)
                {
                    speed = 400;
                }
                Thread.Sleep(500 - (int)speed);
                int chance = rnd.Next(0, 50);
                //Add more cosmonauts
                if (points > 2 && chance < 70 && cosmonauts.Count < 3)
                {
                    //check if it is possible to be played
                    if (cosmonauts.TrueForAll(x => x.X < chance))
                    {
                        Object cos = new Object(9, 0, "$", ConsoleColor.Green, false);
                        cosmonauts.Add(cos);
                    }
                }
                //4.Move the bridge
                MoveTheBridge(bridge, playfieldWidth);
                //Moving our alien....
                for (int i = 0; i < cosmonauts.Count; i++)
                {
                    bridgeHitted = cosmonauts[i].BridgeHitted;
                    if (!bridgeHitted)
                    {
                        if (cosmonauts[i].Y < Console.WindowHeight)
                        {
                            cosmonauts[i].Y++;
                        }
                        if (cosmonauts[i].X < Console.WindowWidth)
                        {
                            if (cosmonauts[i].X == 15)
                            {
                                cosmonauts[i].X--;
                            }
                            cosmonauts[i].X++;
                        }
                        if (cosmonauts[i].Y == bridge.Y && cosmonauts[i].X == bridge.X + 1)
                        {
                            points++;
                            Console.Beep();
                            cosmonauts[i].BridgeHitted = true;

                        }
                        if (cosmonauts[i].Y > Console.WindowHeight - 1)
                        {
                            lives--;
                            cosmonauts.Remove(cosmonauts[i]);
                            if (cosmonauts.Count == 0)
                            {
                                cosmonauts.Add(new Object(9, 0, "$", ConsoleColor.Green, bridgeHitted));
                            }
                            if (lives == 0)
                            {
                                break;
                            }
                        }
                    }
                    if (lives == 0)
                    {
                        break;
                    }
                }
                if (lives == 0)
                {
                    break;
                }
                //Moving the opposite direction after bridge was hitted
                for (int i = 0; i < cosmonauts.Count; i++)
                {
                    bridgeHitted = cosmonauts[i].BridgeHitted;
                    if (bridgeHitted)
                    {
                        if (cosmonauts[i].Y > Console.WindowHeight - 7)
                        {
                            cosmonauts[i].Y--;
                        }
                        else
                        {
                            cosmonauts[i].BridgeHitted = false;
                        }
                        if (cosmonauts[i].X < Console.WindowWidth)
                        {
                            cosmonauts[i].X++;
                        }
                    }
                }
                //remove objects which are outside the field and create new      
                for (int i = 0; i < cosmonauts.Count; i++)
                {
                    if (cosmonauts[i].X > 42)
                    {
                        cosmonauts.Remove(cosmonauts[i]);
                        cosmonauts.Add(new Object(9, 0, "$", ConsoleColor.Green, false));
                    }
                }
                //7.Clear the console with 
                Console.Clear();
                //8. Print Basket
                PrintOnPosition(41, 7, "\\", ConsoleColor.White);
                PrintOnPosition(42, 7, "___", ConsoleColor.White);
                PrintOnPosition(45, 7, "/", ConsoleColor.White);
                //Print Lives
                PrintOnPosition(39, 1, "Lives:" + lives.ToString(), ConsoleColor.DarkRed);
                //Print other object  
                foreach (var cosmo in cosmonauts)
                {
                    if (cosmo.X < 48 && cosmo.Y < 10)
                    {
                        PrintOnPosition(cosmo.X, cosmo.Y, cosmo.Symbol, cosmo.Color);
                    }
                }
                //8.print our object
                PrintOnPosition(bridge.X, bridge.Y, bridge.Symbol, bridge.Color);

                //printing points
                PrintOnPosition(39, 0, "Dolars:" + points, ConsoleColor.DarkRed);
                
            }
            PrintOnPosition(8, 5, "Game Over!!!", ConsoleColor.DarkRed);
        }

        /// <summary>
        /// Move the bridge with keys.
        /// </summary>
        /// <param name="userObject">bridge object</param>
        /// <param name="playfieldWidth">play field</param>
        private static void MoveTheBridge(Object userObject, int playfieldWidth)
        {
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                
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
        }

        /// <summary>
        /// Print the side board
        /// </summary>
        /// <param name="x">x coordinate</param>
        private static void PrintTheSideBoard(int x)
        {
            for (int i = 0; i < Console.WindowHeight; i++)
            {
                PrintOnPosition(x, i, "|", ConsoleColor.White);
            }
        }

        /// <summary>
        /// Print on position.Console.SetCursorPosition move our cursor in place of what we write.
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <param name="symbol">our symbol</param>
        /// <param name="color">color Of symbol</param>
        private static void PrintOnPosition(int x, int y, string symbol, ConsoleColor color = ConsoleColor.Green)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(symbol);
        }
    }
}