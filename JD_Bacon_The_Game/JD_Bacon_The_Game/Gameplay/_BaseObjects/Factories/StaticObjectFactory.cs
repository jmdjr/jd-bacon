using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using LevelContentStructure;

namespace JD_Bacon_The_Game
{
    public static class StaticObjectFactory
    {
        private static Dictionary<string, Type> DefinedModels = new Dictionary<string, Type>();

        private static bool IsInitialized = false;

        public static void InitializeFactory()
        {
            IsInitialized = true;
        }

        public static BaseEntityModel Spawn(JDStaticObject objContent, Game game)
        {
            if (!DefinedModels.Keys.Contains(objContent.TypeName))
            {
            }

            return new GenericStaticObject(game, objContent);
        }
    }
}
