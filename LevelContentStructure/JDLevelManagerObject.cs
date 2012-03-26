using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace LevelContentStructure
{
    public class JDLevelManagerObject
    {
        [ContentSerializer(CollectionItemName = "Level")]
        public List<JDLevelObject> Levels;

        public JDLevelManagerObject() { }
    }
}
