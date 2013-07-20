
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using SmoothMoves;

using Object = UnityEngine.Object;

public interface JDISavableObject : JDIObject
{
    string SaveData();
    void LoadData(string savefiletext);
}