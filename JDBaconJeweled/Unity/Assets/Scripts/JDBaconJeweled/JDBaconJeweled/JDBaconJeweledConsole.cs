using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEditor;
using System.Collections;

using Object = UnityEngine.Object;
using Random = System.Random;

namespace JDBaconJeweled
{
    public class MainHolder
    {
#if DEBUG || RELEASE
        public static void Main()
        {
            Regex rex = new Regex("[0-1]?[0-9]x[0-1]?[0-9]");
            string command = "";

            var stuffs = Initialize(rex, command);

            BulletMatrix frame = new BulletMatrix(stuffs.Y, stuffs.X);

            frame.Load(false);
            //Console.Clear();
            frame.BalanceGrid(true, true);
            do
            {
                command = "";
                rex = new Regex("[0-9A-Z][0-9A-Z]x[0-9A-Z][0-9A-Z]|quit");
                command = PromptForCommand(rex, command, frame);
                
                frame.enablePrinting = true;

                if (!Regex.IsMatch(command, "quit"))
                {
                    KeyValuePair<string, string> stuff = ParseCommandIntoSwapCodes(command);
                    Position2D start = frame.CommandToPosition(stuff.Key);
                    Position2D end = frame.CommandToPosition(stuff.Value);

                    if (!frame.CanSwapPositions(start, end))
                    {
                        Console.WriteLine("these pieces can't be swapped...");
                        System.Threading.Thread.Sleep(750);
                        //Console.Clear();
                        continue;
                    }

                    //Console.Clear();
                    frame.Debug_PrintBulletMatrix();
                    System.Threading.Thread.Sleep(100);

                    frame.SwapPositions(start, end);
                    //Console.SetCursorPosition(0, 0);
                    frame.Debug_PrintBulletMatrix();
                    System.Threading.Thread.Sleep(100);

                    var matches = frame.CollectMatchedBullets();

                    if (matches.Count == 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Bad Swap...");
                        System.Threading.Thread.Sleep(750);
                        frame.SwapPositions(start, end);
                        //Console.Clear();
                    }

                    frame.BalanceGrid(true, false);

                    //Console.SetCursorPosition(0, 0);
                    frame.Debug_PrintBulletMatrix();

                    if (!frame.CanMatchMore())
                    {
                        Console.WriteLine();
                        Console.WriteLine("No more available matches :'(...");
                        System.Threading.Thread.Sleep(750);
                        command = "";
                        rex = new Regex("[YyNn]");
                        do
                        {
                            Console.WriteLine();
                            Console.WriteLine("Would you like to play again?[(Y)es or (N)o]:");
                            command = Console.ReadLine();

                        } while (!rex.IsMatch(command));

                        if (Regex.IsMatch(command, "[Yy]"))
                        {
                            frame.UnLoad();
                            frame.Load(true);
                            frame.BalanceGrid(false, true);
                            command = "";
                        }
                        else
                        {
                            command = "quit";
                        }
                    }
                }
            } while (command != "quit");

            Console.WriteLine("GameOver!");
            Console.WriteLine("Press any key to continue...");
            //Console.ReadKey(true);
        }

        private static string PromptForCommand(Regex rex, string command, BulletMatrix frame)
        {
            do
            {
                //Console.Clear();
                frame.Debug_PrintBulletMatrix();
                Console.WriteLine();
                Console.WriteLine("Please specify two adjacent pieces to swap, or type quit to leave.");
                Console.WriteLine("(example: 5Ax52):");
                command = Console.ReadLine();

            } while (!rex.IsMatch(command) && command != "quit");
            return command;
        }

        private static Position2D Initialize(Regex rex, string command)
        {
            do
            {
                Console.WriteLine("Please specify the dimensions of the grid (ex. 10x10):");
                command = Console.ReadLine();
                Console.WriteLine();

                if (command == "") command = "10x10";

            } while (!rex.IsMatch(command));

            var dimensions = command;

            rex = new Regex("I|U");
            do
            {
                Console.WriteLine("Please specify whether each bullet is counted (I) or only 1 from that type (U):");
                command = Console.ReadLine();
                Console.WriteLine();

                if (command == "") command = "U";
            }
            while (!rex.IsMatch(command));

            switch (command)
            {
                case "I":
                    GameStatistics.Instance.AllowedBulletStat = JDIStatTypes.INDIVIDUALS;
                    break;

                case "U":
                    GameStatistics.Instance.AllowedBulletStat = JDIStatTypes.UNIQUES;
                    break;
            }

            return ParseCommandIntoHeightWidth(dimensions);
        }

        private static Position2D ParseCommandIntoHeightWidth(string command)
        {
            var stuffs = command.Split(new string[] { "x" }, StringSplitOptions.RemoveEmptyEntries);
            return new Position2D(int.Parse(stuffs[0]), int.Parse(stuffs[1]));
        }

        private static KeyValuePair<string, string> ParseCommandIntoSwapCodes(string command)
        {
            var stuffs = command.Split(new string[] { "x" }, StringSplitOptions.RemoveEmptyEntries);
            return new KeyValuePair<string, string>(stuffs[1], stuffs[0]);
        }
#endif
    }
}
