using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;
using System.Collections.Generic;

[Serializable]
public class JDLevel : JDIObject
{
    public string Name { get; set; }
    public int Id { get; set; }
    public float ZombieCollectRate { get; set; }
    public float ZombieLimit { get; set; }

    public JDIObjectTypes JDType { get { return JDIObjectTypes.OBJECT; } }

    public bool ReportStatistics(JDIStatTypes stat, int valueShift)
    {
        throw new NotImplementedException();
    }
}