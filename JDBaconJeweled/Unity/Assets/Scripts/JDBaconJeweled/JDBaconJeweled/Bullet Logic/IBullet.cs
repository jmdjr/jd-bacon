using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

using Object = UnityEngine.Object;
using System.Collections.Generic;

public interface IBullet : JDIObject
{
    int Id { get; set; }
    JDIBulletTypes BulletType { get; }
}
