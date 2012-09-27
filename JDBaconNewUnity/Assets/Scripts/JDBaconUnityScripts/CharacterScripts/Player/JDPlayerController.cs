using UnityEngine;
using System;
using System.Collections;
using SmoothMoves;


public class JDPlayerController : MonoBehaviour, JDIHaveScriptHandles, JDIAmCollidable
{
    public JDCharacter CharacterProperties = new JDCharacter();
    public event MonoScriptEventHandler ScriptAwake;
    public event MonoScriptEventHandler ScriptUpdate;
    public event MonoScriptEventHandler ScriptDestroy;
    public event MonoScriptEventHandler ScriptCollisionEnter;
    public event MonoScriptEventHandler ScriptCollisionStay;
    public event MonoScriptEventHandler ScriptCollisionExit;

    public void OnAwake() 
    {
        if (ScriptAwake != null)
        {
            ScriptAwake(new MonoScriptEventArgs(this));
        }
    }

    public void OnUpdate()
    {
        if (ScriptUpdate != null)
        {
            ScriptUpdate(new MonoScriptEventArgs(this));
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        throw new NotImplementedException();
    }

    public void OnCollisionStay(Collision other)
    {
        throw new NotImplementedException();
    }

    public void OnCollisionExit(Collision other)
    {
        throw new NotImplementedException();
    }
}