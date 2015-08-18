using SmoothMoves;
using System.Collections.Generic;
using UnityEngine;

public class JDMonoGUIBodyBehavior : JDMonoBodyBehavior, JDISeeGUI
{
    public event MonoScriptEventHandler ScriptGUI;

    public void OnGUI()
    {
        if (ScriptGUI != null)
        {
            ScriptGUI(new MonoScriptEventArgs(this));
        }
    }
}