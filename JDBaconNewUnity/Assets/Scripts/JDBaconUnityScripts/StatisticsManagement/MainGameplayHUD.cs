using UnityEngine;
using System;
using System.Collections;

using Object = UnityEngine.Object;
using System.Collections.Generic;

public class MainGameplayHUD : MonoBehaviour
{
    bool isPaused = false;
    float timescale;

    public void PauseGameplay()
    {
        StateMachineSystem[] StateMachineRefrences = (StateMachineSystem[])FindObjectsOfType(typeof(StateMachineSystem));
        foreach (StateMachineSystem sms in StateMachineRefrences)
        {
            sms.PauseStateMachineSystem();
        }
    }
}
