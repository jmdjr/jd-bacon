using UnityEngine;
using System;
using System.Collections;

using Object = UnityEngine.Object;
using System.Collections.Generic;

public delegate void MonoScriptEventHandler(MonoScriptEventArgs eventArgs);
public class MonoScriptEventArgs
{
    private MonoBehaviour script;
    public MonoBehaviour Script { get { return script; } }

    public MonoScriptEventArgs(MonoBehaviour scriptReference)
    {
        this.script = scriptReference;
    }
}


public class CollisionEventArgs
{

}