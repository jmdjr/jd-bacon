using UnityEngine;
using System;
using System.Collections;
using System.Linq.Expressions;

using Object = UnityEngine.Object;
using System.Collections.Generic;


public class CameraController : MonoBehaviour
{
    public enum TransitionModeStates
    {
        CameraRiggingAuto,
        ManualRiggingAuto,
        CameraRiggingManual,
        ManualRiggingManual,
        Player
    }

    #region Public Editor Variables
    public TransitionModeStates TransitionMode;
    public Object CameraRig;
    public List<ManualTransitionObject> ManualRiggingList = new List<ManualTransitionObject>();
    public float TransitionDuration = 2.5f;
    public float IdleDuration = 1.0f;   // Amound of time between each transition.
    public GameObject PlayerReference;
    public Transform OrientationFromPlayer;
    public bool RestartWhenDone = false;
    public float playerCameraSlag = 0.1f;
    #endregion

    #region Private Variables
    private bool atEndOfPositionList { get { return curTransformIndex == positionList.Count - 1; } }
    private List<Transform> positionList;
    private Transform curTransform;
    private ManualTransitionObject curManualTransition;
    private int curTransformIndex;
    private bool isDirty = false;
    private bool requestMade = false;
    #endregion

    #region Camera Follow Coroutines.
    IEnumerator FollowCameraRigTransitions()
    {
        while (RestartWhenDone || !atEndOfPositionList)
        {
            yield return StartCoroutine(MoveToNextCameraRigLocation());
            yield return StartCoroutine(this.transform.MoveWithConstantTimeTo(curTransform, TransitionDuration));
            yield return StartCoroutine(IdleForSeconds(IdleDuration));
        }
    }
    IEnumerator FollowManualTransitions() 
    {
        while (RestartWhenDone || !atEndOfPositionList)
        {
            if (this.TransitionMode == TransitionModeStates.ManualRiggingManual)
            {
                yield return StartCoroutine(IdleTillCameraMoveRequested());
            }
            else
            {
                yield return StartCoroutine(MoveToNextManualRigLocation());
            }

            yield return StartCoroutine(this.transform.MoveWithConstantTimeTo(curManualTransition.Position, curManualTransition.TransitioningToTime));
            yield return StartCoroutine(IdleForSeconds(curManualTransition.IdleTime));
        }

        yield return 0;
    }
    IEnumerator FollowPlayerTransitions()
    {
        Vector3 offset = OrientationFromPlayer.position - PlayerReference.transform.position;
        while(!this.isDirty)
        {
            Vector3 thisPos = this.transform.position;
            Vector3 playerNewPosition = PlayerReference.transform.position;
            playerNewPosition.z = OrientationFromPlayer.position.z;
            playerNewPosition.y += offset.y;
            playerNewPosition.x += offset.x;
            this.transform.position = Vector3.MoveTowards(thisPos, playerNewPosition, Vector3.Distance(thisPos, playerNewPosition) / playerCameraSlag);
            yield return 0;
        }

        yield return 0;
    }
    #endregion

    #region Utility Coroutines
    IEnumerator IdleForSeconds(float time)
    {
        yield return new WaitForSeconds(time);
    }
    IEnumerator IdleTillCameraMoveRequested()
    {
        while (!requestMade) { } // do nothing until a request has been made.

        yield return 0;
    }
    IEnumerator MoveToNextCameraRigLocation()
    {
        if (RestartWhenDone && atEndOfPositionList)
        {
            curTransformIndex = 0;
        }
        else
        {
            ++curTransformIndex;
        }

        curTransform = positionList[curTransformIndex];
        yield return 0;
    }
    IEnumerator MoveToNextManualRigLocation()
    {
        if (RestartWhenDone && atEndOfPositionList)
        {
            curTransformIndex = 0;
        }
        else
        {
            ++curTransformIndex;
        }

        curManualTransition = ManualRiggingList[curTransformIndex];
        yield return 0;
    }
    #endregion
    
    #region Public Request Functions
    /// <summary>
    /// Ceases the camera's motion.
    /// </summary>
    public void StopCameraMotion()
    {
        this.StopAllCoroutines();
    }

    /// <summary>
    /// Starts the camera's motion, based on a TransitionModeState.
    /// </summary>
    /// <param name="state">The state that determines how the camera will move.</param>
    public void StartCameraMotion(TransitionModeStates state)
    {
        this.TransitionMode = state;
        this.isDirty = true;
    }

    /// <summary>
    /// Requests the camera to move to a new transition. if a known name is provided, camera will move to that transform.
    /// </summary>
    /// <param name="name">a name of a transform</param>
    public void RequestCameraMotion(string name)
    {
        switch (this.TransitionMode)
        {
            case TransitionModeStates.CameraRiggingManual:
                if(name == "")
                {
                    StartCoroutine(this.MoveToNextCameraRigLocation());
                }
                else 
                { 
                    List<Transform> foundTransforms = this.positionList.FindAll(tr => tr.name == name);
                    if (foundTransforms.Count > 0)
                    {
                        this.curTransformIndex = this.positionList.IndexOf(foundTransforms[0]);
                        this.curTransform = this.positionList[this.curTransformIndex];
                    }
                }

                this.requestMade = true;
                break;

            case TransitionModeStates.ManualRiggingManual:
                if (name == "")
                {
                    StartCoroutine(this.MoveToNextManualRigLocation());
                }
                else
                {
                    List<ManualTransitionObject> foundTransforms = this.ManualRiggingList.FindAll(tr => tr.Position.name == name);
                    if (foundTransforms.Count > 0)
                    {
                        this.curTransformIndex = this.ManualRiggingList.IndexOf(foundTransforms[0]);
                        this.curManualTransition = this.ManualRiggingList[this.curTransformIndex];
                    }
                }

                this.requestMade = true;
                break;
        }
    }
    #endregion

    public void Awake()
    {
        curTransformIndex = 0;
        positionList = new List<Transform>();
        GameObject obj = CameraRig as GameObject;

        if (obj != null)
        {
            int childCount = obj.transform.GetChildCount();
            int iterator = 0;

            while (iterator < childCount)
            {
                positionList.Add(obj.transform.GetChild(iterator));
                ++iterator;
            }

            positionList.Sort((x, y) => string.Compare(x.name, y.name));
        }

        this.isDirty = true;
    }
    public void Update()
    {
        if (isDirty)
        {
            this.StopAllCoroutines();
            this.isDirty = false;

            switch (this.TransitionMode)
            {
                case TransitionModeStates.Player:
                    StartCoroutine(FollowPlayerTransitions());
                    break;

                case TransitionModeStates.ManualRiggingAuto:
                    StartCoroutine(FollowManualTransitions());
                    break;

                case TransitionModeStates.CameraRiggingAuto:
                    StartCoroutine(FollowCameraRigTransitions());
                    break;
                default:
                    break;
            }
        }
    }

    [System.Serializable]
    public class ManualTransitionObject
    {
        public Transform Position;
        public float IdleTime;
        public float TransitioningToTime;
    }
}