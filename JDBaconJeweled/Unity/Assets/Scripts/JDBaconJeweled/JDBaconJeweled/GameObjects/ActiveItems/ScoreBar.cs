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
    static ScoreBar instance;
    public static ScoreBar Instance
    {
        get
        {
            if (instance == null)
            {
                if (GameObject.Find("Score Bar") != null)
                {
                    instance = GameObject.Find("Score Bar").GetComponent<ScoreBar>();
                }
                else
                {
                    Debug.Log("Can't find Score Bar GameObject!");
                }
            }

            return instance;
        }
    }

    DynamicText dText = null;

    public override void Awake()
    {
        base.Awake();

        dText = DynamicText.GetTextMesh(this);
    }

    public override void Update()
    {
        base.Update();

    }

    public void SetScore(string text)
    {
        this.dText.SetText(text);
    }
}
