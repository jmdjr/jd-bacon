
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using SmoothMoves;

using Object = UnityEngine.Object;

interface JDIObject
{
    string Name { get; set; }
    bool ReportStatistic(string statName, int valueShift);
}
