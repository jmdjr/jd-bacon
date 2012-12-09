using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JDBaconJeweled
{
    public class MainHolder
    {
        public static void Main()
        {
            Console.WriteLine("Please specify the dimensions of the grid (must be greater than a 5x5):");
            string dimensions = Console.ReadLine();

            Console.WriteLine("Number of bullets decoded: " + BulletFactory.Instance.Bullets.Count);
            Console.WriteLine("Bullet Information:");

            foreach (JDBullet bullet in BulletFactory.Instance.Bullets)
            {
                Console.WriteLine("Name: " + bullet.Name);
                Console.WriteLine("Id: " + bullet.Id);
                Console.WriteLine("JDType: " + bullet.JDType);
                Console.WriteLine("BulletType: " + bullet.BulletType);
                Console.WriteLine("\n");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }
    }
}
