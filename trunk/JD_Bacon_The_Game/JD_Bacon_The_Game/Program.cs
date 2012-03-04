using System;

namespace JD_Bacon_The_Game
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (JDBaconTheGame game = new JDBaconTheGame())
            {
                game.Run();
            }
        }
    }
#endif
}

