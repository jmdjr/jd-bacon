using UnityEngine;
using System;
using System.Collections;
using SmoothMoves;
using System.Collections.Generic;

public class JDMonoBehavior : MonoBehaviour, JDIHaveScriptHandles, JDIObject
{
    public event MonoScriptEventHandler ScriptAwake;
    public event MonoScriptEventHandler ScriptStart;
    public event MonoScriptEventHandler ScriptUpdate;
    public event MonoScriptEventHandler ScriptDestroy;

    public Rigidbody Body { get { return this.rigidbody; } }
    public Collider Collider { get { return this.rigidbody.collider; } }
    public Vector3 ColliderCenter { get { return this.Collider.bounds.center; } }
    public bool IsPaused { get { return Time.timeScale == 0; } }

    public List<JDIObject> JDCollection = new List<JDIObject>();
    
    public void Pause()
    {
        Time.timeScale = 0f;
    }
    public void UnPause()
    {
        Time.timeScale = 1f;
    }

    public virtual void Awake()
    {
        if (ScriptAwake != null)
        {
            ScriptAwake(new MonoScriptEventArgs(this));
        }
    }
    public virtual void Start()
    {
        if (ScriptStart != null)
        {
            ScriptStart(new MonoScriptEventArgs(this));
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

    public string Name
    {
        get
        {
            throw new NotImplementedException();
        }
        set
        {
            throw new NotImplementedException();
        }
    }

    public JDIObjectTypes JDType
    {
        get { return JDIObjectTypes.SCRIPT; }
    }

    public bool ReportStatistics(JDIStatTypes stat, int valueShift)
    {
        throw new NotImplementedException();
    }
}
