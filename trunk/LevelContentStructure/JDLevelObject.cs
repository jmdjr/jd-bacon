using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace LevelContentStructure
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class JDLevelObject : JDObject
    {
        #region Public Serialized Fields
        public JDAppearance GroundAppearance;

        [ContentSerializer(CollectionItemName = "PhysicalObject")]
        public List<JDPhysicalObject> PhysicalObjectSet;

        [ContentSerializer(CollectionItemName = "StaticObject")]
        public List<JDStaticObject> StaticObjectSet;

        [ContentSerializer(CollectionItemName = "CollectableObject")]
        public List<JDCollectableObject> CollectableObjectSet;
        public JDLevelObject() { }

        [ContentSerializer(CollectionItemName = "CharacterObject")]
        public List<JDCharacterObject> CharacterObjectSet;
        #endregion 

    }
}
