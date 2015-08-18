using SmoothMoves;
using System.Collections.Generic;
using UnityEngine;

public class JDMonoGuiBehavior : JDMonoBehavior, JDISeeGUI
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
