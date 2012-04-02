using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LevelContentStructure
{
    public abstract class JDObject
    {
        // The unique ID of this entity.  in lists, each JDObject requires a unique ID.
        public int ID;

        // The type of Object to tranform this JDObject into.  Going to be used by the master 
        //  factory during level construction.
        public string TypeName;

        public JDObject() { }
    }
}
