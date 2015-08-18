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
    protected MenuNavigator navigator;
    protected static Vector3 BackLayer = new Vector3(0, 0, -5);
    protected Dictionary<string, JDMenuButton> menuButtons = new Dictionary<string, JDMenuButton>();
    public bool IsTopLevel { get { return this.gameObject.transform.position.z == 0; } }
    public bool HasBackButton { get { return this.menuButtons.ContainsKey("Back Button"); } }
    
    public virtual void RegisterTouchingEvents()
    {
        toucher.PickUpGameObject += toucher_PickUpGameObject;
    }

    private void toucher_PickUpGameObject(GameObjectTransferEventArgs eventArgs)
    {
        if (this.menuButtons.Keys.FirstOrDefault(s => s == eventArgs.GameObject.name) != null)
        {
            this.menuButtons[eventArgs.GameObject.name].GoToAssignedMenu();
            toucher.ClearHistory();
        }
    }

    public virtual void UnregisterTouchingEvents()
    {
        toucher.PickUpGameObject -= toucher_PickUpGameObject;
    }

    public abstract void MenuUpdate();
    public virtual void MenuEnter() { }
    public virtual void MenuLeave() { }
    public virtual void AssignButtonMenus() { }

    public JDMenuButton GetBackButton()
    {
        if (this.HasBackButton)
        {
            return this.menuButtons["Back Button"];
        }

        return null;
    }
    public void BringToTopLayer()
    {
        this.gameObject.transform.localPosition = Vector3.zero;
        MenuEnter();
        RegisterTouchingEvents();
    }

    public JDMenu SendToBackLayer()
    {
        this.gameObject.transform.localPosition = BackLayer;
        MenuLeave();
        UnregisterTouchingEvents();
        return this;
    }

    public override void Awake()
    {
        base.Awake();
        toucher = GameObjectToucher.Instance;
        navigator = MenuNavigator.Instance;

        Transform Buttons = this.gameObject.transform.FindChild("Buttons");
        if (Buttons != null)
        {
            foreach (Transform b in Buttons)
            {
                JDMenuButton script = b.gameObject.AddComponent<JDMenuButton>();
                this.menuButtons.Add(b.gameObject.name, script);
            }
        }
    }
    public override void Start()
    {
        base.Start();
        this.AssignButtonMenus();
    }
    public override void Update()
    {
        base.Update();

        if (this.IsTopLevel)
        {
            this.MenuUpdate();
        }
    }
}
