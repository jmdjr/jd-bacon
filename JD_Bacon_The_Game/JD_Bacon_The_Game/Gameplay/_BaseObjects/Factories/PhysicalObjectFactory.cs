using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LevelContentStructure;
using Microsoft.Xna.Framework;

namespace JD_Bacon_The_Game
{
    public static class PhysicalObjectFactory
    {
        private static Dictionary<string, Type> DefinedModels = new Dictionary<string, Type>();
        
        private static bool IsInitialized = false;

        public static void InitializeFactory()
        {
            IsInitialized = true;
        }

        public static BaseEntityModel Spawn(JDPhysicalObject objContent, Game game)
        {
            if (!DefinedModels.Keys.Contains(objContent.TypeName))
            {
            }

            return new GenericPhysicalObject(game, objContent);
        }
    }
}
