using UnityEngine;
using System;
using System.Collections;

using Object = UnityEngine.Object;
using System.Collections.Generic;

public class Level1StateManager : StateMachineSystem
{
    public Camera CameraReference;
    private CameraController cameraControl = null;
    private CameraController CameraControl 
    { 
        get 
        {
            if (CameraReference != null && cameraControl == null)
            {
                cameraControl = CameraReference.GetComponent<CameraController>();
            }

            return cameraControl;
        }
    }

    public GameObject Hero = null;

    StateMachine CameraSM;

    #region Camera
    
    #region States
    State IntroRoom = new State("Intro Room");
    State FirstRoom = new State("First Room");
    State SecondRoom = new State("Second Room");
    State ThirdRoom = new State("Third Room");
    State FourthRoom = new State("Fourth and Final Room");
    #endregion

    #region Start Actions
    private IEnumerator StartIntroRoomTransitionsAction()
    {
        CameraControl.StartCameraMotion(CameraController.TransitionModeStates.Player);
        yield return 0;
    }
    private IEnumerator StartFirstRoomTransitionsAction()
    {
        CameraControl.StartCameraMotion(CameraController.TransitionModeStates.CameraRiggingManual);
        CameraControl.IdleDuration = 0.0f;
        yield return 0;
    }
    private IEnumerator StartNextRoomTransitionsAction()
    {
        yield return 0;
    }
    #endregion
    
    #region Actions
    private IEnumerator TransitionToNextRoomAction()
    {
        Debug.Log("Request transition");
        CameraControl.RequestCameraMotion("");
        yield return 0;
    }
    private IEnumerator IntroRoomAction()
    {
        yield return 0;
    }
    private IEnumerator FourthRoomAction()
    {
        CameraControl.RequestCameraMotion("");
        CameraControl.TransitionFromRiggingToPlayerMode();
        yield return 0;
    }
    #endregion
    
    #region End Actions
    #endregion

    #region Conditions
    private bool nextRoom = false;
    private Object TransitioningTriggerer = null;
    private void TriggerToNextRoom(GameObject triggerer) 
    {
        if (triggerer != TransitioningTriggerer)
        {
            GameObject trigGO = triggerer as GameObject;
            if (trigGO != null)
            {
                Vector3 heroPosition = new Vector3(Hero.rigidbody.position.x, Hero.rigidbody.position.y, Hero.rigidbody.position.z);
                Vector3 closestpointOtherSide = trigGO.collider.ClosestPointOnBounds(trigGO.collider.transform.position + Vector3.right);
                Hero.rigidbody.MovePosition(closestpointOtherSide);
                trigGO.collider.isTrigger = false;
            }
            TransitioningTriggerer = triggerer;
            Debug.Log("To Next Room Trigger triggered!!!");
            nextRoom = true;
        }
    }
    private bool ToNextRoomCondition()
    {
        if (nextRoom)
        {
            nextRoom = false;
            Debug.Log("To Next Room Condition Called!!!");
            return true;
        }

        return false;
    }
    #endregion
    
    #endregion

    public void InitializeCameraSM() 
    {
        this.CameraSM = new StateMachine("CameraMachine", IntroRoom);

        IntroRoom.RepeatActionCount = 1;
        IntroRoom.RepeateIfDead = false;
        IntroRoom.Entering = this.StartIntroRoomTransitionsAction;
        IntroRoom.AddExitCondition(new ExitStateCondition(ToNextRoomCondition, FirstRoom));

        FirstRoom.RepeatActionCount = 1;
        FirstRoom.RepeateIfDead = false;
        FirstRoom.Entering = this.StartFirstRoomTransitionsAction;
        FirstRoom.Action = this.TransitionToNextRoomAction;
        FirstRoom.AddExitCondition(new ExitStateCondition(this.ToNextRoomCondition, this.SecondRoom));

        SecondRoom.RepeatActionCount = 1;
        SecondRoom.RepeateIfDead = false;
        SecondRoom.Entering = this.StartFirstRoomTransitionsAction;
        SecondRoom.Action = this.TransitionToNextRoomAction;
        SecondRoom.AddExitCondition(new ExitStateCondition(this.ToNextRoomCondition, this.ThirdRoom));

        ThirdRoom.RepeatActionCount = 1;
        ThirdRoom.RepeateIfDead = false;
        ThirdRoom.Entering = this.StartFirstRoomTransitionsAction;
        ThirdRoom.Action = this.TransitionToNextRoomAction;
        ThirdRoom.AddExitCondition(new ExitStateCondition(this.ToNextRoomCondition, this.FourthRoom));

        FourthRoom.RepeatActionCount = 1;
        FourthRoom.RepeateIfDead = false;
        FourthRoom.Entering = this.StartNextRoomTransitionsAction;
        FourthRoom.Action = this.FourthRoomAction;
    }

    protected override void InitializeStateManager()
    {
        base.InitializeStateManager();

        if (CameraControl != null)
        {
            InitializeCameraSM();
            this.MachineList.Add(CameraSM);
        }
    }
}
