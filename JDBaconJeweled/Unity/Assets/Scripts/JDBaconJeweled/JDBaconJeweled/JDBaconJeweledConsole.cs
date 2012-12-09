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

            //Console.WriteLine("Please specify the dimensions of the grid (must be greater than a 5x5):");
            //string dimensions = Console.ReadLine();
            string dimensions = "9x9";
            var stuffs = dimensions.Split(new string[] { "x" }, StringSplitOptions.RemoveEmptyEntries);

            int w = int.Parse(stuffs[0]);
            int h = int.Parse(stuffs[1]);

            BulletMatrix frame = new BulletMatrix(h, w);

            frame.Load();

            
            frame.Debug_PrintBulletMatrix();
            Console.WriteLine();
            Console.WriteLine("The Game will now begin dropping and shifting until all matches are gone...");
            Console.ReadKey(true);

            List<Vector2> matches = frame.CollectMatchedBullets();
            frame.Debug_PrintBulletMatrix();
            Console.WriteLine("Total Matched this round: " + matches.Count);
            do
            {
                frame.DropMatchedBullets(matches);

                frame.ShiftItemsDown(matches);
                matches = frame.CollectMatchedBullets();
            }
            while (matches.Count > 0);


            frame.Debug_PrintBulletMatrix();

            Console.WriteLine();


            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }
    }
}
