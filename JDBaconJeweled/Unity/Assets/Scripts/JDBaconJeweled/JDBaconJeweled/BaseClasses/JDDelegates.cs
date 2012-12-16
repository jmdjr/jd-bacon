using UnityEngine;
using System;
using System.Collections;

using Object = UnityEngine.Object;
using System.Collections.Generic;

public delegate void MonoScriptEventHandler(MonoScriptEventArgs eventArgs);
public class MonoScriptEventArgs
{
    private JDMonoBehavior script;
    public JDMonoBehavior Script { get { return script; } }

    public MonoScriptEventArgs(JDMonoBehavior scriptReference)
    {
        this.script = scriptReference;
    }
}

public delegate void MonoBodyScriptEventHanlder(CollisionEventArgs eventArgs);
public class CollisionEventArgs : MonoScriptEventArgs
{
    private Collision other;
    public Collision Other { get { return this.other; } }
    private JDCollisionObject jdOther;
    public JDCollisionObject JdOther { get { return this.jdOther; } }

    public CollisionEventArgs(JDMonoBehavior scriptReference, Collision other)
        : base(scriptReference)
    {
        this.other = other;
        this.jdOther = new JDCollisionObject(other);
    }
}

public class JDCollisionObject
{
    public JDIObjectTypes ObjectType { get; protected set; }
    public TagTypes ObjectTagType { get; protected set; }
    public JDIObject ScriptObject { get; protected set; }

    public JDCollisionObject(Collision other)
    {
        JDMonoBehavior behavior = other.gameObject.GetComponent<JDMonoBehavior>();
        this.ObjectTagType = TagTypeExtension.ToTagType(other.collider.tag);
        Debug.Log(behavior != null);
    }
}