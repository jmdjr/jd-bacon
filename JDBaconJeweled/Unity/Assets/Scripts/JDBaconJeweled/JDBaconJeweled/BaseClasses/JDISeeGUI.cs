using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    public interface JDISeeGUI
    {
        event MonoScriptEventHandler ScriptGUI;
        void OnGUI();
    }
