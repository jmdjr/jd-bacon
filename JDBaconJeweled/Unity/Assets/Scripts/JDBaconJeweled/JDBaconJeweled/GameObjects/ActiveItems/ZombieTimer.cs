
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
public class ZombieTimer : JDMonoBodyBehavior
{
    public event GenericStatusEvent StartTimer;
    public event GenericStatusEvent StoppedTimer;
    public event GenericStatusEvent EndedTimer;

    public int StepInterval = Time.captureFramerate;

    private GameObject sgo;
    private GameObject startGO
    {
        get
        {
            if (sgo == null)
            {
                sgo = GameObject.Find("Start");
            }
            return sgo;
        }
    }
    private GameObject zbar;
    private GameObject zombieBarGO
    {
        get
        {
            if (zbar == null)
            {
                zbar = GameObject.Find("Zombies");
            }
            return zbar;
        }
    }
    private GameObject ego;
    private GameObject endGO
    {
        get
        {
            if (ego == null)
            {
                ego = GameObject.Find("End");
            }
            return ego;
        }
    }

    static ZombieTimer instance;
    public static ZombieTimer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = JDGame.GameMaster.GetComponent<ZombieTimer>();
            }

            return instance;
        }
    }

    public override void Start()
    {
        base.Start();
        Debug.Log("Time capture Framerate: " + StepInterval);
    }

    public void StartTimerCycle()
    {
        if (StartTimer != null)
        {
            this.StartTimer(new GenericStatusEventArgs(GenericStatusFlags.START));
        }
    }

    public override void Update()
    {
        base.Update();


    }
}
