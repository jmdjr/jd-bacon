using UnityEngine;
using System;
using System.Collections;
using SmoothMoves;
using System.Collections.Generic;

public class JDMonoBehavior : MonoBehaviour, JDIHaveScriptHandles
{
    public event MonoScriptEventHandler ScriptAwake;
    public event MonoScriptEventHandler ScriptUpdate;
    public event MonoScriptEventHandler ScriptDestroy;

    public Rigidbody Body { get { return this.rigidbody; } }
    public Collider Collider { get { return this.rigidbody.collider; } }
    public Vector3 ColliderCenter { get { return this.Collider.bounds.center; } }

    public List<JDIObject> JDCollection = new List<JDIObject>();

    public virtual void Awake()
    {
        if (ScriptAwake != null)
        {
            ScriptAwake(new MonoScriptEventArgs(this));
        }
    }

    public virtual void Update()
    {
        if (ScriptUpdate != null)
        {
            ScriptUpdate(new MonoScriptEventArgs(this));
        }
    }

    public virtual void Destroy()
    {
        if (ScriptDestroy != null)
        {
            ScriptDestroy(new MonoScriptEventArgs(this));
        }
    }
}
