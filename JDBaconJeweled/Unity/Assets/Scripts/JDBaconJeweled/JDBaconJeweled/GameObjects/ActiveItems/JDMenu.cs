using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;

// menus should all be situated so that they share the same coordinates, at origin.
public abstract class JDMenu : JDMonoGuiBehavior
{
    protected GameObjectToucher toucher;
    protected static Vector3 BackLayer = new Vector3(0, 0, -5);
    public override void Awake()
    {
        base.Awake();

        toucher = GameObjectToucher.Instance;
    }

    public bool IsTopLevel { get { return this.gameObject.transform.position.z == 0; } }

    public void BringToTopLayer() 
    {
        this.gameObject.transform.localPosition = Vector3.zero;
        RegisterTouchingEvents();
    }

    private void SendToBackLayer()
    {
        this.gameObject.transform.localPosition = BackLayer;
        UnregisterTouchingEvents();
    }

    public abstract void RegisterTouchingEvents();
    public abstract void UnregisterTouchingEvents();

    public override void Update()
    {
        base.Update();

        if (this.IsTopLevel)
        {
            this.MenuUpdate();
        }
    }

    public abstract void MenuUpdate();
}
