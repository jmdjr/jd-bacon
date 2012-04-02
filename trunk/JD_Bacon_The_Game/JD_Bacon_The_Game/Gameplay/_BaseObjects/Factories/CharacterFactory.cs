using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LevelContentStructure;
using Microsoft.Xna.Framework;

namespace JD_Bacon_The_Game
{
    /// <summary>
    /// Factory used to track and build Character Entities.
    /// </summary>
    public static class CharacterFactory
    {
        private static Dictionary<string, Type> DefinedModels = new Dictionary<string, Type>();

        private static bool IsInitialized = false;

        public static void InitializeFactory()
        {
            IsInitialized = true;
        }

        public static BaseEntityModel Spawn(JDCharacterObject objContent, Game game)
        {
            if (!DefinedModels.Keys.Contains(objContent.TypeName))
            {
            }

            return new GenericCharacterObject(game, objContent);
        }
    }
}
