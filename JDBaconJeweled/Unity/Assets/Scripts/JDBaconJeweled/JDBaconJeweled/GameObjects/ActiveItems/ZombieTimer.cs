﻿
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;

//
//Consists of the timer bar:"Zombies", start indicator:"Start", and end indicator:"End".
//
public class ZombieTimer
{
    public event GenericStatusEvent StartTimer;
    public event GenericStatusEvent StoppedTimer;
    public event GenericStatusEvent EndedTimer;
}
