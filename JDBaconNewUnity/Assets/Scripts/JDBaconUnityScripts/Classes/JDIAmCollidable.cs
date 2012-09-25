
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using SmoothMoves;

using Object = UnityEngine.Object;

public interface JDIAmCollidable
{
    void OnCollisionEnter(Collision other);
    void OnCollisionStay(Collision other);
    void OnCollisionExit(Collision other);
}
