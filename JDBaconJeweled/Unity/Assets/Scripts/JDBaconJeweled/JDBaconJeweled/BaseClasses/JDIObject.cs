
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using SmoothMoves;

using Object = UnityEngine.Object;

public interface JDIObject
{
    string Name { get; set; }
    JDIObjectTypes JDType { get; }
    bool ReportStatistics(JDIStatTypes stat, int valueShift);
}
