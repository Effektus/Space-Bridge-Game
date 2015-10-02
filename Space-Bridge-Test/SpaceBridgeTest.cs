using System;
using System.Collections.Generic;
using System.Threading;

namespace Space_Bridge_Test
{
    public class SpaceBridge
    {
        public static void Main()
        {
            Console.Title = "Space Bridge (Dolars) by Team Sulorine";
         
            const int playfieldWidth = 35;
            const int xCoordinate = 23;
            const double acceleration = 0.04;

            Console.BufferHeight = Console.WindowHeight = 10;//This is the size of the console window height.
            Console.BufferWidth = Console.WindowWidth = 49;//size of window width.                              
            Console.BackgroundColor = ConsoleColor.Black;//color of playfield

            
            int yCoordinate = Console.WindowHeight - 3;
            string symbolForBridge = "___";
            ConsoleColor colorOfBridge = ConsoleColor.White;
            Object bridge = new Object(xCoordinate, yCoordinate, symbolForBridge, colorOfBridge, false);
                       
            int lives = 3;
            int points = 0;
            double speed = 10.0;
            bool bridgeHitted = false;
            Random rnd = new Random();

            PrintOnPosition(20, 5, "START GAME", ConsoleColor.Red);
            PlayMusicStart();
            Object cosmonaut = new Object(9, 0, "$", ConsoleColor.Green, false);
            List<Object> cosmonauts = new List<Object>();
            cosmonauts.Add(cosmonaut);
            while (true)
            {
                SpeedControl(speed, acceleration);

                int chance = rnd.Next(0, 70);

                AddMoreCosmonaut(points, cosmonauts, chance);
              
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
                            Console.Beep(678, 200);
                            cosmonauts[i].BridgeHitted = true;

                        }
                        if (cosmonauts[i].Y > Console.WindowHeight - 1)
                        {
                            lives--;
                            Console.Beep(327, 500); Console.Beep(213, 500);
                            speed += 5;
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
   
                CreateNewCosmonaut(cosmonauts);
      
                Console.Clear();

                //8. Print Basket
                PrintOnPosition(41, 7, "\\", ConsoleColor.White);
                PrintOnPosition(41, 8, "|", ConsoleColor.White);
                PrintOnPosition(45, 8, "|", ConsoleColor.White);
                PrintOnPosition(42, 8, "___", ConsoleColor.White);
                PrintOnPosition(45, 7, "/", ConsoleColor.White);

                //Print Lives
                PrintOnPosition(38, 1, "Lives:" + lives.ToString(), ConsoleColor.Red);

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
                PrintOnPosition(38, 0, "Dolars:" + points, ConsoleColor.Green);
                
            }

            PrintOnPosition(15, 4, "GAME OVER!!!", ConsoleColor.DarkRed);
            if (points < 10)
            {
                PrintOnPosition(10, 5, $"Oooh poor litle baby! Just {points} dolars", ConsoleColor.DarkRed);
                PlayMusicEnd();
            }
            else if (points > 10 && points < 20)
            {
                PrintOnPosition(10, 5, $"You have {points} dolars.", ConsoleColor.DarkRed);
            }
            else if (points > 20)
            {
                PrintOnPosition(10, 5, $"You are rich!!! {points} dolars.", ConsoleColor.DarkRed);
            }
            Console.WriteLine();
        }

        private static void CreateNewCosmonaut(List<Object> cosmonauts)
        {
            for (int i = 0; i < cosmonauts.Count; i++)
            {
                if (cosmonauts[i].X > 42)
                {
                    cosmonauts.Remove(cosmonauts[i]);
                    cosmonauts.Add(new Object(9, 0, "$", ConsoleColor.Green, false));
                }
            }
        }

        private static void AddMoreCosmonaut(int points, List<Object> cosmonauts, int chance)
        {
            if (points > 2 && cosmonauts.Count < 3)
            {
                //check if it is possible to be played
                if (cosmonauts.TrueForAll(x => x.X < chance))
                {
                    Object cos = new Object(9, 0, "$", ConsoleColor.Green, false);
                    cosmonauts.Add(cos);
                }
            }
        }

        /// <summary>
        /// Use Thread.Sleep and double speed for control
        /// </summary>
        /// <param name="speed">speed</param>
        /// <param name="acceleration">acceleratino</param>
        private static void SpeedControl(double speed, double acceleration)
        {
            speed += acceleration;
            if (speed > 400)
            {
                speed = 400;
            }
            Thread.Sleep(500 - (int) speed);
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

        /// <summary>        
        /// Sound
        /// </summary>
        public static void PlayMusicEnd()
        {
            Console.Beep(528, 500); Console.Beep(440, 500); Console.Beep(419, 500);
            Console.Beep(495, 500); Console.Beep(660, 500); Console.Beep(528, 500);
            Console.Beep(594, 500);
        }
        public static void PlayMusicStart()
        {
            Console.Beep(659, 200); Console.Beep(659, 200); Thread.Sleep(167);
            Console.Beep(659, 200); Thread.Sleep(167); Console.Beep(523, 200);
            Console.Beep(659, 200); Thread.Sleep(200); Console.Beep(784, 200);
        }
    }
}