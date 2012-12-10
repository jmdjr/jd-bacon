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
            Regex rex = new Regex("[0-1]?[0-9]x[0-1]?[0-9]");
            string dimensions = "";
            do
            {
                Console.WriteLine("Please specify the dimensions of the grid (must be greater than a 5x5):");
                dimensions = Console.ReadLine();
                Console.WriteLine();

            } while (!rex.IsMatch(dimensions));

            //string dimensions = "9x9";

            var stuffs = dimensions.Split(new string[] { "x" }, StringSplitOptions.RemoveEmptyEntries);

            int w = int.Parse(stuffs[0]);
            int h = int.Parse(stuffs[1]);

            BulletMatrix frame = new BulletMatrix(h, w);

            frame.Load();

            List<Vector2> matches = frame.CollectMatchedBullets();
            frame.enablePrinting = true;
            do
            {
                frame.DropMatchedBullets(matches);
                frame.ShiftItemsDown(matches);
                matches = frame.CollectMatchedBullets();
            }
            while (matches.Count > 0);
            BulletFactory.Instance.ResetStatistics();

            Console.Clear();
            frame.Debug_PrintBulletMatrix();
            string command = "";
            rex = new Regex("[0-9A-Z][0-9A-Z]x[0-9A-Z][0-9A-Z]");

            frame.enablePrinting = true;
            do
            {
                do
                {
                    Console.WriteLine("Please specify two adjacent pieces to swap, or type quit to leave.");
                    Console.WriteLine("(example: 5Ax52):");
                    command = Console.ReadLine();

                } while (!rex.IsMatch(command) || command == "quit");

                if (command != "quit")
                {
                    stuffs = command.Split(new string[] { "x" }, StringSplitOptions.RemoveEmptyEntries);
                    Tuple<int, int> start = frame.CommandToPosition(stuffs[0]);
                    Tuple<int, int> end = frame.CommandToPosition(stuffs[1]);

                    if (!frame.CanSwapPositions(start, end))
                    {
                        Console.WriteLine("these pieces can't be swapped...");
                        System.Threading.Thread.Sleep(500);
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
                    frame.enablePrinting = true;

                    do
                    {
                        frame.DropMatchedBullets(matches);
                        frame.ShiftItemsDown(matches);
                        matches = frame.CollectMatchedBullets();
                    }
                    while (matches.Count > 0);
                }
            } while (command != "quit");

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }
    }
}
