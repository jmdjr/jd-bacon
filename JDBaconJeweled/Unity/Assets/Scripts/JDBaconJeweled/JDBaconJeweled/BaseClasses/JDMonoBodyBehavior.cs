using UnityEngine;
using System;
using System.Collections;
using SmoothMoves;


public class JDMonoBodyBehavior : JDMonoBehavior, JDIAmCollidable
{
    public event MonoBodyScriptEventHanlder ScriptCollisionEnter;
    public event MonoBodyScriptEventHanlder ScriptCollisionStay;
    public event MonoBodyScriptEventHanlder ScriptCollisionExit;

    public event MonoTriggerScriptEventHanlder ScriptTriggerEnter;
    public event MonoTriggerScriptEventHanlder ScriptTriggerStay;
    public event MonoTriggerScriptEventHanlder ScriptTriggerExit;

    public virtual void OnCollisionEnter(Collision other)
    {
        if (ScriptCollisionEnter != null)
        {
            ScriptCollisionEnter(new CollisionEventArgs(this, other));
        }
    }
    public virtual void OnCollisionStay(Collision other)
    {
        if (ScriptCollisionStay != null)
        {
            ScriptCollisionStay(new CollisionEventArgs(this, other));
        }
    }
    public virtual void OnCollisionExit(Collision other)
    {
        if (ScriptCollisionExit != null)
        {
            ScriptCollisionExit(new CollisionEventArgs(this, other));
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (ScriptTriggerEnter != null)
        {
            ScriptTriggerEnter(new ColliderEventArgs(this, other));
        }
    }
    public virtual void OnTriggerStay(Collider other)
    {
        if (ScriptTriggerStay != null)
        {
            ScriptTriggerStay(new ColliderEventArgs(this, other));
        }
    }
    public virtual void OnTriggerExit(Collider other)
    {
        if (ScriptTriggerExit != null)
        {
            ScriptTriggerExit(new ColliderEventArgs(this, other));
        }
    }
}
