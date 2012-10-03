
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using SmoothMoves;

using Object = UnityEngine.Object;

public interface JDIWeapon : JDIObject
{
    int DamageAmount { get; set; }
    bool IsActive { get; set; }
    int CooldownTime { get; set; }
    float Range { get; set; }
}