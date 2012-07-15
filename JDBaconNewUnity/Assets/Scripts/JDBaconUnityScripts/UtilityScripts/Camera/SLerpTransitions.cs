using UnityEngine;
using System;
using System.Collections;
using System.Linq.Expressions;

using Object = UnityEngine.Object;
using System.Collections.Generic;

public class SLerpTransitions : MonoBehaviour
{
    public enum TransitionModeStates
    {
        CameraRigging,
        ManualRigging,
        Player
    }
    [System.Serializable]
    public class ManualTransitionObject
    {
        public Transform Position;
        public float IdleTime;
        public float TransitioningToTime;
    }

    public TransitionModeStates TransitionMode;
    public Object CameraRig;

    public List<ManualTransitionObject> ManualRigging = new List<ManualTransitionObject>();
    public float TransitionDuration = 2.5f;

    // Amound of time between each transition.
    public float IdleDuration = 1.0f;
    public bool RestartWhenDone = false;
    
    public Array stuffs;
    private List<Transform> positionList;
    private Transform curTransform;
    private int curTransformIndex;

    private bool atEndOfPositionList
    {
        get { return curTransformIndex == positionList.Count - 1; }
    }
    public void Awake()
    {
        curTransformIndex = 0;
        positionList = new List<Transform>();
        GameObject obj = CameraRig as GameObject;

        if(obj != null)
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

        switch (this.TransitionMode)
        {
            case SLerpTransitions.TransitionModeStates.ManualRigging:

                break;

            case SLerpTransitions.TransitionModeStates.Player:

                break;

            case SLerpTransitions.TransitionModeStates.CameraRigging:
            default:
                StartCoroutine(FollowTransitions());
                break;
        }
    }

    IEnumerator IdleForSeconds(float time)
    {
        yield return new WaitForSeconds(time);
    }
    IEnumerator FollowTransitions()
    {
        while (RestartWhenDone || !atEndOfPositionList)
        {
            yield return StartCoroutine(MoveToNextLocationList());
            yield return StartCoroutine(TransitionTo(curTransform));
            yield return StartCoroutine(IdleForSeconds(IdleDuration));
        }
    }
    IEnumerator FollowManualTransitions()
    {
        if (RestartWhenDone && atEndOfPositionList)
        {
            curTransformIndex = 0;
        }
        else
        {
            ++curTransformIndex;
        }

        //curTransform = ManualRigging[curTransformIndex].Position;
        yield return 0;
    }
    IEnumerator TransitionTo(Transform positionOrientation)
    {
        float t = 0.0f;
        Vector3 startingPos = transform.position;
        Quaternion startingOri = transform.rotation;

        Vector3 position = positionOrientation.position;
        Quaternion orientation = positionOrientation.rotation;

        if (position != Vector3.zero || orientation != Quaternion.identity)
        {
            while (t < 1.0f)
            {
                t += Time.deltaTime * (Time.timeScale / TransitionDuration);

                if (position != Vector3.zero)
                {
                    transform.position = Vector3.Lerp(startingPos, position, t);
                }

                if (orientation != Quaternion.identity)
                {
                    transform.rotation = Quaternion.Slerp(startingOri, orientation, t);
                }

                yield return 0;
            }
        }
    }
    IEnumerator MoveToNextLocationList()
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
}
