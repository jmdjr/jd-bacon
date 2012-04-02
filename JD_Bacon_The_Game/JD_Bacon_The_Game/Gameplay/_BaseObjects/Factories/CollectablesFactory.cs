using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LevelContentStructure;
using Microsoft.Xna.Framework;

namespace JD_Bacon_The_Game
{
    /// <summary>
    /// This factory spawns anything that the hero can consume/use within the level.
    /// </summary>
    public static class CollectablesFactory
    {
        private static Dictionary<string, Type> DefinedModels = new Dictionary<string, Type>();

        private static bool IsInitialized = false;

        public static void InitializeFactory()
        {
            IsInitialized = true;
        }

        public static BaseEntityModel Spawn(JDCollectableObject objContent, Game game)
        {
            if (!DefinedModels.Keys.Contains(objContent.TypeName))
            {
            }

            return new GenericCollectableObject(game, objContent);
        }
    }
}
