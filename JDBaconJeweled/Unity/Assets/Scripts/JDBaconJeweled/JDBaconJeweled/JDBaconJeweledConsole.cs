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
        public static void Main()
        {
#if DEBUG || RELEASE
            Regex rex = new Regex("[0-1]?[0-9]x[0-1]?[0-9]");
            string command = "";
            do
            {
                Console.WriteLine("Please specify the dimensions of the grid (must be >= 5x5):");
                command = Console.ReadLine();
                Console.WriteLine();

                if (command == "") command = "5x5";

            } while (!rex.IsMatch(command));

            var stuffs = command.Split(new string[] { "x" }, StringSplitOptions.RemoveEmptyEntries);

            int w = int.Parse(stuffs[0]);
            int h = int.Parse(stuffs[1]);

            BulletMatrix frame = new BulletMatrix(h, w);

            frame.Load();

            List<KeyValuePair<int, int>> matches = frame.CollectMatchedBullets();
            frame.enablePrinting = false;
            do
            {
                frame.DropMatchedBullets(matches);
                frame.ShiftItemsDown(matches);
                matches = frame.CollectMatchedBullets();
            }
            while (matches.Count > 0);
            BulletFactory.Instance.ResetStatistics();

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

            command = "";
            rex = new Regex("[0-9A-Z][0-9A-Z]x[0-9A-Z][0-9A-Z]|quit");
            do
            {
                do
                {
                    Console.Clear();
                    frame.Debug_PrintBulletMatrix();
                    Console.WriteLine();
                    Console.WriteLine("Please specify two adjacent pieces to swap, or type quit to leave.");
                    Console.WriteLine("(example: 5Ax52):");
                    command = Console.ReadLine();

                } while (!rex.IsMatch(command) && command != "quit");

                if (command != "quit")
                {
                    stuffs = command.Split(new string[] { "x" }, StringSplitOptions.RemoveEmptyEntries);
                    KeyValuePair<int, int> start = frame.CommandToPosition(stuffs[0]);
                    KeyValuePair<int, int> end = frame.CommandToPosition(stuffs[1]);

                    if (!frame.CanSwapPositions(start, end))
                    {
                        Console.WriteLine("these pieces can't be swapped...");
                        System.Threading.Thread.Sleep(750);
                        Console.Clear();
                        continue;
                    }

                    Console.Clear();
                    frame.Debug_PrintBulletMatrix();
                    System.Threading.Thread.Sleep(100);

                    frame.SwapPositions(start, end);
                    Console.SetCursorPosition(0, 0);
                    frame.Debug_PrintBulletMatrix();
                    System.Threading.Thread.Sleep(100);

                    matches = frame.CollectMatchedBullets();
                    if (matches.Count == 0)
                    {
                        Console.WriteLine("Bad Swap...");
                        System.Threading.Thread.Sleep(750);
                        frame.SwapPositions(start, end);
                        Console.Clear();
                    }
                    frame.enablePrinting = true;
                    do
                    {
                        frame.DropMatchedBullets(matches);
                        frame.ShiftItemsDown(matches);
                        matches = frame.CollectMatchedBullets();
                    }
                    while (matches.Count > 0);

                    Console.SetCursorPosition(0, 0);
                    frame.Debug_PrintBulletMatrix();

                    if (!frame.CanMatchMore())
                    {
                        Console.WriteLine("No more available matches :'(...");
                        System.Threading.Thread.Sleep(750);
                        break;
                    }
                }
            } while (command != "quit");

            Console.WriteLine("GameOver!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
#endif
        }
    }
}
