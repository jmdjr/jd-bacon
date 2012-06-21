using UnityEngine;
using System;
using System.Collections;

using Object = UnityEngine.Object;
using System.Collections.Generic;

public class SLerpTransitions : MonoBehaviour
{
    public Object CameraRig;
    public float TransitionDuration = 2.5f;

    // Amound of time between each transition.
    public float IdleDuration = 1.0f;    
   
    public Transform pos0;
    public Transform pos1;
    public Transform pos2;
    private List<Transform> transforms;
    private Transform curTransform;
    private int transformIndex;
    private float transitionCounter = 0.0f;

    IEnumerator IdleForSeconds(float time)
    {
        yield return new WaitForSeconds(time);
    }


    IEnumerator FollowTransitions()
    {
        int transPos = 0;

        while(transPos < transforms.Count)
        {
            MoveToNextLocation();
            yield return TransitionTo(curTransform);
        }
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

    public void Awake()
    {
        curTransform = pos0;

        transformIndex = 0;
        transforms = new List<Transform>();
        GameObject obj = CameraRig as GameObject;

        if(obj != null)
        {
            int childCount = obj.transform.GetChildCount();
            int iterator = 0;

            while (iterator < childCount)
            {
                transforms.Add(obj.transform.GetChild(iterator));
                ++iterator;
            }
        }
    }

    public void Update()
    {
        if (transitionCounter <= 0.0f)
        {
            transitionCounter = TransitionDuration;
            
            MoveToNextLocation();

            StartCoroutine(TransitionTo(curTransform));
        }

        this.transitionCounter -= Time.deltaTime;
    }

    public void MoveToNextLocation()
    {
        if (curTransform == pos0)
        {
            curTransform = pos1;
        }
        else if (curTransform == pos1)
        {
            curTransform = pos2;
        }
        else if (curTransform == pos2)
        {
            curTransform = pos0;
        }

    }
}
