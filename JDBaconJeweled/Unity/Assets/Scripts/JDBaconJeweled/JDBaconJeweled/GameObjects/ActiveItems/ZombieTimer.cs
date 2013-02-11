
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
    //public event GenericStatusEvent StoppedTimer;
    //public event GenericStatusEvent EndedTimer;

    private bool isRunning = false;

    #region Cached References
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

    private float distanceSE;
    private float DistanceSE
    {
        get
        {
            if (distanceSE == 0)
            {
                distanceSE = Math.Abs(endGO.transform.position.x - startGO.transform.position.x);
            }

            return distanceSE;
        }
    }
    #endregion

    static ZombieTimer instance;
    public static ZombieTimer Instance
    {
        get
        {
            if (instance == null)
            {
                if (GameObject.Find("ZombieTimer") != null)
                {
                    instance = GameObject.Find("ZombieTimer").GetComponent<ZombieTimer>();
                }
            }

            return instance;
        }
    }

    public override void Start()
    {
        base.Start();
        LevelManager.Instance.FirstLevel();

    }

    public void StartTimerCycle()
    {
        if (StartTimer != null)
        {
            this.StartTimer(new GenericStatusEventArgs(GenericStatusFlags.START));
        }

        isRunning = true;
    }

    public void PauseTimer()
    {
        this.isRunning = false;
    }

    public void ResumeTimer()
    {
        this.isRunning = true;
    }

    public override void Update()
    {
        base.Update();
        
        if (isRunning && !this.IsPaused)
        {
            LevelManager.Instance.StepZombieCount();
            float /*xScale = zombieBarGO.transform.localScale.x,*/ yScale = zombieBarGO.transform.localScale.y, zScale = zombieBarGO.transform.localScale.z;

            float xpos = startGO.transform.position.x, ypos = zombieBarGO.transform.position.y, zpos = zombieBarGO.transform.position.z;
            float numberOfZombies = LevelManager.Instance.CurrentZombieCount();

            float newXScale = (this.DistanceSE * numberOfZombies) / (LevelManager.Instance.CurrentZombieLimit() * 2);

            if (numberOfZombies < LevelManager.Instance.CurrentZombieLimit())
            {
                zombieBarGO.transform.localScale = new Vector3(newXScale, yScale, zScale);
                zombieBarGO.transform.position = new Vector3(xpos - newXScale, ypos, zpos);
            }
        }

    }
}
