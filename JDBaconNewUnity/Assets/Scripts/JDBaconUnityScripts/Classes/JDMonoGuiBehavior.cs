using SmoothMoves;
using System.Collections.Generic;
using UnityEngine;

public class JDMonoGuiBehavior : JDMonoBehavior, JDISeeGUI
{
    public event MonoScriptEventHandler ScriptGUI;

    public List<JDIObject> JDCollection = new List<JDIObject>();

    public void OnGUI()
    {
        if (ScriptGUI != null)
        {
            ScriptGUI(new MonoScriptEventArgs(this));
        }
    }
}
