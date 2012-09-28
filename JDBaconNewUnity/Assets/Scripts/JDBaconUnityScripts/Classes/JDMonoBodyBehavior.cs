using UnityEngine;
using System;
using System.Collections;
using SmoothMoves;


public class JDMonoBodyBehavior : JDMonoBehavior, JDIAmCollidable
{
    public event MonoBodyScriptEventHanlder ScriptCollisionEnter;
    public event MonoBodyScriptEventHanlder ScriptCollisionStay;
    public event MonoBodyScriptEventHanlder ScriptCollisionExit;

    public void OnCollisionEnter(Collision other)
    {
        if (ScriptCollisionEnter != null)
        {
            ScriptCollisionEnter(new CollisionEventArgs(this, other));
        }
    }

    public void OnCollisionStay(Collision other)
    {
        if (ScriptCollisionStay != null)
        {
            ScriptCollisionStay(new CollisionEventArgs(this, other));
        }
    }

    public void OnCollisionExit(Collision other)
    {
        if (ScriptCollisionExit != null)
        {
            ScriptCollisionExit(new CollisionEventArgs(this, other));
        }
    }
}
