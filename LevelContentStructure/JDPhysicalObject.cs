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
    public class JDPhysicalObject : JDObject
    {
        #region Public Serialized Fields
        public JDAppearance Appearance;

        public JDPhysicalObject() { }
        #endregion 
    }
}
