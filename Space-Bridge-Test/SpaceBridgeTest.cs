using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Linq;

namespace Space_Bridge_Test
{
    public class SpaceBridge
    {
        public static int points = 0;
        static double speed = 40.0;
        public static void Main()
        {
            Console.Title = "$CATCH THE MONEY$ by Team Sulorine";

            const int playfieldWidth = 35;
            const int xCoordinate = 23;
            const double acceleration = 2;

            Console.BufferHeight = Console.WindowHeight = 10;//This is the size of the console window height.
            Console.BufferWidth = Console.WindowWidth = 49;//size of window width.                              
            Console.BackgroundColor = ConsoleColor.Black;//color of playfield
            PrintStartText();
            PrintOnPosition(19, 6, "G A M E", ConsoleColor.Red);
            PlayMusicStart();
            Console.Clear();

            int x = 10;
            int y = 5;
            Console.SetCursorPosition(x, y);
            Console.Write("Please enter your nickname: ");
            string playerName = Console.ReadLine();
            Console.Clear();

            int yCoordinate = Console.WindowHeight - 3;
            string symbolForBridge = "___";
            ConsoleColor colorOfBridge = ConsoleColor.White;
            Object bridge = new Object(xCoordinate, yCoordinate, symbolForBridge, colorOfBridge, false);
           
            
            int lives = 3;         
           
            bool bridgeHitted = false;
            Random randomnd = new Random();
     
            Object dollar = new Object(9, 0, "$", ConsoleColor.Green, false);
            List<Object> wallet = new List<Object>();
            wallet.Add(dollar);
            PrintBasket();
            while (true)
            {
                SpeedControl(speed, acceleration);
                int chance = randomnd.Next(0, 70);
                AddMoreDollars(points, wallet, chance);
                MoveTheBridge(bridge, playfieldWidth);
                PrintNewBridgeState(bridge);
                //Moving our alien....
                for (int i = 0; i < wallet.Count; i++)
                {
                    bridgeHitted = wallet[i].BridgeHitted;
                    if (!bridgeHitted)
                    {
                        if (wallet[i].Y < Console.WindowHeight)
                        {
                            wallet[i].Y++;
                        }
                        if (wallet[i].X < Console.WindowWidth)
                        {
                            if (wallet[i].X == 15)
                            {
                                wallet[i].X--;
                            }
                            wallet[i].X++;
                        }
                        if (wallet[i].Y == bridge.Y && wallet[i].X == bridge.X + 1)
                        {
                            Console.Beep(678, 200);
                            wallet[i].BridgeHitted = true;
                        }
                        if (wallet[i].Y > Console.WindowHeight - 1)
                        {
                            lives--;
                            Console.Beep(327, 500); Console.Beep(213, 500);
                            speed += 5;
                            wallet.Remove(wallet[i]);
                            if (wallet.Count == 0)
                            {
                                wallet.Add(new Object(9, 0, "$", ConsoleColor.Green, bridgeHitted));
                            }
                            if (lives == 0)
                            {
                                break;
                            }
                        }
                        try
                        {
                            PrintNewLowerState(wallet[i]);
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            continue;
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
                for (int i = 0; i < wallet.Count; i++)
                {
                    bridgeHitted = wallet[i].BridgeHitted;
                    if (bridgeHitted)
                    {
                        if (wallet[i].Y > Console.WindowHeight - 7)
                        {
                            wallet[i].Y--;
                        }
                        else
                        {
                            wallet[i].BridgeHitted = false;
                        }
                        if (wallet[i].X < Console.WindowWidth)
                        {
                            wallet[i].X++;
                        }
                        PrintNewUpperState(wallet[i]);
                    }
                }
                PrintOnPosition(38, 1, "Lives:" + lives, ConsoleColor.Red);
                PrintOnPosition(38, 0, "Dollars:" + points);
                CreateNewObject(wallet);
            }

            FinalMessages();
            Console.Clear();

            List<string> plScores = PlayersScores(playerName);                    
            PrintScore(plScores);

        }

        /// <summary>
        /// Print start text.
        /// </summary>
        private static void PrintStartText()
        {
            string[] startText = new string[7];
            startText[0] = "                                       ";
            startText[1] = "  $$$$$  $$$$$    $      $$$$   $$$$$  ";
            startText[2] = "  $   $    $     $ $     $__$     $    ";
            startText[3] = "   $       $    $   $    $$       $    ";
            startText[4] = "    $      $   $     $   $  $     $    ";
            startText[5] = "  ___ $    $  $       $  $   $    $    ";
            startText[6] = "                                       ";

            int x = 4;
            int y = 2;

            foreach (string line in startText)
            {
                Console.SetCursorPosition(x, y);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(line);
                Console.Beep(418, 130);
                y++;
            }
        }

        /// <summary>
        /// Print score.
        /// </summary>
        /// <param name="plScore">list of score</param>
        private static void PrintScore(List<string> plScore)
        {
            int count = 1;
            Console.BufferHeight = Console.WindowHeight = 13;
            Console.BufferWidth = Console.WindowWidth = 49;
            string highScore = "HIGH SCORE";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"      {highScore}\n");

            Dictionary<string, int> listOfPlayers = plScore
                .Select(l => l.Split('-'))
                .ToDictionary(a => a[0], a => int.Parse(a[1]));
            
            var listOfOrders = listOfPlayers
                .OrderByDescending(p => p.Value);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("RANK    SCORE    NAME\n");
            List<ConsoleColor> colors = new List<ConsoleColor>()
            {
                ConsoleColor.DarkCyan,
                ConsoleColor.Blue,
                ConsoleColor.DarkRed,
                ConsoleColor.Gray,
                ConsoleColor.DarkYellow
            };
            int i = 0;
            string appendages = string.Empty;
            foreach (var score in listOfOrders)
            {
                switch (i)
                {
                    case 0:
                        appendages = "ST";
                        break;
                    case 1:
                        appendages = "ND";
                        break;
                    case 2:
                        appendages = "RD";
                        break;
                    case 3:
                    case 4:
                        appendages = "TH";
                        break;
                }
                Console.ForegroundColor = colors[i];
                Console.WriteLine($"{count}{appendages, -8}{score.Value, -9}{score.Key}\n");
                count++;
                if (count == 6)
                {
                    break;
                }
                i++;
            }
        }

        private static List<string> PlayersScores(string nick)
        {
            Console.Clear();
            List<string> plScore = new List<string>();
            if (!File.Exists("score.txt"))
            {
                //PrintOnPosition(0, 3, "New high score: " + nick + "Points: " + points.ToString());
                plScore = new List<string>{ nick + "-" + points};
                File.WriteAllLines("score.txt", plScore);
            }
            else
            {
                string[] score = File.ReadAllLines("score.txt");
                plScore = new List<string>(score);
                Dictionary<string, int> dict = plScore
                    .Select(l => l.Split('-'))
                    .ToDictionary(a => a[0], a => int.Parse(a[1]));

                if (!dict.ContainsKey(nick))
                {
                    plScore.Add(nick + "-" + points);
                }
                else
                {
                    if (dict[nick] < points)
                    {
                        dict[nick] = points;
                    }               
                    plScore = dict
                        .Select(kvp => kvp.Key + "-" + kvp.Value)
                        .ToList();
                }
                File.WriteAllLines("score.txt", plScore);
                //if (points > dict.Values.Max())
                //{
                //    PrintOnPosition(0, 3, "New high score: " + nick + "Points: " + points.ToString());
                //}
            }
            return plScore;
        }

        /// <summary>
        /// Print final messages.
        /// </summary>
        private static void FinalMessages()
        {
            PrintOnPosition(19, 4, "GAME OVER!!!", ConsoleColor.DarkRed);
            if (points < 10)
            {
                PrintOnPosition(6, 5, $"Oooh poor litle baby! Just {points} dollars", ConsoleColor.DarkRed);
                PlayMusicEnd();
            }
            else if (points > 10 && points < 20)
            {
                PrintOnPosition(10, 5, $"You have {points} dollars.", ConsoleColor.DarkRed);
                PlayMusicStart();
            }
            else if (points > 20)
            {
                PrintOnPosition(10, 5, $"You are rich!!! {points} dollars.", ConsoleColor.DarkRed);
                PlayMusicStart();
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Print Basket
        /// </summary>
        private static void PrintBasket()
        {
            PrintOnPosition(41, 7, "\\", ConsoleColor.White);
            PrintOnPosition(41, 8, "|", ConsoleColor.White);
            PrintOnPosition(45, 8, "|", ConsoleColor.White);
            PrintOnPosition(42, 8, "___", ConsoleColor.White);
            PrintOnPosition(45, 7, "/", ConsoleColor.White);
        }

        /// <summary>
        /// Create new object
        /// </summary>
        /// <param name="dollars">new object - dollar</param>
        private static void CreateNewObject(List<Object> dollars)
        {
            for (int i = 0; i < dollars.Count; i++)
            {
                if (dollars[i].X > 42)
                {
                    points++;        
                    dollars.Remove(dollars[i]);
                    dollars.Add(new Object(9, 0, "$", ConsoleColor.Green, false));
               }          
            }
        }

        /// <summary>
        /// Add  more dollars
        /// </summary>
        /// <param name="countOfDollars">points</param>
        /// <param name="wallet">list of objects</param>
        /// <param name="chance">random number</param>
        private static void AddMoreDollars(int countOfDollars, List<Object> wallet, int chance)
        {
            if (countOfDollars > 2 && wallet.Count < 3)
            {
                //diff controler
                if (wallet.TrueForAll(x => x.X < chance))
                {
                    Object dolar = new Object(9, 0, "$", ConsoleColor.Green, false);
                    wallet.Add(dolar);
                }
            }
            if (chance < 2)
            {
               
                    Object dolar = new Object(9, 0, "$", ConsoleColor.Green, false);
                    wallet.Add(dolar);
                
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
            Thread.Sleep(500 - (int)speed);
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
        /// Print new lower state.
        /// </summary>
        /// <param name="objectDollar">new object for dollar</param>
        private static void PrintNewLowerState(Object objectDollar)
        {
            if (objectDollar.Y > 1)
            {
                Console.SetCursorPosition(objectDollar.X - 1, objectDollar.Y - 1);
                Console.Write(new string(' ', 3));
                PrintOnPosition(objectDollar.X, objectDollar.Y, objectDollar.Symbol, objectDollar.Color);
            }
        }

        /// <summary>
        /// Print new upper state
        /// </summary>
        /// <param name="objectDollar">new object for dollar</param>
        private static void PrintNewUpperState(Object objectDollar)
        {
            Console.SetCursorPosition(objectDollar.X - 1, objectDollar.Y + 1);
            Console.Write(" ");
            if (objectDollar.Y == 3)
            {
                Console.SetCursorPosition(objectDollar.X - 1, objectDollar.Y);
                Console.Write(" ");
            }
            PrintOnPosition(objectDollar.X, objectDollar.Y, objectDollar.Symbol, objectDollar.Color);
        }

        /// <summary>
        /// Print new bridge state
        /// </summary>
        /// <param name="bridge">new object for bridge</param>
        private static void PrintNewBridgeState(Object bridge)
        {
            Console.SetCursorPosition(0, bridge.Y);
            Console.WriteLine(new string(' ', Console.WindowWidth - 8));
            PrintOnPosition(bridge.X, bridge.Y, bridge.Symbol, bridge.Color);
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
        /// <summary>
        /// Play start music.
        /// </summary>
        public static void PlayMusicStart()
        {
            Console.Beep(659, 200); Console.Beep(659, 200); Thread.Sleep(167);
            Console.Beep(659, 200); Thread.Sleep(167); Console.Beep(523, 200);
            Console.Beep(659, 200); Thread.Sleep(200); Console.Beep(784, 200);
        }
    }
}