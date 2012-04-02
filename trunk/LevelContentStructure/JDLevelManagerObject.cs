using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace LevelContentStructure
{
    public class JDLevelManagerObject
    {
        [ContentSerializer(CollectionItemName = "LevelName")]
        public List<string> Levels;
        public JDLevelManagerObject() { }
    }
}
