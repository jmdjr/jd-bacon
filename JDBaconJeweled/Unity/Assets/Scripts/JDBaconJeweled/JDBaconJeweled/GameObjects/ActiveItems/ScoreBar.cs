using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;

public class ScoreBar : JDMonoBehavior
{
    DynamicText dText = null;

    public override void Awake()
    {
        base.Awake();

        dText = DynamicText.GetTextMesh(this);
        dText.SetText("0");
    }

    public override void Update()
    {
        base.Update();

        dText.SetText(Time.frameCount.ToString());

    }
}
