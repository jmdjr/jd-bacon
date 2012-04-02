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
    public class JDLevelObject
    {
        [ContentSerializer(Optional = true)]
        public JDAudioObject WorldMusic;

        public JDAppearance GroundAppearance;

        public JDCameraObject CameraObject;

        [ContentSerializer(CollectionItemName = "StaticObject")]
        public List<JDStaticObject> StaticObjectSet;

        [ContentSerializer(CollectionItemName = "CollectableObject")]
        public List<JDCollectableObject> CollectableObjectSet;

        [ContentSerializer(CollectionItemName = "PhysicalObject")]
        public List<JDPhysicalObject> PhysicalObjectSet;

        [ContentSerializer(CollectionItemName = "CharacterObject")]
        public List<JDCharacterObject> CharacterObjectSet;

        [ContentSerializer(CollectionItemName = "TriggerObject")]
        public List<JDTriggerObject> TriggerObjectSet;

        
        public JDLevelObject() { }
        
    }
}
