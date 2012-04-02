using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LevelContentStructure
{
    /// <summary>
    /// A Trigger Area will be meshes (could be covered in a texture like rain or fog or something.) that execute an EventFunction
    /// based on that function's test.  The trigger Area is unique, it is used to call one of the static event functions of a 
    /// global event collection.  The parameter's for any event function should only be the 
    /// 
    /// </summary>
    public class JDTriggerObject : JDObject
    {
        public string EventFunctionName;

        public List<JDObject> EventTargetObjectSet;

        public JDAppearance EventAppearance;

        public Vector3 Position;

        public JDTriggerObject() { }

    }
}
