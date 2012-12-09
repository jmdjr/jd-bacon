
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using SmoothMoves;

using Object = UnityEngine.Object;

public interface JDIAmCollidable
{
    event MonoBodyScriptEventHanlder ScriptCollisionEnter;
    event MonoBodyScriptEventHanlder ScriptCollisionStay;
    event MonoBodyScriptEventHanlder ScriptCollisionExit;

    void OnCollisionEnter(Collision other);
    void OnCollisionStay(Collision other);
    void OnCollisionExit(Collision other);
}
