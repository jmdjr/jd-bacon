using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace LevelContentStructure
{
    public class JDCameraObject : JDObject
    {

        public Vector3 Position;
        public Vector3 Target;
        public float NearPlaneDistance;
        public float FarPlaneDistance;

        public JDCameraObject() { }
    }
}
